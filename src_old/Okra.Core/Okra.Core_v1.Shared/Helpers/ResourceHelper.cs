using Windows.ApplicationModel.Resources;

namespace Okra.Helpers
{
    internal static class ResourceHelper
    {
        // *** Constants ***

        private const string ERROR_RESOURCEMAP = "Okra.Core/Errors";

        // *** Static Fields ***

        private static ResourceLoader s_errorResourceLoader;

        // *** Methods ***

        public static string GetErrorResource(string resourceName)
        {
            if (s_errorResourceLoader == null)
                s_errorResourceLoader = ResourceLoader.GetForViewIndependentUse(ERROR_RESOURCEMAP);

            return s_errorResourceLoader.GetString(resourceName);
        }
    }
}
