using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Okra.Tests.Helpers
{
    public static class AssertEx
    {
        // *** Static Methods ***

        public static async Task<T> ThrowsExceptionAsync<T>(Func<Task> action) where T : Exception
        {
            Exception capturedException = null;

            try
            {
                await (action());
            }
            catch (Exception exception)
            {
                capturedException = exception;
            }

            return Assert.ThrowsException<T>(() => ThrowException(capturedException));
        }

        // *** Private Static Methods ***

        private static void ThrowException(Exception capturedException)
        {
            if (capturedException != null)
                throw capturedException;
        }
    }
}
