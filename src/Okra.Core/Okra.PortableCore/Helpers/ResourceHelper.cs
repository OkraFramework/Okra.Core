using Okra.Strings;

namespace Okra.Helpers
{
    internal static class ResourceHelper
    {
        // *** Methods ***

        public static string GetErrorResource(string resourceName)
        {
            return Errors.ResourceManager.GetString(resourceName);
        }
    }
}
