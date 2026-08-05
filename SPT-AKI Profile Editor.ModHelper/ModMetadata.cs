using SPTarkov.Server.Core.Models.Spt.Mod;

namespace SPT_AKI_Profile_Editor.ModHelper;

public record ModMetadata : IModMetadata
{
    public string ModGuid { get; init; } = "com.skitles.profile.editor";
    public string Name { get; init; } = "[SPT-AKI Profile Editor] Helper Mod";
    public string Author { get; init; } = "SkiTles55";
    public List<string>? Contributors { get; init; } = [];
    public SemanticVersioning.Version Version { get; init; } = new("0.0.8");
    public SemanticVersioning.Range SptVersion { get; init; } = new("~4.1.0");
    public bool HasPrepatcher { get; init; } = false;
    public List<string>? Incompatibilities { get; init; } = [];
    public Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; } = [];
    public string? Url { get; init; } = "https://github.com/SkiTles55/SPT-AKI-Profile-Editor";
    public string License { get; init; } = "MIT";
}