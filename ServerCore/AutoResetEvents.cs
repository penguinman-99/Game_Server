using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//3번째 방법. 직원에게 알려달라고 부탁하기.


namespace ServerCore
{
    class Lock
    {
        //bool <-> 커널
        AutoResetEvent _available = new AutoResetEvent(true);
        //ManualResetEvent _available = new ManualResetEvent(true);
        //manual은 권장안함. waitone reset이 atomic하지 않기 때문.
        //잠금 풀때까지 뺑뺑이
        public void Acquire()
        {
            //커널까지 가서 요구하는게 어마어마하게 오래거린다.
            //spinlock에 비해 느림
            _available.WaitOne();//입장 시도, 톨게이트   
            //_available.Reset();//manualresetevent에선 이것도 넣어줘야함, 문을 닫는 개념, autoreset에선 자동으로해줌.
        }
        public void Release()
        {
            _available.Set();//flag=true
        }
    }
    class AutoResetEvents
    {
        static int _num = 0;
        static Lock _lock = new Lock();
        //mutex도 한 방법이 될 수있다. 메소드는 똑같이 waitone, releasemutex..
        //mutex도 커널동기화 객체임. autoresetevent와 다르게 많은 정보를 가짐(잠금 횟수에 따른 푸는 횟수 변경, 쓰레드 이름으로 에러 잡아주기도 함.)

        static void Thread_1()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }
        static void Thread_2()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
            }
        }
        //스핀락: lock으로 잠겼을 경우 풀릴때까지 기다리는 것.(CPU 점유율이 높아짐)
        //기존 코드는 atomic하지 않아 동시에 점유하면서 교착 상태에 빠질 우려가 있음.
        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();
            Task.WaitAll(t1, t2);
            Console.WriteLine(_num);
        }
    }
}
