using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class WeaponBuild : AddableItem
    {
        private float RecoilDelta = 0;

        public static readonly string WeaponBuildType = "weapon";

        [JsonConstructor]
        public WeaponBuild(string id, string name, string root, object[] items, string type)
        {
            Id = id;
            Name = name;
            Root = root;
            Items = items;
            Type = type;
            var buildItems = items.Select(x => JsonConvert.DeserializeObject<InventoryItem>(x.ToString()));
            if (buildItems.Any())
                CalculateBuildProperties(buildItems);
            CanBeAddedToStash = true;
        }

        public WeaponBuild(ItemPreset itemPreset)
        {
            Id = itemPreset.Id;
            Root = itemPreset.Root;
            Items = itemPreset.Items;
            Type = WeaponBuildType;
            var buildItems = itemPreset.Items.Select(x => JsonConvert.DeserializeObject<InventoryItem>(x.ToString()));
            if (buildItems.Any())
                CalculateBuildProperties(buildItems, true);
            CanBeAddedToStash = true;
        }

        public WeaponBuild(InventoryItem item, List<InventoryItem> items)
        {
            Id = item.Id;
            Name = item.LocalizedName;
            Root = item.Id;
            foreach (var innerItem in items)
            {
                innerItem.Upd = null;
                innerItem.Location = null;
                if (innerItem.Id == item.Id)
                {
                    innerItem.ParentId = null;
                    innerItem.SlotId = null;
                }
            }
            Items = items.ToArray();
            Type = WeaponBuildType;
            CalculateBuildProperties(items);
            CanBeAddedToStash = true;
        }

        public override string LocalizedName => Name;

        [JsonProperty("id")]
        public override string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("root")]
        public string Root { get; set; }

        [JsonProperty("items")]
        public object[] Items { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonIgnore]
        public IEnumerable<InventoryItem> BuildItems { get; set; }

        [JsonIgnore]
        public InventoryItem Weapon { get; set; }

        [JsonIgnore]
        public string RootTpl { get; set; }

        [JsonIgnore]
        public float Ergonomics { get; set; }

        [JsonIgnore]
        public int RecoilForceUp { get; set; }

        [JsonIgnore]
        public int RecoilForceBack { get; set; }

        [JsonIgnore]
        public bool HasModdedItems { get; set; }

        public static WeaponBuild CopyFrom(WeaponBuild item) => new(item.Id, item.Name, item.Root, item.Items, item.Type);

        private void CalculateBuildProperties(IEnumerable<InventoryItem> buildItems, bool fromTemplate = false)
        {
            foreach (var item in buildItems)
            {
                if (item.Id == Root)
                    SetupWeaponProperties(item);
                else
                    AddModProperties(item);
            }
            RecoilDelta /= 100f;
            RecoilForceUp = (int)Math.Round(RecoilForceUp + RecoilForceUp * RecoilDelta);
            RecoilForceBack = (int)Math.Round(RecoilForceBack + RecoilForceBack * RecoilDelta);
            BuildItems = buildItems.Where(x => x.Id != Root);
            HasModdedItems = buildItems.Any(x => !x.IsInItemsDB);
            var weapon = buildItems.Where(x => x.Id == Root).FirstOrDefault();
            Weapon = weapon;
            RootTpl = weapon?.Tpl;
            Parent = weapon?.IsInItemsDB ?? false ? AppData.ServerDatabase.ItemsDB[weapon?.Tpl].Parent : null;
            if (!fromTemplate)
                return;
            Name = weapon?.LocalizedName;
            if (AppData.ServerDatabase?.LocalesGlobal?.ContainsKey(Id) ?? false)
                Name += " " + AppData.ServerDatabase.LocalesGlobal[Id];
        }

        private void AddModProperties(InventoryItem item)
        {
            if (item.IsInItemsDB && AppData.ServerDatabase.ItemsDB[item.Tpl].Properties != null)
            {
                RecoilDelta += AppData.ServerDatabase.ItemsDB[item.Tpl].Properties.Recoil;
                Ergonomics += AppData.ServerDatabase.ItemsDB[item.Tpl].Properties.Ergonomics;
            }
        }

        private void SetupWeaponProperties(InventoryItem item)
        {
            if (item.IsInItemsDB && AppData.ServerDatabase.ItemsDB[item.Tpl].Properties != null)
            {
                RecoilForceUp = AppData.ServerDatabase.ItemsDB[item.Tpl].Properties.RecoilForceUp;
                RecoilForceBack = AppData.ServerDatabase.ItemsDB[item.Tpl].Properties.RecoilForceBack;
                Ergonomics = AppData.ServerDatabase.ItemsDB[item.Tpl].Properties.Ergonomics;
            }
        }
    }
}