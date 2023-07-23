using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public abstract class AddableItem : BindableEntity
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
        public virtual string LocalizedName { get; }

        [JsonIgnore]
        public virtual string LocalizedDescription { get; }

        [JsonIgnore]
        public StashType StashType { get; set; } = StashType.Stash;

        [JsonIgnore]
        public virtual bool IsQuestItem => false;

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

        public bool ContainsText(string text, bool includeDesriptions)
        {
            return LocalizedName.ToUpper().Contains(text.ToUpper())
                || FilterWithDescription(text, includeDesriptions, LocalizedDescription);
        }

        private static bool FilterWithDescription(string text, bool includeDesriptions, string itemDescription)
            => includeDesriptions
            && !string.IsNullOrEmpty(itemDescription)
            && itemDescription.ToUpper().Contains(text.ToUpper());
    }
}