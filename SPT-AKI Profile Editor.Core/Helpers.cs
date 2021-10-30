using System.Globalization;
using System.IO;
using System.Text.Json;

namespace SPT_AKI_Profile_Editor.Core
{
    static class Helpers
    {
        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static string WindowsCulture => CultureInfo.CurrentCulture.Parent.ToString();

        public static void SaveJson(string path, object data) => File.WriteAllText(path, JsonSerializer.Serialize(data, _serializerOptions));
    }
}