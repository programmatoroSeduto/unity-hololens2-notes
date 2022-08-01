using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;

namespace SocketsTest
{
    class NetTalker
    {
        // crea una connessione coun un determinato IP
        //    IPEnd : default "" -> connessione loopback, altrimenti connessione
        //        ad un altro indirizzo
        //    portno : il numero di porta a cui connettersi (default 5901)
        public Socket ConnectTo(string IPEnd = "", int portno = 5901)
        {
            // from this machine
            IPHostEntry ipHost;
            if (IPEnd == "")
                ipHost = Dns.GetHostEntry(Dns.GetHostName());
            else
                ipHost = Dns.GetHostEntry(IPEnd);
            IPAddress ipAddr = ipHost.AddressList[0];

            // socket
            Socket s = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // to the endpoint
            IPEndPoint endPoint = new IPEndPoint(ipAddr, portno);

            // connect
            s.Connect(endPoint);

            return s;
        }

        // simple talker
        static void Main(string[] args)
        {
            Console.WriteLine("ECHO TALKER");

            Console.WriteLine("Opening connection to '127.0.0.1' port 5000");
            NetTalker talk = new NetTalker();
            Socket s = talk.ConnectTo("127.0.0.1", 5000);

            Console.WriteLine("checking connection...");
            if (!s.Connected)
            {
                Console.WriteLine("Unable to connect!");
                return;
            }
            else
                Console.WriteLine("Connected.");

            int count = 9;
            Console.WriteLine("Sending {0} messages to the listener...", count);

            while (count > 0)
            {
                byte[] buffer = new byte[16];
                string st = "---cheers---";

                Console.WriteLine("(NO.{0}) sending '{1}'...", count, st);
                buffer = Encoding.ASCII.GetBytes(st);
                int countBytes = s.Send(buffer, st.Length, SocketFlags.None);
                Console.WriteLine("(NO.{0}) sent {1} bytes", count, countBytes);

                Console.WriteLine("receiving echo ...");
                countBytes = s.Receive(buffer);
                string msg = Encoding.ASCII.GetString(buffer).Replace("\0", "");
                Console.WriteLine("(NO.{0}) received '{1}' of len {2} bytes", count, msg, countBytes);

                --count;
            }

            s.Shutdown(SocketShutdown.Both);
            s.Close();

            Console.WriteLine("DONE closing...");
            Console.ReadKey();
        }
    }
}
