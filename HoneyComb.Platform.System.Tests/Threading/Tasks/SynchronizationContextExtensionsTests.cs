using System;
using System.Threading;
using System.Threading.Tasks;
using HoneyComb.Core.Threading;
using HoneyComb.TestChamber;
using Xunit;

namespace HoneyComb.Core.Tests.Threading.Tasks
{
    public class SynchronizationContextExtensionsTests
    {
        public class SendAsyncTests : SynchronizationContextExtensionsTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task ShouldAwaitTaskCompletion()
            {
                var context = new SynchronizationContext();
                var taskSource = new TaskCompletionSource<bool>();
                var task = context.SendAsync(async () => await taskSource.Task);
                Assert.False(task.IsCompletedSuccessfully);
                
                taskSource.SetResult(true);
                await task;
                
                Assert.True(task.IsCompletedSuccessfully);
            }
            
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task ShouldPropagateException()
            {
                var context = new SynchronizationContext();
                var taskSource = new TaskCompletionSource<bool>();
                var exception = new ApplicationException();
                var task = context.SendAsync(async () => await taskSource.Task);
                
                taskSource.SetException(exception);
                var result = await Assert.ThrowsAsync<ApplicationException>(() => task);
                
                Assert.StrictEqual(exception, result);
            }
        }
    }
}
