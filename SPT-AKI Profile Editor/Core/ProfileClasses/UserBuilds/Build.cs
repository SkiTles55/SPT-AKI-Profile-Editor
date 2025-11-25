using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class Build(string id,
                 string name,
                 string root,
                 object[] items) : AddableItem
    {
        public override string Id { get; set; } = id;

        public string Name { get; set; } = name;

        public string Root { get; set; } = root;

        public object[] Items { get; set; } = items;

        [JsonIgnore]
        public IEnumerable<InventoryItem> BuildItems { get; set; }

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