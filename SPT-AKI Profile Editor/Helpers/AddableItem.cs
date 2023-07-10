using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public abstract class AddableItem
    {
        public virtual string Id { get; set; }
        public virtual string Parent { get; set; }

        [JsonIgnore]
        public bool CanBeAddedToStash { get; set; }

        [JsonIgnore]
        public int AddingQuantity { get; set; } = 1;

        [JsonIgnore]
        public bool AddingFir { get; set; } = false;

        [JsonIgnore]
        public DogtagProperties DogtagProperties { get; set; }

        [JsonIgnore]
        public virtual string LocalizedName { get; set; }

        public bool CanBeAddedToContainer(TarkovItem container)
        {
            var filters = container.Properties?.Grids?.FirstOrDefault().Props?.Filters;
            if (filters == null || filters.Length == 0)
                return true;
            if (filters[0].ExcludedFilter.Contains(Parent))
                return false;
            if (filters[0].Filter.Length > 0)
            {
                List<string> parents = new() { Parent };
                while (AppData.ServerDatabase.ItemsDB.ContainsKey(parents.Last()))
                    parents.Add(AppData.ServerDatabase.ItemsDB[parents.Last()].Parent);
                return parents.Any(x => filters[0].Filter.Contains(x));
            }
            return true;
        }
    }
}