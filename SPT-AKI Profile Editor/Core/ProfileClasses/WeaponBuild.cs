using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class WeaponBuild
    {
        private float RecoilDelta = 0;

        [JsonConstructor]
        public WeaponBuild(string id, string name, string root, object[] items)
        {
            Id = id;
            Name = name;
            Root = root;
            Items = items;
            CalculateBuildProperties();
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("root")]
        public string Root { get; set; }

        [JsonProperty("items")]
        public object[] Items { get; set; }

        [JsonIgnore]
        public IEnumerable<InventoryItem> BuildItems { get; set; }

        [JsonIgnore]
        public string Weapon { get; set; }

        [JsonIgnore]
        public float Ergonomics { get; set; }

        [JsonIgnore]
        public int RecoilForceUp { get; set; }

        [JsonIgnore]
        public int RecoilForceBack { get; set; }

        [JsonIgnore]
        public bool HasModdedItems { get; set; }

        private void CalculateBuildProperties()
        {
            List<InventoryItem> buildItems = new();
            foreach (var obj in Items)
            {
                try
                {
                    var item = JsonConvert.DeserializeObject<InventoryItem>(obj.ToString());
                    buildItems.Add(item);
                    if (item.Id == Root)
                        SetupWeaponProperties(item);
                    else
                        AddModProperties(item);
                }
                catch (Exception ex) { Logger.Log($"WeaponBuilds weapon item loading error: {ex.Message}"); }
            }
            RecoilDelta /= 100f;
            RecoilForceUp = (int)Math.Round(RecoilForceUp + RecoilForceUp * RecoilDelta);
            RecoilForceBack = (int)Math.Round(RecoilForceBack + RecoilForceBack * RecoilDelta);
            if (buildItems.Count == 0)
                return;
            BuildItems = buildItems.Where(x => x.Id != Root);
            HasModdedItems = buildItems.Any(x => !AppData.ServerDatabase.ItemsDB.ContainsKey(x.Tpl));
            Weapon = buildItems.Where(x => x.Id == Root).FirstOrDefault().LocalizedName;
        }

        private void AddModProperties(InventoryItem item)
        {
            if (AppData.ServerDatabase.ItemsDB.ContainsKey(item.Tpl) && AppData.ServerDatabase.ItemsDB[item.Tpl].Properties != null)
            {
                RecoilDelta += AppData.ServerDatabase.ItemsDB[item.Tpl].Properties.Recoil;
                Ergonomics += AppData.ServerDatabase.ItemsDB[item.Tpl].Properties.Ergonomics;
            }
        }

        private void SetupWeaponProperties(InventoryItem item)
        {
            if (AppData.ServerDatabase.ItemsDB.ContainsKey(item.Tpl) && AppData.ServerDatabase.ItemsDB[item.Tpl].Properties != null)
            {
                RecoilForceUp = AppData.ServerDatabase.ItemsDB[item.Tpl].Properties.RecoilForceUp;
                RecoilForceBack = AppData.ServerDatabase.ItemsDB[item.Tpl].Properties.RecoilForceBack;
                Ergonomics = AppData.ServerDatabase.ItemsDB[item.Tpl].Properties.Ergonomics;
            }
        }
    }
}