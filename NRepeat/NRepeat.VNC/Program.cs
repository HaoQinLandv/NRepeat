﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NRepeat.VNC
{
    class Program
    {
        public static TcpVNCRepeater Proxy;
        static void Main(string[] args)
        {
            // Create a localhost definition if the debugger is attached
            var definition = new ProxyDefinition() { ServerAddress = IPAddress.Any };

            if (Debugger.IsAttached)
            {
                definition.ServerAddress = IPAddress.Any;
                definition.ServerPort = 4501;
            }
            else
            {
                Console.WriteLine("Enter server port:");
                var port = Console.ReadLine();
                if (!String.IsNullOrEmpty(port))
                {
                    definition.ServerPort = Convert.ToInt16(port);
                }

            }

            // Create a new VNC Repeater
            Proxy = new TcpVNCRepeater(definition.ServerPort, definition.ServerAddress);
            Proxy.Start();

            Console.WriteLine("Repeater started on {0}:{1}", definition.ServerAddress, definition.ServerPort);

            // Proxy.BytesTransfered += Proxy_BytesTransfered;
            // Proxy.ServerDataSentToClient += Proxy_ServerDataSentToClient;
            // Proxy.ClientDataSentToServer += Proxy_ClientDataSentToServer;

            Console.WriteLine("Press any key to stop repeater");
            Console.ReadLine();
            Proxy.Stop();
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();

        }

        static void Proxy_BytesTransfered(object sender, ProxyByteDataEventArgs e)
        {
            Console.WriteLine("{0} : {1} sent {2}", DateTime.Now, e.Source, Encoding.ASCII.GetString(e.Bytes));
        }

        static void Proxy_ClientDataSentToServer(object sender, ProxyDataEventArgs e)
        {
            Console.WriteLine("{0} : Client sent {1} bytes to Server", DateTime.Now, e.Bytes);
        }

        static void Proxy_ServerDataSentToClient(object sender, ProxyDataEventArgs e)
        {
            Console.WriteLine("{0} : Server sent {1} bytes to Client", DateTime.Now, e.Bytes);
        }
    }
}
