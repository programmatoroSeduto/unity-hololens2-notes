using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketsTest
{
    class NetTools
    {
        public string DisplayLocalHostName()
        {
            try
            {
                // Get the local computer host name.
                String hostName = Dns.GetHostName();
                // Console.WriteLine("Computer name :" + hostName);

                return hostName;
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }

            return "";
        }

        public IPHostEntry getEntryOf(string hostAddr)
        {
            return Dns.GetHostEntry(hostAddr);
        }
    }
}
