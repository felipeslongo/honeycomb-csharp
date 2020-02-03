using HoneyComb.Core.Threading.Tasks;
using HoneyComb.TestChamber;
using System;
using Xunit;

namespace HoneyComb.Core.Tests.Threading.Tasks
{
    public class ITaskScope_IsActiveTests
    {
        private readonly ITaskScopeMutable taskScope;

        public ITaskScope_IsActiveTests()
        {
            taskScope = TaskScopeFactory.Create();
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public void GivenACanceledScope_WhenInvoked_ShouldBeFalse()
        {
            taskScope.Cancel();

            Assert.False(taskScope.IsActive());
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public void GivenAFaultedScope_WhenInvoked_ShouldBeFalse()
        {
            taskScope.FinishWithException(new ApplicationException());

            Assert.False(taskScope.IsActive());
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public void GivenAFinishedScope_WhenInvoked_ShouldBeFalse()
        {
            taskScope.Finish();

            Assert.False(taskScope.IsActive());
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public void GivenAUnfinishedScope_WhenInvoked_ShouldBeTrue()
        {
            Assert.True(taskScope.IsActive());
        }
    }
}
