using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TestLisen
{

       
    class ServerS
    {

        Socket _socket;
        byte[] _buffer = new byte[1024];
        public ServerS()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Bind(int port)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Lisen(int backlog)
        {
            _socket.Listen(backlog);
        }

        public void Accept()
        {
            _socket.BeginAccept(AcceptCallback,null);
        }

        private void AcceptCallback(IAsyncResult result)
        {
            Socket clientSocket = _socket.EndAccept(result); 
            _buffer = new byte[1024];
            clientSocket.BeginReceive(_buffer,0,_buffer.Length,SocketFlags.None,ReceiveCallback,clientSocket);
            Accept();
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            Socket clientSocket = result.AsyncState as Socket;
            int bufferSize = clientSocket.EndReceive(result);
            byte[] packet = new byte[bufferSize];
            Array.Copy(_buffer, packet, packet.Length);

            //handle

            clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);

            /*
            string FileText = "123456789";

            var request_url = "GET HTTP/1.1\r\n" +
            "Content-Length:" + FileText.Length + "\r\n" +
            "\r\n\r\n" +
            FileText;

            byte[] buffer = Encoding.Default.GetBytes(request_url);
            clientSocket.Send(buffer, 0, buffer.Length, 0);//SocketFlags.None

            buffer = new byte[255];
            int receive = clientSocket.Receive(buffer, 0, buffer.Length, 0);//SocketFlags.None

            Array.Resize(ref buffer, receive);

            //Console.WriteLine("rec: {0}", Encoding.Default.GetString(buffer));

            clientSocket.Close();*/
        }
    }
}
