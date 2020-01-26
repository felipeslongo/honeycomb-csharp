using HoneyComb.Core.Threading.Tasks;
using HoneyComb.TestChamber;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HoneyComb.Core.Tests.Threading.Tasks
{
    public class ITaskScope_RunTests
    {
        private readonly ITaskScopeMutable taskScope;

        public ITaskScope_RunTests()
        {
            taskScope = TaskScopeFactory.Create();
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenACanceledScope_WhenInvoked_ShouldThrowCancelledException()
        {
            taskScope.Cancel();

            await Assert.ThrowsAsync<TaskCanceledException>(() => taskScope.Run(_ => { }));
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAScope_WhenInvoked_ShouldPassAValidCancellationToken()
        {
            CancellationToken? token = null;

            await taskScope.Run(actualToken => token = actualToken);

            Assert.NotNull(token);
            Assert.False(token!.Value.IsCancellationRequested);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAScope_WhenInvokedWithAException_ShouldRethrowTheException()
        {
            var exception = new ApplicationException("Message");

            var result = await Assert.ThrowsAsync<ApplicationException>(() => taskScope.Run(_ => throw exception));

            Assert.Equal(exception, result);
        }
    }
}
