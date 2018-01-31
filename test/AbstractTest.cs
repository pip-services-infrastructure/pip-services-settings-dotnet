using System;

namespace test
{
    public abstract class AbstactTest : IDisposable
    {
        protected AbstactTest()
        {
            Initialize();
        }

        public void Dispose()
        {
            Uninitialize();
        }

        protected abstract void Initialize();

        protected abstract void Uninitialize();
    }
}
