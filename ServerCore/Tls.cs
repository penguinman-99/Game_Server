using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//일감이 많게 큐 저장시 하나만 꺼내는게 아니라 뭉탱이로 뽑아다가 내 공간에 넣어두고 사용
namespace ServerCore
{
    class Tls
    {
        //lock없이 부담없이 쓸수있다. 내 영역이야!!
        static ThreadLocal<string> ThreadName=new ThreadLocal<string>(()=> { return $"My Name Is {Thread.CurrentThread.ManagedThreadId}"; });
        //static string ThreadName;
        static void WhoAmI()
        {
            bool repeat = ThreadName.IsValueCreated;
            if (repeat)
            {

                Console.Write(ThreadName.Value+"(repeat)");
            }
            else
            {
                Console.Write(ThreadName.Value);
            }
            //ThreadName.Value = $"My Name Is {Thread.CurrentThread.ManagedThreadId}";
            //Thread.Sleep(1000);

        }

        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(3, 3);
            //인자만큼 task 실행
            Parallel.Invoke(WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI);
            ThreadName.Dispose();
        }
    }
}
