using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {//DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            //첫번째로 알려준 주소
            IPAddress ipAddr = ipHost.AddressList[0];
            //식당 주소, 식당 문의 번호
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);
            //휴대폰 설정
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(endPoint);
                //연결한 반대쪽 대상
                Console.WriteLine($"Connected to {socket.RemoteEndPoint.ToString()}");
                //보낸다
                byte[] sendBuff = Encoding.UTF8.GetBytes("Hello world");
                int sendBytes = socket.Send(sendBuff);

                //받는다
                byte[] recvBuff = new byte[1024];
                int recvBytes = socket.Receive(recvBuff);
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                Console.WriteLine($"From server:{recvData}");
                //나간다

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            //문지기한테 문의
            
        }
    }
}
