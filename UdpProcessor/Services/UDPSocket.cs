using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Fanview.UDPProcessor.Services
{
    public class UDPSocket
    {
        private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private const int bufSize = 100 * 1024;       
        private State state = new State();
        private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 9011);
        private AsyncCallback recv = null;
        private IMatchSummaryRepository _matchSummaryRepository;
        private IPlayerKillRepository _playerKillRepository;
        private Queue<JObject[]> _eventMessages;

        public UDPSocket()
        {
            var servicesProvider = ServiceConfiguration.BuildDI();

            _matchSummaryRepository = servicesProvider.GetService<IMatchSummaryRepository>();

            _playerKillRepository = servicesProvider.GetService<IPlayerKillRepository>();

            _eventMessages = new Queue<JObject[]>();
        }

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public async void Server(string address, int port)
        {
            //_socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            Console.WriteLine("bound on {0} at port {1}", address, port);
           Receive();
        }

        public void Client(string address, int port)
        {
            _socket.Connect(IPAddress.Parse(address), port);
            Console.WriteLine("connected on {0} at port {1}", address, port);
            Receive();
        }

        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndSend(ar);
                Console.WriteLine("SEND: {0}, {1}", bytes, text);
            }, state);
        }

        private async void Receive()
        {

            var fileName = string.Format("telemetry-data-log-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);

            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                State so = (State)ar.AsyncState;

                int bytes = _socket.EndReceiveFrom(ar, ref epFrom);

                byte[] packet = new byte[bytes];

                Array.Copy(so.buffer, packet, packet.Length);

                var receivedData = Encoding.UTF8.GetString(packet);

                Console.WriteLine("RECV: {0}: {1}, {2}", epFrom.ToString(), packet, receivedData);

                var objects = Deserializeobjects(receivedData);

                var array = objects.ToArray();               

                _eventMessages.Enqueue(array);

                var eventTime = DateTime.UtcNow;

                while (_eventMessages.Count >= 1)
                {
                    var message = _eventMessages.Dequeue();

                    Task.Run(async () => _playerKillRepository.InsertLiveKillEventTelemetry(message, fileName, eventTime));
                    Task.Run(async () => _matchSummaryRepository.InsertLiveEventMatchStatusTelemetry(message, fileName, eventTime));

                }



                    so.buffer = new byte[bufSize];

                _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);

            }, state);


        }

        private IEnumerable<JObject> Deserializeobjects(string jsonInput)
        {
            var serializer = new JsonSerializer();
            using (var strReader = new StringReader(jsonInput))
            {
                using (var jsonReader = new JsonTextReader(strReader))
                {
                    jsonReader.SupportMultipleContent = true;
                    while (jsonReader.Read())
                    {
                        yield return (JObject)serializer.Deserialize(jsonReader);
                    }
                }
            }
        }
    }
}
