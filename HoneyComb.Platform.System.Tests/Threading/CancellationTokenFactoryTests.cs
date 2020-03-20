using System;
using System.Threading;
using HoneyComb.Core.Threading;
using HoneyComb.TestChamber;
using Xunit;

namespace HoneyComb.Core.Tests.Threading
{
    public class CancellationTokenFactoryTests
    {
        public class FromEventTests : CancellationTokenFactoryTests
        {
            public event EventHandler<int>? TestEvent;
            
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnEvent_WhenTestMethodInvoked_ShouldReturnAnCancellationTokenWithoutCancellationRequested()
            {
                var token = CancellationTokenFactory.FromEvent<int>(
                    subscribe => TestEvent += subscribe, 
                    unsubscribe => TestEvent -= unsubscribe
                    );

                Assert.False(token.IsCancellationRequested);
            }
            
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnSubscribedToken_WhenEventIsInvoked_ShouldRequestCancellation()
            {
                var token = CancellationTokenFactory.FromEvent<int>(
                    subscribe => TestEvent += subscribe, 
                    unsubscribe => TestEvent -= unsubscribe
                );
                
                TestEvent!.Invoke(this, int.MaxValue);

                Assert.True(token.IsCancellationRequested);
            }
            
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnSubscribedToken_WhenEventIsInvoked_ShouldUnsubscribe()
            {
                var token = CancellationTokenFactory.FromEvent<int>(
                    subscribe => TestEvent += subscribe, 
                    unsubscribe => TestEvent -= unsubscribe
                );
                
                TestEvent!.Invoke(this, int.MaxValue);

                Assert.Null(TestEvent);
            }
            
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnSubscribedTokenWithAnotherCancellationToken_WhenAnotherTokenIsCancelled_ShouldRequestCancellation()
            {
                var tokenSource = new CancellationTokenSource();
                var token = CancellationTokenFactory.FromEvent<int>(
                    subscribe => TestEvent += subscribe, 
                    unsubscribe => TestEvent -= unsubscribe,
                    tokenSource.Token
                );

                tokenSource.Cancel();

                Assert.True(token.IsCancellationRequested);
            }
            
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnSubscribedTokenWithAnotherCancellationToken_WhenAnotherTokenIsCancelled_ShouldUnsubscribe()
            {
                var tokenSource = new CancellationTokenSource();
                var token = CancellationTokenFactory.FromEvent<int>(
                    subscribe => TestEvent += subscribe, 
                    unsubscribe => TestEvent -= unsubscribe,
                    tokenSource.Token
                );

                tokenSource.Cancel();

                Assert.Null(TestEvent);
            }
        }
    }
}
