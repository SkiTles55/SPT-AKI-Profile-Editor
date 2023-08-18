using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class Build : AddableItem
    {
        [JsonProperty("id")]
        public override string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("root")]
        public string Root { get; set; }

        [JsonProperty("items")]
        public object[] Items { get; set; }

        public virtual string Type { get; set; }

        public void PrepareForImport(IEnumerable<Build> existBuilds)
        {
            int count = 1;
            string tempFileName = Name;
            while (existBuilds.Any(x => x.Name == tempFileName))
                tempFileName = string.Format("{0}({1})", Name, count++);
            Name = tempFileName;
            Id = ExtMethods.GenerateNewId(existBuilds.Select(x => x.Id));
        }
    }
}