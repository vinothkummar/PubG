﻿using Fanview.API.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using UdpProcessor.Services;


namespace UdpProcessor
{
    public class Program
    { 
        static void Main(string[] args)
        {
            UDPSocket s = new UDPSocket();
            s.Server("127.0.0.1", 6011);
            //s.Server("192.168.0.64", 6011);
            Console.ReadKey();
        }
    }
}
