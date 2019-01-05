using Fanview.API.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using Fanview.UDPProcessor.Services;


namespace Fanview.UDPProcessor
{
    public class Program
    { 
        static void Main(string[] args)
        {
            UDPSocket s = new UDPSocket();
            s.Server("0.0.0.0", 6011);
            //s.Server("192.168.0.64", 6011);
            Console.ReadKey();
        }
    }
}
