using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Okra.Helpers
{
    internal static class ExceptionHelper
    {
        // *** Methods ***

        public static async void InjectAsyncExceptions(Task task)
        {
            // Await the task to allow any exceptions to be thrown

            try
            {
                await task;
            }
            catch (Exception e)
            {
                // If we are currently on the core dispatcher then we can simply rethrow the exception
                // NB: This will occur if awaiting the task returned immediately

                CoreDispatcher dispatcher = CoreApplication.GetCurrentView().CoreWindow.Dispatcher;

                if (dispatcher.HasThreadAccess)
                    throw;

                // Otherwise capture the exception with its original stack trace

                ExceptionDispatchInfo exceptionDispatchInfo = ExceptionDispatchInfo.Capture(e);

                // Re-throw the exception via a DispatcherTimer (this will then be captured by the Application.UnhandledException event)

                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    exceptionDispatchInfo.Throw();
                };
                timer.Start();
            }
        }
    }
}
