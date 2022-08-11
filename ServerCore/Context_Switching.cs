using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

/*
 * 실제론 쓰레드가 프로세스 끼리 바로 왔다갔다하는게 아닌, 커널 모드를 거친 후 다른 프로세스로 옮겨간다.
 * 
 * 
 */
namespace ServerCore
{
    class SpinLock
    {
        volatile int _locked = 0;
        //잠금 풀때까지 뺑뺑이
        public void Acquire()
        {
            while (true)
            {

                //int original = Interlocked.Exchange(ref _locked, 1);
                //if (original == 0)
                //    break;
                //CAS compare-and-swap
                int expected = 0;
                int desired = 1;

                if (Interlocked.CompareExchange(ref _locked, desired, expected)==expected)
                    break;

                //Thread.Sleep(1);//무조건 1ms 휴식 다만 실제론 운체에서 스케줄러가 정하는편
                //Thread.Sleep(0);//조건부 양보 나보다 낮은 우선순위가진 얘한텐 양보X==그런얘들없으면 다시 나한테. 다만 기아현상 발생우려
                Thread.Yield();//관대한 양보==sleep 0보단 널널한편. 실행할 수 있는 쓰레드 있으면 그 쓰레드 실행. 그런 얘들이 없으면 다시 나한테

            }
        }
        public void Release()
        {
            _locked = 0;

        }
    }
    class Context_Switching
    {
        static int _num = 0;
        static SpinLock _lock = new SpinLock();
        static void Thread_1()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }
        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
            }
        }
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
