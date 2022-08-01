using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace SocketsTest
{
    class NetListener
    {
        // crea un socket in ascolto su una certa porta
        public Socket ListenOn(int portno = 5901)
        {
            // indirizzo dell'host attuale
            IPHostEntry ipHost = Dns.GetHostEntry("127.0.0.1");
            IPAddress ipAddr = ipHost.AddressList[0];

            // socket
            Socket s = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // bind
            IPEndPoint endPoint = new IPEndPoint(ipAddr, portno);
            s.Bind(endPoint);

            // listen
            s.Listen(10);

            return s;
        }

        // simple listener main
        static void Main(string[] args)
        {
            Console.WriteLine("ECHO LISTENER");

            Console.WriteLine("Opening connection to '127.0.0.1' port 5000");
            NetListener listen = new NetListener();
            Socket s = listen.ListenOn(5000);

            Console.WriteLine("waiting for any incoming connection...");
            Socket insock = s.Accept();

            /*
            if(!insock.Connected)
            {
                // https://stackoverflow.com/questions/14940553/a-request-to-send-or-receive-data-was-disallowed-because-the-socket-is-not-conne
                Console.WriteLine("fake connection received, closing...");
                return;
            }
            */

            Console.WriteLine("INCOMING CONNECTION! from endpoint {0}",
                insock.RemoteEndPoint.ToString());

            int count = 9;
            while (count > 0)
            {
                Console.WriteLine("creating ECHO thread...");
                Task t = new Task(() => listen.ServerEcho(insock));
                t.Start();
                Console.WriteLine("(MAIN) waiting ECHO ...");
                t.Wait();

                count -= 1;
                Console.WriteLine("Remaining {0}", count);
            }

            Console.WriteLine("DONE closing listener...");
            insock.Shutdown(SocketShutdown.Both);
            insock.Close();

            Console.WriteLine("DONE closing...");
            Console.ReadKey();
        }

        public void ServerEcho(Socket s)
        {
            Console.WriteLine("(ECHO) thread ECHO spawned!");

            Console.WriteLine("(ECHO) waiting to receive...");
            byte[] buffer = new byte[16];
            int countBytes = s.Receive(buffer);
            string msg = Encoding.ASCII.GetString(buffer).Replace("\0", "");
            Console.WriteLine("(ECHO) received '{0}' bytes={1} len={2}", msg, countBytes, msg.Length);

            Console.WriteLine("(ECHO) sending again...");
            buffer = Encoding.ASCII.GetBytes(msg);
            countBytes = s.Send(buffer, msg.Length, SocketFlags.None);
            Console.WriteLine("(ECHO) sent bytes={0} done.", countBytes);
        }
    }
}
