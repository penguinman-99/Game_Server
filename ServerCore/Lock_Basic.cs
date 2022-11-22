using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ServerCore
{
    class Lock_Basic
    {
        //1. 전역변수 메모리 값을 레지스터에 갖고오고, 레지스터 값을 증가시키고, 그 값을 메모리 값에 넣는다
        static object _obj = new object();
        static int number = 0;
        static void Thread_1()
        {

            for (int i = 0; i < 10000; i++)
            {
                /*
                 * Mutual Exclusive 상호배제
                 */
                Monitor.Enter(_obj);
                number++;
                //블록 내부는 사실상 single Thread다.
                //동시 다발적으로 쓰레드 접근시 문제발생 Critical Section 임계영역
                //Lock
                //monitor exit을 안하고 나가버리면 영원히 잠겨버림
                //Thread 2에서 접근을 못해 하염없이 대기
                //데드락 상황
                //수동으로 exit를 해줘야한다. 예외처리로 한다해도 번거로움
                //직접 monitor로 하는 경우는 없고 lock을 쓸거임 
                //
                /*
                 * lock(_obj){
                 * number++; 끗
                 * }
                 * 끝나면 알아서 열어준다.
                 */
                Monitor.Exit(_obj);
                //화장실에서 나간다 잠금을 푼다 
                
            }
        }
        static void Thread_2()
        {
            for (int i = 0; i < 10000; i++)
            {

                Monitor.Enter(_obj);
                number--;
                Monitor.Exit(_obj);

            }
        }
        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();
            Task.WaitAll(t1, t2);
            Console.WriteLine(number);
        }
    }
}