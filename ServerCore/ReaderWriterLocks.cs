using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ServerCore
{
    class ReaderWriterLocks
    {
        //상호배제
        static object _lock = new object();
        static SpinLock _lock2 = new SpinLock();
        
        class Reward
        {

        }
        static ReaderWriterLockSlim _lock3 = new ReaderWriterLockSlim();
        static Reward GetRewardById(int id)
        {
            _lock3.EnterReadLock();
            _lock3.ExitReadLock();
            lock (_lock)
            {
                //진짜진짜진짜가끔 일어남->그걸 위해 lock 잡는건 조금 비효율적
                //특수한 경우 일어난 경우에만 막는걸 ReaderWriterLock이라함.
            }
            return null;
        }
        static void AddReward(Reward reward)
        {
            _lock3.EnterWriteLock();
            _lock3.ExitWriteLock();

        }
        static void Main(string[] args)
        {
            //1. spinlock
            //2. yield
            //3. AutoResetEvent

            bool lockTaken = false;
            _lock2.Enter(ref lockTaken);

            _lock2.Exit();
        
        }
    }
}
