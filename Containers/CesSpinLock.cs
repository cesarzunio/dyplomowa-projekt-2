using System.Threading;

namespace Ces.Collections
{
    public struct CesSpinLock
    {
        const int UNLOCKED = 0;
        const int LOCKED = 1;

        int _lockValue;

        public readonly bool IsLocked => _lockValue == LOCKED;

        public static CesSpinLock Create() => new()
        {
            _lockValue = UNLOCKED,
        };

        public void Lock()
        {
            while (Interlocked.CompareExchange(ref _lockValue, LOCKED, UNLOCKED) == LOCKED) { }
        }

        public void Unlock()
        {
            Interlocked.Exchange(ref _lockValue, UNLOCKED);
        }
    }
}