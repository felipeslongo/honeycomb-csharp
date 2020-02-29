
using HoneyComb.Core.Threading.Tasks;
using HoneyComb.TestChamber;
using System;
using System.Threading.Tasks;
using Xunit;

namespace HoneyComb.Core.Tests.Threading.Tasks
{
    public class ITaskScope_EnsureActiveTests
    {
        private readonly ITaskScopeMutable taskScope;

        public ITaskScope_EnsureActiveTests()
        {
            taskScope = TaskScopeFactory.Create();
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public void GivenACanceledScope_WhenInvoked_ShouldThrowTaskCanceledExceptionWithoutInnerException()
        {
            taskScope.Cancel();

            var result = Assert.Throws<TaskCanceledException>(taskScope.EnsureActive);

            Assert.Null(result.InnerException);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public void GivenAFaultedScope_WhenInvoked_ShouldThrowTaskCanceledExceptionWithTheFaultExceptionAsTheInnerException()
        {
            var faultException = new ApplicationException("Faulted Exception");
            taskScope.FinishWithException(faultException);

            var result = Assert.Throws<TaskCanceledException>(taskScope.EnsureActive);

            Assert.NotNull(result.InnerException);
            Assert.Equal(faultException, result.InnerException);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public void GivenAFinishedScope_WhenInvoked_ShouldThrowTaskCanceledExceptionWithoutInnerException()
        {
            taskScope.Finish();

            var result = Assert.Throws<TaskCanceledException>(taskScope.EnsureActive);

            Assert.Null(result.InnerException);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public void GivenAUnfinishedScope_WhenInvoked_ShouldNotThrowAnyException()
        {
            taskScope.EnsureActive();
        }
    }
}
