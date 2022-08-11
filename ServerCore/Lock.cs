using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ServerCore
{
    //재귀적 lock 허용?(yes)->writeLock->writelock ok writelock->readlock ok readlock->writelock xx
    //스핀락 정책(5000번->yield)

    class Lock
    {
        const int EMPTY_FLAG = 0x00000000;
        const int WRITE_MASK = 0x7FFF0000;
        const int READ_MASK = 0x0000FFFF;
        const int MAX_SPIN_COUNT = 5000;
        //32bit.. [unused(1)] [writeThreadId(15bit)] [ReadCount(16bit)]
        int _flag=EMPTY_FLAG;
        int _writeCount = 0;

        public void WriteLock()
        {

            //동일 쓰레드가 wrtielock을 이미 획득하고 잇는지 확인
            int lockThreadId = (_flag & WRITE_MASK) >> 16;
            if (Thread.CurrentThread.ManagedThreadId == lockThreadId)
            {
                _writeCount++;
                return;

            }

            //아무도 writeLock or ReadLock을 획득하고 있지 않을때, 경합해 소유권을 얻는다
            while (true)
            {
                int desired=(Thread.CurrentThread.ManagedThreadId << 16) & WRITE_MASK;
                for(int i = 0; i < MAX_SPIN_COUNT; i++)
                {
                    //시도할때성공시 return
                    if(Interlocked.CompareExchange(ref _flag, desired, EMPTY_FLAG) == EMPTY_FLAG)
                    {
                        _writeCount = 1;

                        //성공
                        return;
                    }
                    //이건 atomic 하지 않음.
                    //if (_flag == EMPTY_FLAG)
                    //    _flag = desired;
                }

                Thread.Yield();

            }
        }
        public void WriteUnlock()
        {
            int lockCount = --_writeCount;
            if (lockCount == 0)
                Interlocked.Exchange(ref _flag, EMPTY_FLAG);
        }
        public void ReadLock()
        {//동일 쓰레드가 wrtielock을 이미 획득하고 잇는지 확인
            int lockThreadId = (_flag & WRITE_MASK) >> 16;
            if (Thread.CurrentThread.ManagedThreadId == lockThreadId)
            {
                Interlocked.Increment(ref _flag);
                return;

            }
            //아무도 write lock을 획득하고 잇지 않으면, readcount를 1늘린다.
            while (true)
            {
                for(int i = 0; i < MAX_SPIN_COUNT; i++)
                {
                    int expected = (_flag & READ_MASK);
                    if(Interlocked.CompareExchange(ref _flag, expected + 1, expected) == expected)
                    {

                        return;
                    }

                //    if ((_flag & WRITE_MASK) == 0){
                //        _flag = _flag + 1;
                //}
                }
                Thread.Yield();
            }
        }
        public void ReadUnLock()
        {
            Interlocked.Decrement(ref _flag);

        }
    }
}
