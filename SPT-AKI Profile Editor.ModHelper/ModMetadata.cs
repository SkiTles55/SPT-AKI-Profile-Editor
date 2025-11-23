using SPTarkov.Server.Core.Models.Spt.Mod;

namespace SPT_AKI_Profile_Editor.ModHelper;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.skitles.profile.editor";
    public override string Name { get; init; } = "[SPT-AKI Profile Editor] Helper Mod";
    public override string Author { get; init; } = "SkiTles55";
    public override List<string>? Contributors { get; init; } = [];
    public override SemanticVersioning.Version Version { get; init; } = new("0.0.7");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
    public override List<string>? Incompatibilities { get; init; } = [];
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; } = [];
    public override string? Url { get; init; } = "https://github.com/SkiTles55/SPT-AKI-Profile-Editor";
    public override bool? IsBundleMod { get; init; } = false;
    public override string License { get; init; } = "MIT";
}