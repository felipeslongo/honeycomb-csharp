using HoneyComb.Core.Threading;
using HoneyComb.TestChamber;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HoneyComb.Core.Tests.Threading
{
    public class SemaphoreSlim_ExtensionsTests
    {
        public class IsLockedTests : SemaphoreSlim_ExtensionsTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenASemaphoreSlim_WhenThreadLimitIsNotReached_ShouldReturnFalse()
            {
                var semaphore = new SemaphoreSlim(2);
                await semaphore.WaitAsync();

                Assert.False(semaphore.IsLocked());
                semaphore.Dispose();
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenASemaphoreSlim_WhenThreadLimitIsReached_ShouldReturnTrue()
            {
                var semaphore = new SemaphoreSlim(2);
                await semaphore.WaitAsync();
                await semaphore.WaitAsync();

                Assert.True(semaphore.IsLocked());
                semaphore.Dispose();
            }
        }

        public class IsUnlockedTests : SemaphoreSlim_ExtensionsTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenASemaphoreSlim_WhenThreadLimitIsNotReached_ShouldReturnTrue()
            {
                var semaphore = new SemaphoreSlim(2);
                await semaphore.WaitAsync();

                Assert.True(semaphore.IsUnlocked());
                semaphore.Dispose();
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenASemaphoreSlim_WhenThreadLimitIsReached_ShouldReturnFalse()
            {
                var semaphore = new SemaphoreSlim(2);
                await semaphore.WaitAsync();
                await semaphore.WaitAsync();

                Assert.False(semaphore.IsUnlocked());
                semaphore.Dispose();
            }
        }

        public class WaitDisposableAsyncTests : SemaphoreSlim_ExtensionsTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenASemaphoreSlim_WhenMethodInvoked_ShouldDecrementSemaphoreCountByOne()
            {
                var semaphore = new SemaphoreSlim(2);
                using (await semaphore.WaitDisposableAsync())
                {
                    Assert.Equal(1, semaphore.CurrentCount);
                }
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenTheDisposableReturnValue_WhenDisposed_ShouldReleaseSemaphoreCountByOne()
            {
                const int initialCurrentCount = 1;
                var semaphore = new SemaphoreSlim(initialCurrentCount, 2);
                var disposable = await semaphore.WaitDisposableAsync();

                disposable.Dispose();

                Assert.Equal(initialCurrentCount, semaphore.CurrentCount);
            }
        }
    }
}
