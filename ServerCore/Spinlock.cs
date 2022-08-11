using System;
using System.Threading;
using System.Threading.Tasks;
//원자성을 충족하지 못해, 동시에 들어가 기다리는 현상이 발생할 수 있다.

namespace ServerCore
{
    class SpinLock
    {
        //쓰레드에 의해 수정될 수 있음을 명시
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
                //expected와 lock이 같다면..locked에 desired 넣어줌.
                //자물쇠,바꾸고자하는값,locked와 비교.
                //original_value는 바꾸기 전 locked의 값이다. 일단 내가 바꿀건데, 0인 상황이면 그땐 내가 쓸 수 있다는 뜻.
                //
                if (Interlocked.CompareExchange(ref _locked,desired,expected) == expected)
                    break;
            }
        }
        public void Release()
        {
            _locked = 0;

        }
    }
    class Program
    {
        static int _num = 0;
        static SpinLock _lock = new SpinLock();
        static void Thread_1()
        {
            for(int i = 0; i < 100000; i++)
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
