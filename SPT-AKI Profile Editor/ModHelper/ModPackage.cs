using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.ModHelper
{
    public class ModPackage
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("akiVersion")]
        public string AkiVersion { get; set; }

        [JsonProperty("scripts")]
        public Scripts Scripts { get; set; }

        [JsonProperty("devDependencies")]
        public DevDependencies DevDependencies { get; set; }
    }

    public class DevDependencies
    {
        [JsonProperty("@types/node")]
        public string TypesNode { get; set; }

        [JsonProperty("@typescript-eslint/eslint-plugin")]
        public string TypescriptEslintEslintPlugin { get; set; }

        [JsonProperty("@typescript-eslint/parser")]
        public string TypescriptEslintParser { get; set; }

        [JsonProperty("bestzip")]
        public string Bestzip { get; set; }

        [JsonProperty("eslint")]
        public string Eslint { get; set; }

        [JsonProperty("fs-extra")]
        public string FsExtra { get; set; }

        [JsonProperty("glob")]
        public string Glob { get; set; }

        [JsonProperty("tsyringe")]
        public string Tsyringe { get; set; }

        [JsonProperty("typescript")]
        public string Typescript { get; set; }
    }

    public class Scripts
    {
        [JsonProperty("setup")]
        public string Setup { get; set; }

        [JsonProperty("build")]
        public string Build { get; set; }
    }
}