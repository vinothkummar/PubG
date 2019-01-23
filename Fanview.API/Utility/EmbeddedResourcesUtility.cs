using System.IO;
using System.Reflection;

namespace Fanview.API.Utility
{
    public static class EmbeddedResourcesUtility
    {
        public static string ReadEmbeddedResource(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException(resourcePath);
                }
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
