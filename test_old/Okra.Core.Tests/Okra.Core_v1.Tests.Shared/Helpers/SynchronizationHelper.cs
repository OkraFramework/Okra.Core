using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// NB: Based upon code from the Microsoft PFX team
//     See link http://blogs.msdn.com/b/pfxteam/archive/2012/02/02/10263555.aspx

namespace Okra.Tests.Helpers
{
    public static class SynchronizationHelper
    {
        /// <summary>Runs the specified asynchronous method.</summary>
        /// <param name="asyncMethod">The asynchronous method to execute.</param>
        public static void Run(Action asyncMethod)
        {
            if (asyncMethod == null) throw new ArgumentNullException("asyncMethod");

            var prevCtx = SynchronizationContext.Current;
            try
            {
                // Establish the new context
                var syncCtx = new SingleThreadSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(syncCtx);

                // Invoke the function
                syncCtx.OperationStarted();
                asyncMethod();
                syncCtx.OperationCompleted();

                // Pump continuations and propagate any exceptions
                syncCtx.RunOnCurrentThread();
            }
            finally { SynchronizationContext.SetSynchronizationContext(prevCtx); }
        }

        /// <summary>Runs the specified asynchronous method.</summary>
        /// <param name="asyncMethod">The asynchronous method to execute.</param>
        public static void Run(Func<Task> asyncMethod)
        {
            if (asyncMethod == null) throw new ArgumentNullException("asyncMethod");

            var prevCtx = SynchronizationContext.Current;
            try
            {
                // Establish the new context
                var syncCtx = new SingleThreadSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(syncCtx);

                // Invoke the function and alert the context to when it completes
                var t = asyncMethod();
                if (t == null) throw new InvalidOperationException("No task provided.");
                t.ContinueWith(delegate { syncCtx.Complete(); }, TaskScheduler.Default);

                // Pump continuations and propagate any exceptions
                syncCtx.RunOnCurrentThread();
                t.GetAwaiter().GetResult();
            }
            finally { SynchronizationContext.SetSynchronizationContext(prevCtx); }
        }

        /// <summary>Runs the specified asynchronous method.</summary>
        /// <param name="asyncMethod">The asynchronous method to execute.</param>
        public static T Run<T>(Func<Task<T>> asyncMethod)
        {
            if (asyncMethod == null) throw new ArgumentNullException("asyncMethod");

            var prevCtx = SynchronizationContext.Current;
            try
            {
                // Establish the new context
                var syncCtx = new SingleThreadSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(syncCtx);

                // Invoke the function and alert the context to when it completes
                var t = asyncMethod();
                if (t == null) throw new InvalidOperationException("No task provided.");
                t.ContinueWith(delegate { syncCtx.Complete(); }, TaskScheduler.Default);

                // Pump continuations and propagate any exceptions
                syncCtx.RunOnCurrentThread();
                return t.GetAwaiter().GetResult();
            }
            finally { SynchronizationContext.SetSynchronizationContext(prevCtx); }
        }

        // *** Private Sub-classes ***

        /// <summary>Provides a SynchronizationContext that's single-threaded.</summary>
        private sealed class SingleThreadSynchronizationContext : SynchronizationContext
        {
            /// <summary>The queue of work items.</summary>
            private readonly BlockingCollection<KeyValuePair<SendOrPostCallback, object>> _queue =
                new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();
            /// <summary>The processing thread.</summary>
            //private readonly Thread m_thread = Thread.CurrentThread;
            /// <summary>The number of outstanding operations.</summary>
            private int _operationCount = 0;

            /// <summary>Dispatches an asynchronous message to the synchronization context.</summary>
            /// <param name="d">The System.Threading.SendOrPostCallback delegate to call.</param>
            /// <param name="state">The object passed to the delegate.</param>
            public override void Post(SendOrPostCallback d, object state)
            {
                if (d == null) throw new ArgumentNullException("d");
                _queue.Add(new KeyValuePair<SendOrPostCallback, object>(d, state));
            }

            /// <summary>Not supported.</summary>
            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("Synchronously sending is not supported.");
            }

            /// <summary>Runs an loop to process all queued work items.</summary>
            public void RunOnCurrentThread()
            {
                foreach (var workItem in _queue.GetConsumingEnumerable())
                    workItem.Key(workItem.Value);
            }

            /// <summary>Notifies the context that no more work will arrive.</summary>
            public void Complete() { _queue.CompleteAdding(); }

            /// <summary>Invoked when an async operation is started.</summary>
            public override void OperationStarted()
            {
                Interlocked.Increment(ref _operationCount);
            }

            /// <summary>Invoked when an async operation is completed.</summary>
            public override void OperationCompleted()
            {
                if (Interlocked.Decrement(ref _operationCount) == 0)
                    Complete();
            }
        }
    }
}
