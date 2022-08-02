using System;
using System.Collections.Generic;
using System.Text;
using Windows;
using Windows.Networking;
using Windows.Networking.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkingTalker
{
    class NetworkTalker
    {
        public StreamSocket ConnectTo(string address, int portno)
        {
            // destination host
            HostName dest = new HostName(address);

            // socket
            StreamSocket s = new StreamSocket();

            // connect
            s.ConnectAsync(dest, portno.ToString()).AsTask().Wait();

            return s;
        }


    }
}
