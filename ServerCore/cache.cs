using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore
{
    class cache
    {
        static void Main(string[] args)
            //첫번째: 순차적으로 앞으로 간다. 1,2,3,4,5.... SPACIAL LOCALITY
            //바로 옆에 있는건 캐시에 미리 저장
            //두번째: 1, 6, 11, 16.... 순으로 띄엄띄엄 접근. 그다음 2,7,...
            //공간적 이점아님. 
        {   //[][][][][] [][][][][]... 5*5 배열 
            int[,] arr = new int[10000, 10000];
            {
                long now = DateTime.Now.Ticks;
                //시간이 얼마나 걸리나
                for(int y = 0; y < 10000; y++)
                {
                    for(int x = 0; x < 10000; x++)
                    {
                        arr[y, x] = 1;
                        Console.WriteLine($"(y,x) 순서 걸린 시간: {end - now}");
                    }
                }

                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(y,x) 순서 걸린 시간: {end-now}");
            }
            {
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                {
                    for (int x = 0; x < 10000; x++)
                    {
                        arr[x, y] = 1;
                    }
                }

                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(x,y) 순서 걸린 시간: {end - now}");
            }
        }
    }
}
