using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    
    class Threads
    {
        static void MainThread(object _obj)
        {
            for(int i=0;i<5;i++)
                Console.WriteLine("Hello Thread");
        }
        static void Main(string[] args)
        {
            
            //Task도 ThreadPool 넣어 관리
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(5, 5);
            //직원이 할 일감을 정의
            
            for (int i = 0; i < 5; i++)
            {
                //TaskCreationOptions.LongRunning을 이용하면 별도로 관리할 수 있다.
                Task t = new Task(() => { while (true) { } }, TaskCreationOptions.LongRunning);
                t.Start();
            }
            ThreadPool.QueueUserWorkItem(MainThread);
            while (true)
            {

            }
        }
    }
}
