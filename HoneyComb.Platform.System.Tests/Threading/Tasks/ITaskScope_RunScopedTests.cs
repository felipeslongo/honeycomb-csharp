using HoneyComb.Core.Threading.Tasks;
using HoneyComb.TestChamber;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HoneyComb.Core.Tests.Threading.Tasks
{
    public class ITaskScope_RunScopedTests
    {
        private readonly ITaskScopeMutable taskScope;

        public ITaskScope_RunScopedTests()
        {
            taskScope = TaskScopeFactory.Create();
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenACanceledScope_WhenInvoked_ShouldThrowCancelledException()
        {
            taskScope.Cancel();

            await Assert.ThrowsAsync<TaskCanceledException>(() => taskScope.RunScoped(_ => { }));
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenACanceledScope_WhenInvoked_ShouldRemainCancelledItsScopeTask()
        {
            taskScope.Cancel();

            await Assert.ThrowsAsync<TaskCanceledException>(() => taskScope.RunScoped(_ => { }));

            Assert.True(taskScope.ScopeTask.IsCanceled);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenACanceledScope_WhenInvoked_ShouldRemainCancelledItsToken()
        {
            taskScope.Cancel();

            await Assert.ThrowsAsync<TaskCanceledException>(() => taskScope.RunScoped(_ => { }));

            Assert.True(taskScope.CancellationToken.IsCancellationRequested);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAFaltedScope_WhenInvoked_ShouldThrowTaskCanceledException()
        {
            var faultException = new ApplicationException("Fault Exception");
            taskScope.FinishWithException(faultException);

            await Assert.ThrowsAsync<TaskCanceledException>(() => taskScope.RunScoped(_ => { }));
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAFaltedScope_WhenInvoked_ShouldRemainFaltedItsScopeTaskWithTheOriginalExceptionThatCausedTheFault()
        {
            var faultException = new ApplicationException("Fault Exception");
            taskScope.FinishWithException(faultException);

            await Assert.ThrowsAsync<TaskCanceledException>(() => taskScope.RunScoped(_ => { }));

            Assert.True(taskScope.ScopeTask.IsFaulted);
            Assert.Equal(faultException, taskScope.ScopeTask.Exception?.InnerExceptions.Single());
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAFinishedScope_WhenInvoked_ShouldRemainCancelledItsToken()
        {            
            taskScope.Finish();

            await Assert.ThrowsAsync<TaskCanceledException>(() => taskScope.RunScoped(_ => { }));

            Assert.True(taskScope.CancellationToken.IsCancellationRequested);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAFinishedScope_WhenInvoked_ShouldThrowTaskCanceledException()
        {
            taskScope.Finish();

            await Assert.ThrowsAsync<TaskCanceledException>(() => taskScope.RunScoped(_ => { }));
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAFinishedScope_WhenInvoked_ShouldRemainCompletedSuccessfullyItsScopeTask()
        {
            taskScope.Finish();

            await Assert.ThrowsAsync<TaskCanceledException>(() => taskScope.RunScoped(_ => { }));

            Assert.True(taskScope.ScopeTask.IsCompletedSuccessfully);            
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAFaltedScope_WhenInvoked_ShouldRemainCancelledItsToken()
        {
            var faultException = new ApplicationException("Fault Exception");
            taskScope.FinishWithException(faultException);

            await Assert.ThrowsAsync<TaskCanceledException>(() => taskScope.RunScoped(_ => { }));

            Assert.True(taskScope.CancellationToken.IsCancellationRequested);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAScope_WhenInvoked_ShouldPassAValidCancellationToken()
        {
            CancellationToken? token = null;

            await taskScope.RunScoped(actualToken => token = actualToken);

            Assert.NotNull(token);
            Assert.False(token!.Value.IsCancellationRequested);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAScope_WhenInvokedWithAException_ShouldRethrowTheException()
        {
            var exception = new ApplicationException("Message");

            var result = await Assert.ThrowsAsync<ApplicationException>(() => taskScope.RunScoped(_ => throw exception));

            Assert.Equal(exception, result);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAScope_WhenInvokedWithAException_ShouldCancelItsToken()
        {
            var exception = new ApplicationException("Message");

            _ = await Assert.ThrowsAsync<ApplicationException>(() => taskScope.RunScoped(_ => throw exception));

            Assert.True(taskScope.CancellationToken.IsCancellationRequested);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAScope_WhenInvokedWithAException_ShouldSetToFaltedItsScopeTask()
        {
            var exception = new ApplicationException("Message");

            _ = await Assert.ThrowsAsync<ApplicationException>(() => taskScope.RunScoped(_ => throw exception));

            Assert.True(taskScope.ScopeTask.IsFaulted);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAScope_WhenInvokedWithCancellationException_ShouldRethrowTheException()
        {
            var exception = new TaskCanceledException("Message");

            var result = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => 
            taskScope.RunScoped(_ => throw exception));

            Assert.Equal(exception, result);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAScope_WhenInvokedWithCancellationException_ShouldCancelItsToken()
        {
            var exception = new TaskCanceledException("Message");

            _ = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => taskScope.RunScoped(_ => throw exception));

            Assert.True(taskScope.CancellationToken.IsCancellationRequested);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAScope_WhenInvokedWithCancellationException_ShouldSetToCancelledItsScopeTask()
        {
            var exception = new TaskCanceledException("Message");

            _ = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => taskScope.RunScoped(_ => throw exception));

            Assert.True(taskScope.ScopeTask.IsCanceled);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAScope_WhenDoubleInvokedWithAException_ShouldSetTheFirstReturningExceptionAsTheFaultException()
        {
            var firstFaultException = new ApplicationException("First Fault Exception");
            var awaiter = new TaskCompletionSource<EventArgs>();
            var secondFaultException = new ApplicationException("Second Fault Exception");

            var secondFaultTask = Assert.ThrowsAsync<ApplicationException>(() => taskScope.RunScoped(async _ =>
            {
                await awaiter.Task;
                throw secondFaultException;
            }));
            var firstFaultTask = Assert.ThrowsAsync<ApplicationException>(() => taskScope.RunScoped(_ =>
            {
                throw firstFaultException;
            }));
            await firstFaultTask;
            awaiter.SetResult(EventArgs.Empty);
            await Task.WhenAll(firstFaultTask, secondFaultTask);

            Assert.Equal(firstFaultException, taskScope.ScopeTask?.Exception?.InnerExceptions.Single());
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAScope_WhenDoubleInvokedWithAExceptionThenACancellation_ShouldSetToFaltedItsScopeTask()
        {
            var faultException = new ApplicationException("Fault Exception");
            var awaiter = new TaskCompletionSource<EventArgs>();
            var cancellationException = new TaskCanceledException("Cancel Exception");

            var cancellationTask = Assert.ThrowsAsync<TaskCanceledException>(() => taskScope.RunScoped(async _ =>
            {
                await awaiter.Task;
                throw cancellationException;
            }));
            var faultTask = Assert.ThrowsAsync<ApplicationException>(() => taskScope.RunScoped(_ =>
            {
                throw faultException;
            }));
            awaiter.SetResult(EventArgs.Empty);
            await Task.WhenAll(faultTask, cancellationTask);

            Assert.True(taskScope.ScopeTask.IsFaulted);
            Assert.Equal(faultException, taskScope.ScopeTask?.Exception?.InnerExceptions.Single());
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAScope_WhenDoubleInvokedWithACancellationThenAException_ShouldSetToCancelledItsScopeTask()
        {
            var faultException = new ApplicationException("Fault Exception");
            var awaiter = new TaskCompletionSource<EventArgs>();
            var cancellationException = new TaskCanceledException("Cancel Exception");

            var faultTask = Assert.ThrowsAsync<ApplicationException>(() => taskScope.RunScoped(async _ =>
            {
                await awaiter.Task;
                throw faultException;
            }));
            var cancellationTask = Assert.ThrowsAsync<TaskCanceledException>(() => taskScope.RunScoped(_ =>
            {                
                throw cancellationException;
            }));            
            awaiter.SetResult(EventArgs.Empty);
            await Task.WhenAll(faultTask, cancellationTask);

            Assert.True(taskScope.ScopeTask.IsCanceled);
            Assert.Null(taskScope.ScopeTask?.Exception);
        }
    }
}
