using Windows.ApplicationModel.Resources;

namespace Okra.Helpers
{
    internal static class ResourceHelper
    {
        // *** Constants ***

        private const string ERROR_RESOURCEMAP = "Okra.Core/Errors";

        // *** Static Fields ***

        private static ResourceLoader errorResourceLoader;

        // *** Methods ***

        public static string GetErrorResource(string resourceName)
        {
            if (errorResourceLoader == null)
                errorResourceLoader = ResourceLoader.GetForViewIndependentUse(ERROR_RESOURCEMAP);

            return errorResourceLoader.GetString(resourceName);
        }
    }
}
