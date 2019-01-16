using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Net;
using System.Net.Sockets; 

namespace TestLisen
{ 
    class SocketAcceptedEventHandler : EventArgs
    {
        public Socket AcceptedSocket
        {
            get;
            private set;
        }
        public SocketAcceptedEventHandler(Socket s)
        {
            AcceptedSocket = s;
        }
    }
      
    class Listener
    {
        Socket MainSocket;
        public event EventHandler<SocketAcceptedEventHandler> SocketAcceptedE_Handler;
  
        public static bool Listening
        {
            get;
            private set;
        }

        public int Port
        {
            get;
            private set;
        }

        public Listener(int port)
        {
            Port = port;
            MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start()
        {
            if (Listening)
                return;

            MainSocket.Bind(new IPEndPoint(IPAddress.Any, Port));
            MainSocket.Listen(2);
            MainSocket.BeginAccept(callback, null); 

            Listening = true;
        }

        private void callback(IAsyncResult ar)
        {
            try
            { 
                Socket CS = MainSocket.EndAccept(ar);

                if (SocketAcceptedE_Handler != null)
                {
                    SocketAcceptedE_Handler(this, new SocketAcceptedEventHandler(CS)); 
                    Send(CS, FileHandlercs.GetResponseMessage()); 
                }

                MainSocket.BeginAccept(callback, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void Send(Socket CS, string data)
        { 
            byte[] byteData = Encoding.Default.GetBytes(data); 
            CS.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), CS);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {  
                Socket CS = (Socket)ar.AsyncState; 
                int bytesSent = CS.EndSend(ar); 

                Stop(CS);  
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void Stop(Socket CS)
        {
            if (!Listening)
                return;

            CS.Close();
            CS.Dispose(); 

            foreach (Socket s in Program.getSockets())
            {
                if (s == CS)
                {
                    Program.getSockets().Remove(CS); 
                    break;
                }
            }
        }

        public void EndListener()
        {
            if (!Listening)
                return;

            MainSocket.Close();
            MainSocket.Dispose();
        } 

    }
}
