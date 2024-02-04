using Newtonsoft.Json;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    internal class BotTypeAppearance
    {
        [JsonProperty("head")]
        public Dictionary<string, int> Heads { get; set; }

        [JsonProperty("voice")]
        public Dictionary<string, int> Voices { get; set; }
    }
}