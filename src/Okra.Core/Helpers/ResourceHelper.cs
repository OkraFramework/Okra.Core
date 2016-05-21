using Okra.Core;

namespace Okra.Helpers
{
    internal static class ResourceHelper
    {
        // *** Methods ***

        public static string GetErrorResource(string resourceName)
        {
            return Resources.ResourceManager.GetString(resourceName);
        }
    }
}
