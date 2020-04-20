using HoneyComb.Core.Threading;
using HoneyComb.TestChamber;
using System.Threading.Tasks;
using Xunit;

namespace HoneyComb.Core.Tests.Threading
{
    public class MutexLockAsyncTests
    {
        public class WaitAsyncTests : MutexLockAsyncTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAMutexLockAsync_WhenInvoked_ShouldBeLocked()
            {
                using var mutexLock = new MutexLockAsync();

                using (await mutexLock.WaitAsync())
                {
                    Assert.True(mutexLock.IsLocked);
                }
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAMutexLockAsyncInLockedState_WhenLockIsDisposed_ShouldBeUnlocked()
            {
                using var mutexLock = new MutexLockAsync();
                var disposable = await mutexLock.WaitAsync();

                disposable.Dispose();

                Assert.False(mutexLock.IsLocked);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAMutexLockAsyncInLockedState_WhenTryingToEnter_ShouldBeBeLockedInAwaitState()
            {
                using var mutexLock = new MutexLockAsync();
                using var disposable = await mutexLock.WaitAsync();

                var lockAsyncTask = mutexLock.WaitAsync();

                Assert.False(lockAsyncTask.IsCompleted);
            }
        }
    }
}
