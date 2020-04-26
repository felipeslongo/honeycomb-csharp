using HoneyComb.Core.Threading;
using HoneyComb.TestChamber;
using System.Threading;
using Xunit;

namespace HoneyComb.Core.Tests.Threading
{
    public class SemaphoreSlimFactoryTests
    {
        public class CreateMutexLockTests : SemaphoreSlimFactoryTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenNoParameter_WhenInvoked_ShouldReturnASemaphoreWithCurrentCountOfOne()
            {
                using var semaphore = SemaphoreSlimFactory.CreateMutexLock();

                Assert.Equal(1, semaphore.CurrentCount);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenNoParameter_WhenInvoked_ShouldReturnASemaphoreWithMaxCountOfOne()
            {
               using var semaphore = SemaphoreSlimFactory.CreateMutexLock();

                Assert.Throws<SemaphoreFullException>(() => semaphore.Release());
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenTrueParameter_WhenInvoked_ShouldReturnASemaphoreWithCurrentCountOfZero()
            {
                using var semaphore = SemaphoreSlimFactory.CreateMutexLock(true);

                Assert.Equal(0, semaphore.CurrentCount);
            }
        }
    }
}
