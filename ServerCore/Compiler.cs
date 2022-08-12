using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Compiler
    {
        //static: 모든 쓰레드가 공유함. 
        static bool _stop = false;
        static void ThreadMain()
        {
            Console.WriteLine("쓰레드 시작");
            while (_stop == false)
            {
                //stop신호를 기다림 
            }
            Console.WriteLine("쓰레드 끝");


        }
        static void Main(string[] args)
        {
            Task t = new Task(ThreadMain);
            t.Start();
            Thread.Sleep(1000);
            _stop = true;
            Console.WriteLine("Stop 호출");
            Console.WriteLine("종료 대기");
            t.Wait();//Thread join과 같음
            Console.WriteLine("종료");
        }
    }
}
