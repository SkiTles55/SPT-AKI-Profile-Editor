using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPT_AKI_Profile_Editor.Core;
using System;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public static class GameParameterEditorService
    {
        public static int UpdateBossChance(string serverRootPath, float bossChance)
        {
            if (string.IsNullOrWhiteSpace(serverRootPath) || !Directory.Exists(serverRootPath))
                return 0;

            string locationsRoot = Path.Combine(serverRootPath, "SPT", "SPT_Data", "database", "locations");
            if (!Directory.Exists(locationsRoot))
                return 0;

            var baseJsonFiles = Directory.GetFiles(locationsRoot, "base.json", SearchOption.AllDirectories)
                .Where(x => x.EndsWith("base.json", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            int updatedFiles = 0;
            foreach (var path in baseJsonFiles)
            {
                try
                {
                    JObject json = JObject.Parse(File.ReadAllText(path));
                    JToken token = json["BossChance"];
                    if (token == null)
                        continue;

                    json["BossChance"] = JToken.FromObject(bossChance);
                    File.WriteAllText(path, json.ToString(Formatting.Indented));
                    updatedFiles++;
                }
                catch (Exception ex)
                {
                    Logger.Log($"Failed to update boss chance in {path}: {ex.Message}");
                }
            }

            return updatedFiles;
        }
    }
}
