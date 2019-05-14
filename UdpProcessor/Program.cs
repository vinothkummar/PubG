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
            s.Server("0.0.0.0", 9011);
            //s.Server("10.100.108.42", 6011);
            Console.ReadKey();
        }
    }
}
