using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Utils;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

using IOPath = System.IO.Path;

namespace SPT_AKI_Profile_Editor.ModHelper;

[Injectable(TypePriority = OnLoadOrder.PostSptModLoader + 1)]
public class ProfileEditorModHelper(
    DatabaseServer databaseServer,
    ConfigServer configServer,
    ISptLogger<ProfileEditorModHelper> logger,
    FileUtil fileUtil,
    JsonUtil jsonUtil) : IOnLoad
{
    private readonly string hashesFileName = "Hashes.json";
    private Dictionary<String, String> hashes = [];
    private bool hasDataUpdates = false;
    private string exportPath = "";

    public Task OnLoad()
    {
        var pathToMod = IOPath.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
        exportPath = IOPath.Combine(pathToMod, "exportedDB");
        var hashesPath = IOPath.Combine(pathToMod, hashesFileName);
        if (fileUtil.FileExists(hashesPath))
            hashes = jsonUtil.DeserializeFromFile<Dictionary<String, String>>(hashesPath) ?? [];
        LogMessage("Started database exporting");
        var tables = databaseServer.GetTables();
        ExportDatabaseEntry("Handbook", tables.Templates.Handbook);
        ExportDatabaseEntry("Production", tables.Hideout.Production);
        ExportDatabaseEntry("Items", tables.Templates.Items);
        ExportDatabaseEntry("Quests", tables.Templates.Quests);
        var questConfig = configServer.GetConfig<QuestConfig>();
        if (questConfig != null)
            ExportDatabaseEntry("QuestConfig", questConfig);
        // Traders still exporting on every run, due to nextRessuply changes
        ExportDictionaryEntry("Traders", tables.Traders.ToDictionary(x => x.Key.ToString(), y => (object)y.Value.Base));
        ExportDictionaryEntry("Locales", tables.Locales.Global.ToDictionary(x => x.Key.ToString(), y => (object)y));
        ExportDatabaseEntry("ItemPresets", tables.Globals.ItemPresets);
        ExportDatabaseEntry("Mastering", tables.Globals.Configuration.Mastering);
        ExportDatabaseEntry("ExpTable", tables.Globals.Configuration.Exp);

        if (hasDataUpdates)
        {
            fileUtil.WriteFile(hashesPath, jsonUtil.Serialize(hashes) ?? "");
            LogMessage("DB successfully exported");
        }
        else
        {
            LogMessage("DB is up to date!");
        }
        return Task.CompletedTask;
    }

    private static string GenerateMd5Hash(string input)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = MD5.HashData(inputBytes);
        return Convert.ToHexStringLower(hashBytes);
    }

    private void ExportDatabaseEntry(string name, object entry)
    {
        string entryJson = jsonUtil.Serialize(entry) ?? "";
        string entryHash = GenerateMd5Hash(entryJson);

        if (hashes.TryGetValue(name, out string? existingHash) && existingHash == entryHash)
            return;

        string filePath = IOPath.Combine(exportPath, $"{name}.json");
        fileUtil.WriteFile(filePath, entryJson);
        LogMessage($"{name} exported");
        hashes[name] = entryHash;
        hasDataUpdates = true;
    }

    private void ExportDictionaryEntry(string name, IDictionary<string, object> dictionary)
    {
        bool hasInnerUpdates = false;

        foreach (var (key, value) in dictionary)
        {
            string entryName = $"{name}/{key}";
            string valueJson = jsonUtil.Serialize(value) ?? "";
            string valueHash = GenerateMd5Hash(valueJson);

            if (hashes.TryGetValue(entryName, out string? existingHash) && existingHash == valueHash)
                continue;

            string filePath = IOPath.Combine(exportPath, $"{entryName}.json");
            fileUtil.WriteFile(filePath, valueJson);
            hashes[entryName] = valueHash;
            hasInnerUpdates = true;
        }

        if (hasInnerUpdates)
        {
            LogMessage($"{name} exported");
            hasDataUpdates = true;
        }
    }

    private void LogMessage(string message) => logger.LogWithColor($"[[SPT-AKI Profile Editor] Helper Mod] : {message}", LogTextColor.Green);
}