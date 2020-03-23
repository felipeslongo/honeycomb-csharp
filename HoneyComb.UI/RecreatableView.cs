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

        public async Task<T> WhenActiveAsync<T>(Func<TaskCompletionSource<T>, Task> interactAsync)
        {
            await lifecycle.WhenActiveAsync();
            var interactionTaskSource = new TaskCompletionSource<T>();
            
            EventHandler<EventArgs> onLifecycleRenewed = async (_, __) =>
            {
                if (interactionTaskSource.Task.IsCompleted)
                    return;

                await lifecycle.WhenActiveAsync();
                await InvokeAsyncTaskAndHandleExceptionIntoTaskSource(interactionTaskSource, interactAsync);
            };
            EventHandler<EventArgs> onLifecycleDisposed = (_, __) => interactionTaskSource.TrySetCanceled();

            try
            {
                lifecycle.Renewed += onLifecycleRenewed;
                lifecycle.OnDisposed += onLifecycleDisposed;

                await InvokeAsyncTaskAndHandleExceptionIntoTaskSource(interactionTaskSource, interactAsync);
                
                return await interactionTaskSource.Task;
            }
            finally
            {
                lifecycle.Renewed -= onLifecycleRenewed;
                lifecycle.OnDisposed -= onLifecycleDisposed;
            }
        }
        
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
