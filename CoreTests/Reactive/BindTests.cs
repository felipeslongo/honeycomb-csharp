/* Obsoleted
using Core.Reactive;
using Core.Threading;
using CoreTests.Assertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoreTests.Reactive
{
    public class BindTests
    {
        protected const int DifferentValue = 2;
        protected const int SameValue = 1;

        public int Property { get; set; }

        public class WithSynchronizationContextTests : BindTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenASynchronizationContext_ShouldBeExecutedInThatContext_WhenPropertyIsChanged()
            {
                await SynchronizationContextAssert.ShouldBeExecutedInCurrentContextAsynchronously(contextVisitor =>
                {
                    var liveData = new MutableLiveData<int>(SameValue);
                    liveData.BindMethod(contextVisitor.CaptureContext)
                        .WithSynchronizationContext(contextVisitor.CurrentContext);

                    liveData.Value = DifferentValue;                    
                });
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAEmptySynchronizationContext_ShouldBeExecutedInTheCurrentThread_WhenPropertyIsChanged()
            {
                SynchronizationContextAssert.ShouldBeExecutedInCurrentThreadSynchronously(contextVisitor =>
                {
                    var liveData = new MutableLiveData<int>(SameValue);
                    liveData.BindMethod(contextVisitor.CaptureContext);

                    liveData.Value = DifferentValue;
                });
            }
        }

        public class WithCurrentSynchronizationContextTests : LiveDataTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenACurrentSynchronizationContext_ShouldBeExecutedInThatContext_WhenPropertyIsChanged()
            {
                await SynchronizationContextAssert.ShouldBeExecutedInCurrentContextAsynchronously(contextVisitor =>
                {
                    var liveData = new MutableLiveData<int>(SameValue);
                    liveData.BindMethod(contextVisitor.CaptureContext)
                        .WithCurrentSynchronizationContext();

                    liveData.Value = DifferentValue;
                });
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAEmptyCurrentSynchronizationContext_ShouldBeExecutedInTheCurrentThread_WhenPropertyIsChanged()
            {
                SynchronizationContextAssert.ShouldBeExecutedInCurrentThreadSynchronously(contextVisitor =>
                {
                    var liveData = new MutableLiveData<int>(SameValue);
                    liveData.BindMethod(contextVisitor.CaptureContext);

                    liveData.Value = DifferentValue;
                });
            }
        }
    }
}
*/