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
    class NetworkListener
    {
        public bool endOfTransmission = false;

        public StreamSocketListener ListenOn(string address, int portno)
        {
            // socket
            StreamSocketListener s = new StreamSocketListener();

            // callback
            s.ConnectionReceived += this.Echo;

            // bind listen
            HostName host = new HostName(address);
            s.BindEndpointAsync(host, portno.ToString()).AsTask().Wait();

            return s;
        }

        // la callback
        void Echo(
            StreamSocketListener listener,
            StreamSocketListenerConnectionReceivedEventArgs args)
        {
            listener.Dispose();
            StreamSocket s = args.Socket;

            Console.WriteLine("(ECHO) received incoming connection!");

            int count = 9;
            while (count > 0)
            {
                Console.WriteLine("(ECHO) number {0} echo", count);

                string msg;
                using (var dr = new DataReader(s.InputStream))
                {
                    Console.WriteLine("(ECHO) receiving the message...");
                    uint countBytes = (uint) dr.ReadInt32();
                    msg = dr.ReadString(countBytes);
                    Console.WriteLine("(ECHO) received '{0}'", msg);
                }

                using (var dw = new DataWriter(s.OutputStream))
                {
                    Console.WriteLine("(ECHO) message echo...");
                    dw.WriteUInt32(dw.MeasureString(msg));
                    dw.WriteString(msg);
                    Console.WriteLine("(ECHO) echo done");
                }

                --count;
            }

            Console.WriteLine("(ECHO) end of the transmission!");
            endOfTransmission = true;
        }
    }
}
