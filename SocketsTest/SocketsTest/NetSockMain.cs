using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace SocketsTest
{
    class NetSockMain
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            NetTools nt = new NetTools();
            Console.WriteLine("(GetHostName) host name: {0}", nt.DisplayLocalHostName());

            string addr = "127.0.0.1";
            IPHostEntry h = nt.getEntryOf(addr);
            Console.WriteLine("(IHostEntry) name of the host '{0}': {1}", addr, h.HostName);
            Console.WriteLine("(IHostEntry) Addresses: {0}", h.AddressList);
        }
    }
}