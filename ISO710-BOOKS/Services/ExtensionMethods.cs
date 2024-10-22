using System.Text.Json;

namespace ISO710_BOOKS.Services
{
    public static class JsonElementExtenstion
    {
        public static JsonElement? GetPropertyExtension(this JsonElement jsonElement, string propertyName)
        {
            if (jsonElement.TryGetProperty(propertyName, out JsonElement returnElement))
            {
                return returnElement;
            }

            return null;
        }
    }
}
