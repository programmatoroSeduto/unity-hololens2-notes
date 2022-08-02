using System;
using Windows;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Networking;
using Windows.Networking.Sockets;
// vedi
//    C:\Program Files (x86)\Windows Kits\10\UnionMetadata\10.0.19041.0\winmd
// per usare await:
//    C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETCore\v4.5\System.Runtime.WindowsRuntime.dll
//    https://stackoverflow.com/questions/44099401/frombluetoothaddressasync-iasyncoperation-does-not-contain-a-definition-for-get

namespace NetworkingTalker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine("Opening connection to '127.0.0.1' port 5000");
            NetworkTalker talk = new NetworkTalker();
            StreamSocket s = talk.ConnectTo("127.0.0.1", 5000);

            int count = 9;
            Console.WriteLine("Sending {0} messages to the listener...", count);

            while(count > 0)
            {
                byte[] buffer = new byte[16];
                string st = "---cheers---";

                using (var dw = new DataWriter(s.OutputStream))
                {
                    Console.WriteLine("(NO.{0}) sending '{1}'...", count, st);
                    dw.WriteUInt32(dw.MeasureString(st));
                    dw.WriteString(st);
                    dw.FlushAsync().AsTask().Wait();
                    Console.WriteLine("(NO.{0}) message sent", count);
                }

                using (var dr = new DataReader(s.InputStream))
                {
                    Console.WriteLine("(NO.{0}) receiving echo ...", count);
                    uint countBytes = dr.ReadUInt32();
                    string msg = dr.ReadString(countBytes);
                    Console.WriteLine("(NO.{0}) received '{1}'", count, msg);
                }
            }

            s.Dispose();
            Console.WriteLine("Done. Closing...");
            Console.ReadKey();
        }
    }
}
