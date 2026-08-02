using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Spt.Tables;
using SPTarkov.Server.Core.Utils;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

using Color = Spectre.Console.Color;
using IOPath = System.IO.Path;

namespace SPT_AKI_Profile_Editor.ModHelper;

[Injectable(TypePriority = OnLoadOrder.PostLoad + 1)]
public class ProfileEditorModHelper(
    TemplateTable templateTable,
    HideoutTable hideoutTable,
    TradersTable tradersTable,
    LocaleTable localeTable,
    GlobalTable globalTable,
    QuestConfig questConfig,
    ISptLogger<ProfileEditorModHelper> logger,
    FileUtil fileUtil,
    JsonUtil jsonUtil) : IOnLoad
{
    private readonly string hashesFileName = "Hashes.json";
    private Dictionary<String, String> hashes = [];
    private bool hasDataUpdates = false;
    private string exportPath = "";

    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        var pathToMod = IOPath.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (pathToMod == null)
        {
            LogMessage("Unable to get mod location path. Export Cancelled", Color.Red);
            return Task.CompletedTask;
        }

        exportPath = IOPath.Combine(pathToMod, "exportedDB");
        var hashesPath = IOPath.Combine(pathToMod, hashesFileName);
        if (fileUtil.FileExists(hashesPath))
            hashes = jsonUtil.DeserializeFromFile<Dictionary<String, String>>(hashesPath) ?? [];
        LogMessage("Started database exporting");
        ExportDatabaseEntry("Handbook", templateTable.Handbook);
        ExportDatabaseEntry("Production", hideoutTable.Production);
        ExportDatabaseEntry("Items", templateTable.Items);
        ExportDatabaseEntry("Quests", templateTable.Quests);
        ExportDatabaseEntry("QuestConfig", questConfig);
        // Traders still exporting on every run, due to nextRessuply changes
        ExportDictionaryEntry("Traders", tradersTable.ToDictionary(x => x.Key.ToString(), y => (object)y.Value.Base));
        ExportDictionaryEntry("Locales", localeTable.Global.ToDictionary(x => x.Key.ToString(), y => (object)(y.Value.Value ?? [])));
        ExportDatabaseEntry("ItemPresets", globalTable.ItemPresets);
        ExportDatabaseEntry("Mastering", globalTable.Configuration.Mastering);
        ExportDatabaseEntry("ExpTable", globalTable.Configuration.Exp);

        if (hasDataUpdates)
        {
            var hasesData = jsonUtil.Serialize(hashes);
            if (hasesData != null)
            {
                fileUtil.WriteFile(hashesPath, hasesData);
                LogMessage("DB successfully exported");
            }
            else
            {
                LogMessage("DB successfully exported, but hashes not updated", Color.Red);
            }
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
        string? entryJson = jsonUtil.Serialize(entry);
        if (entryJson == null)
            return;
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
            string? valueJson = jsonUtil.Serialize(value);
            if (valueJson == null)
                continue;
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

    private void LogMessage(string message, Color? textColor = null)
        => logger.LogWithColor($"[[SPT-AKI Profile Editor] Helper Mod] : {message}", textColor ?? Color.Green);
}