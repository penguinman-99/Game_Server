using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ServerCore
{

    class SessionManager
    {

        static object _lock = new object();
        public static void TestSession()
        {
            lock (_lock)
            {

            }

        }
        //쓰레드에서 Test를 호출할때, 먼저 자기 자신을 lock한다. 이후 상대 manager의 메소드를 호출한다.
        //동시에 호출된 경우 서로의 자물쇠를 잠가버리므로, 아무것도 할 수 없는 상태가 된다.
        public static void Test()
        {
            lock (_lock)
            {
                UserManager.TestUser();
            }
        }
    }
    class UserManager
    {
        static object _lock = new object();
        public static void Test()
        {

            lock (_lock)
            {
                SessionManager.TestSession();
            }

        }
        public static void TestUser()
        {
            lock (_lock)
            {

            }
        }
    }
    class Lock_1129
    {

        static int number = 0;
        static object _obj = new object();
        static void Thread_1()
        {
            for(int i = 0; i < 10000; i++)
            {

                SessionManager.Test();
            }
        }
        static void Thread_2()
        {
            for (int i = 0; i < 10000; i++)
            {
                UserManager.Test();
            }
        }
        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            //동시에 시작하면 데드락이 걸린다. sleep을 이용하면 정상적으로 작동된다.
            t1.Start();
            t2.Start();
            Task.WaitAll(t1, t2);
            Console.WriteLine(number);
        }
    }
}