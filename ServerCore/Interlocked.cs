using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ServerCore
{
    class Interlock
    {
        //컴파일러나 하드웨어 관련은 큰 문제 X
        /*
         * lock, atomic으로 충분히 해결
         * 공유변수 접근에 대한 문제
         */
        static int number = 0;
        static void Thread_1()
        {

            for (int i = 0; i < 10000; i++)
            {
                //특정 변수를 atomic하게 증가시키다. 성능 손해
                //ref: number가 뭔지 알지 못하지만 참조하여 수치를 1 증가시킨다. 
                Interlocked.Increment(ref number);
                //컴파일러 최적화 문제는 발견되지 않음
                //number++; 실행이 되거나, 실행이 안되거나
            }
        }
        static void Thread_2()
        {
            for (int i = 0; i < 10000; i++)
            {
                Interlocked.Decrement(ref number);
                //number--;
            }
        }
        static void Main(string[] args)
        {
            /*
             * 
             * number++은 어떻게 작동하나?
             * 어셈블리 코드로 가면 사실 3줄로 동작
             * int temp=number;
             * temp+=1;
             * number=temp;
             * 동시다발로 들어가면 
             * 0으로 시작하다가 T1은 1, T2는 -1
             * 누가 먼저 실행될지는 모름
             * 단계별로 실행되므로 문제가 생기는것
             * 원자성! atomic. CPU 한 단위에 작업이 처리되는것
             * 골드-=100
             * 인벤+=검->원자적으로 일어나야함  왜?
             * 이사이에 서버 크래쉬나면 샀는데 검이 없는버그
             * 
             */
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();
            Task.WaitAll(t1, t2);
            Console.WriteLine(number);
            //잘 된다. 0
            /*
             * 10만번하면 0이 아니게 된다. 
             * 경합 조건을 다뤄보자(Race condition
             * 내 메모장에만 적지말고 주문현황판에도 업로드
             * (다른 직원도 주문현황 볼 수있게)
             * 콜라를 2번 테이블 - 냉장고에 꺼내주면됨
             * 3명 직원이 각자 콜라를 하나씩 꺼내옴
             * 2번테이블에 콜라 3개
             */
        }
    }
}
