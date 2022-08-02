using System;
using System.Collections.Generic;
using System.Text;
using Windows;
using Windows.Networking;
using Windows.Networking.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace NetworkingListener
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ECHO LISTENER");

            Console.WriteLine("starting the listener (127.0.0.1 portno=5000)...");
            NetworkListener nl = new NetworkListener();
            StreamSocketListener s =  nl.ListenOn("127.0.0.1", 5000);

            Console.WriteLine("Waiting for the end of the transmission");
            while (!nl.endOfTransmission) 
            {
                Thread.Sleep(2000);
            }

            Console.WriteLine("End, closing...");
        }
    }
}
