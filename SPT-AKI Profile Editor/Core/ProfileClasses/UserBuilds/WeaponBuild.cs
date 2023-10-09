using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class WeaponBuild : Build
    {
        public static readonly string WeaponBuildType = "weapon";
        private float RecoilDelta = 0;

        [JsonConstructor]
        public WeaponBuild(string id,
                           string name,
                           string root,
                           object[] items,
                           string type) : base(id, name, root, items)
        {
            Type = type;
            BuildItems = items.Select(x => JsonConvert.DeserializeObject<InventoryItem>(x.ToString()));
            CalculateBuildProperties();
            CanBeAddedToStash = true;
        }

        public WeaponBuild(ItemPreset itemPreset) : base(itemPreset.Id,
                                                         null,
                                                         itemPreset.Root,
                                                         itemPreset.Items)
        {
            Type = WeaponBuildType;
            BuildItems = itemPreset.Items.Select(x => JsonConvert.DeserializeObject<InventoryItem>(x.ToString()));
            CalculateBuildProperties(true);
            CanBeAddedToStash = true;
        }

        public WeaponBuild(InventoryItem item, List<InventoryItem> items) : base(item.Id,
                                                                                 item.LocalizedName,
                                                                                 item.Id,
                                                                                 null)
        {
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
            BuildItems = items;
            Type = WeaponBuildType;
            CalculateBuildProperties();
            CanBeAddedToStash = true;
        }

        public override string LocalizedName => Name;

        [JsonProperty("type")]
        public override string Type { get; set; }

        [JsonIgnore]
        public InventoryItem Weapon { get; set; }

        [JsonIgnore]
        public string RootTpl { get; set; }

        [JsonIgnore]
        public float Ergonomics { get; set; }

        [JsonIgnore]
        public float RecoilForceUp { get; set; }

        [JsonIgnore]
        public float RecoilForceBack { get; set; }

        [JsonIgnore]
        public bool HasModdedItems { get; set; }

        public static WeaponBuild CopyFrom(WeaponBuild item) => new(item.Id, item.Name, item.Root, item.Items, item.Type);

        private void CalculateBuildProperties(bool fromTemplate = false)
        {
            if (!BuildItems.Any())
                return;
            foreach (var item in BuildItems)
            {
                if (item.Id == Root)
                    SetupWeaponProperties(item);
                else
                    AddModProperties(item);
            }
            RecoilDelta /= 100f;
            RecoilForceUp = (int)Math.Round(RecoilForceUp + RecoilForceUp * RecoilDelta);
            RecoilForceBack = (int)Math.Round(RecoilForceBack + RecoilForceBack * RecoilDelta);
            HasModdedItems = BuildItems.Any(x => !x.IsInItemsDB);
            var weapon = BuildItems.Where(x => x.Id == Root).FirstOrDefault();
            BuildItems = BuildItems.Where(x => x.Id != Root);
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