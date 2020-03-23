using System;
using System.Threading.Tasks;
using HoneyComb.Core.Lifecycles;
using HoneyComb.Core.Lifecycles.Threading.Tasks;

namespace HoneyComb.UI
{
    //Rename to RenewableView ?
    public sealed class RecreatableView<TView> : ILifecycleOwner where TView : class, ILifecycleOwner
    {
        private readonly LifecycleOfLifecycles lifecycle;
        private TView? view;

        public RecreatableView()
        {
            lifecycle = new LifecycleOfLifecycles(this);
        }

        public Lifecycle Lifecycle => lifecycle;

        public TView? View
        {
            get => view;
            set
            {
                view = value;
                NotifyViewSet();
            }
        }

        public async Task<T> WhenActiveAsync<T>(Func<TaskCompletionSource<T>, Task> asyncTask)
        {
            await lifecycle.WhenActiveAsync();
            return await InvokeAsyncTaskHandlingLifecycleRenewEventsForResult(asyncTask);
        }
        
        private async Task<T> InvokeAsyncTaskHandlingLifecycleRenewEventsForResult<T>(Func<TaskCompletionSource<T>, Task> asyncTask)
        {
            var taskSource = new TaskCompletionSource<T>();
            
            var onLifecycleRenewed =
                CreateEventHandlerForReinvokeAsyncTaskOnLifecycleRenewed(taskSource, asyncTask);
            
            var onLifecycleDisposed = CreateEventHandlerForCancelTaskSourceOnLifecycleDisposed(taskSource);
            
            try
            {
                lifecycle.Renewed += onLifecycleRenewed;
                lifecycle.OnDisposed += onLifecycleDisposed;

                await InvokeAsyncTaskAndHandleExceptionIntoTaskSource(taskSource, asyncTask);
                
                return await taskSource.Task;
            }
            finally
            {
                lifecycle.Renewed -= onLifecycleRenewed;
                lifecycle.OnDisposed -= onLifecycleDisposed;
            }
        }

        private EventHandler<EventArgs> CreateEventHandlerForReinvokeAsyncTaskOnLifecycleRenewed<T>(
            TaskCompletionSource<T> taskSource, 
            Func<TaskCompletionSource<T>, Task> asyncTask)
        {
            return async (_, __) =>
            {
                if (taskSource.Task.IsCompleted)
                    return;

                await lifecycle.WhenActiveAsync();
                await InvokeAsyncTaskAndHandleExceptionIntoTaskSource(taskSource, asyncTask);
            };
        }
        
        private EventHandler<EventArgs> CreateEventHandlerForCancelTaskSourceOnLifecycleDisposed<T>(
            TaskCompletionSource<T> taskSource) => (_, __) => taskSource.TrySetCanceled();

        private async Task InvokeAsyncTaskAndHandleExceptionIntoTaskSource<T>(
            TaskCompletionSource<T> taskSource, 
            Func<TaskCompletionSource<T>, Task> asyncTask)
        {
            try
            {
                await asyncTask(taskSource);
            }
            catch (OperationCanceledException)
            {
                taskSource.TrySetCanceled();
            }
            catch (Exception e)
            {
                taskSource.TrySetException(e);
            }
        }

        private void NotifyViewSet()
        {
            lifecycle.SetLifecycle(view!.Lifecycle);
        }
    }
}
