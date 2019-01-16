using System;
using System.Collections.Generic; 
using System.Net.Sockets; 

namespace TestLisen
{ 
    class Program
    { 
        static Listener l;
        static List<Socket> sockets;

        static void SocketAccepted(object sender, SocketAcceptedEventHandler eHandler)
        { 
            sockets.Add(eHandler.AcceptedSocket); 
        }

        public static List<Socket> getSockets()
        {
            return sockets;
        } 

        static void Main(String[] args)
        { 
            int TPort = 0; 
            while (TPort == 0) 
            {
                Console.WriteLine("Enter port number:");
                string portString = Console.ReadLine();

                Int32.TryParse(portString, out TPort);
                if (TPort == 0)
                {
                    Console.WriteLine(portString == "0" ? "Port cant be 0." : "Bad port, not a number.");
                }
            }
            Console.WriteLine();
            Console.WriteLine("Enter Document Root path (to change it - restart application):"); 
            FileHandlercs.SetDir(Console.ReadLine());

            Console.WriteLine("");
            Console.WriteLine("Starting server...");

            l = new Listener(TPort);
            l.SocketAcceptedE_Handler += new EventHandler<SocketAcceptedEventHandler>(SocketAccepted);
            l.Start();
            sockets = new List<Socket>();

            Console.ReadLine();
            l.EndListener();

        } 
    }
}
