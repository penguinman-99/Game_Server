using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ServerCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost=Dns.GetHostEntry(host);
            //첫번째로 알려준 주소
            IPAddress ipAddr=ipHost.AddressList[0];
            //식당 주소, 식당 문의 번호
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            //문지기의 휴대폰
            //www.rookiss.com->??????
            Socket listenSocket = new Socket(endPoint.AddressFamily,SocketType.Stream,ProtocolType.Tcp);
            //소켓, 프로토콜 타입
            try
            {
                //문지기 교육
                listenSocket.Bind(endPoint);
                //영업 시작
                //backlog: 최대 대기수.
                listenSocket.Listen(10);

                //식당 여는 동안은 무한 루프
                while (true)
                {
                    Console.WriteLine("Listening...");

                    //손님을 입장시킨다.
                    Socket clientSocket = listenSocket.Accept();

                    //받는다
                    byte[] recvBuff = new byte[1024];
                    int recvBytes = clientSocket.Receive(recvBuff);
                    //string 변환
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                    Console.WriteLine($"From Client{recvData}");
                    //보낸다
                    byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to mmorpg server");
                    clientSocket.Send(sendBuff);
                    //쫓아내기
                    clientSocket.Shutdown(SocketShutdown.Both);//듣기도 싫고 말하기도 싫어

                    clientSocket.Close();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            

        }
    }
}
