using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    //메모리 배리어
    //코드 재배치 억제
    //1)FullMemory Barrier(ASM MFENCE, C# Thread.MemoryBarrier) Store Load 둘다 막음
    //2)Store Memory Barrier SFENCE, Store만 막음 
    //3) Load Memory Barrier LFENCE Load만 막음.
    //가시성(?) 고급식당으로 돌아가자
    /*직원 두명 첫번째:주문현황판엔 안올리고 수첩에다가 콜라 적음
     * 다른 직원은 콜라를 주문한걸 몰라. 사이다!! 
     * 가시성:첫번째 직원이 주문받은걸 다른 직원들도 바로 볼 수 있는가?
     * 언젠가는 콜라 수첩을 실제 주문판에다가 놔야한다.
     * 그럼 두번째 직원 수첩에도 콜라를 적어놓으면 똑같아진다
     * 
     * 
     * 
     * 
     * 
     */
    class MemoryBarrier
    {
        static int x = 0;
        static int y = 0;
        static int r1 = 0;
        static int r2 = 0;
        static void Thread_1()
        {
            y = 1;
            //Store y
            Thread.MemoryBarrier();
            //더이상 r1=x 먼저 실행 안됨.
            //store후 메모리 배리어: 물내리기
            //Load디기전: 따끈따끈한 실제값 
            r1 = x;//Load x
        }
        static void Thread_2()
        {
            x = 1;//store x
            Thread.MemoryBarrier();
            //이렇게하면 둘다 0이 될 일이 아예 없다.
            r2 = y;//Load y

        }
        static void Main(string[] args)
        {
            int count = 0;
            while (true)
            {
                count++;
                x = y = r1 = r2 = 0;
                //모두 0으로 밀기
                Task t1 = new Task(Thread_1);
                Task t2 = new Task(Thread_2);
                t1.Start();
                t2.Start();
                Task.WaitAll(t1, t2);//끝날떄까지 메인 쓰레드는 대기
                if (r1 == 0 && r2 == 0)
                {

                    break;
                }
            }
            Console.WriteLine($"{count}번만에 빠져나왔다");
        }
        //컴파일러가 제멋대로 하는걸 막아주엇다
        //하드웨어도 이짓거리를 하는게 문제. 
    }
}
