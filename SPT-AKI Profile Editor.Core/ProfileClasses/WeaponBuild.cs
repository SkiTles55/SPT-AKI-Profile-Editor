using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class WeaponBuild
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("root")]
        public string Root { get; set; }

        [JsonProperty("items")]
        public object[] Items { get; set; }

        [JsonIgnore]
        public string Weapon =>
            BuildItems
            .Where(x => x.Id == Root)
            .FirstOrDefault()
            .LocalizedName;

        [JsonIgnore]
        public float Ergonomics => ergonomics;

        [JsonIgnore]
        public int RecoilForceUp => recoilForceUp;

        [JsonIgnore]
        public int RecoilForceBack => recoilForceBack;

        [JsonIgnore]
        private List<InventoryItem> BuildItems
        {
            get
            {
                List<InventoryItem> items = new();
                float RecoilDelta = 0;
                foreach (var obj in Items)
                {
                    try
                    {
                        var item = JsonConvert.DeserializeObject<InventoryItem>(obj.ToString());
                        items.Add(item);
                        if (item.Id == Root)
                        {
                            if (AppData.ServerDatabase.ItemsDB.ContainsKey(item.Tpl) && AppData.ServerDatabase.ItemsDB[item.Tpl].Properties != null)
                            {
                                recoilForceUp = AppData.ServerDatabase.ItemsDB[item.Tpl].Properties.RecoilForceUp;
                                recoilForceBack = AppData.ServerDatabase.ItemsDB[item.Tpl].Properties.RecoilForceBack;
                                ergonomics = AppData.ServerDatabase.ItemsDB[item.Tpl].Properties.Ergonomics;
                            }
                        }
                        else
                        {
                            if (AppData.ServerDatabase.ItemsDB.ContainsKey(item.Tpl) && AppData.ServerDatabase.ItemsDB[item.Tpl].Properties != null)
                            {
                                RecoilDelta += AppData.ServerDatabase.ItemsDB[item.Tpl].Properties.Recoil;
                                ergonomics += AppData.ServerDatabase.ItemsDB[item.Tpl].Properties.Ergonomics;
                            }
                        }
                    }
                    catch (Exception ex) { Logger.Log($"WeaponBuilds weapon item loading error: {ex.Message}"); }
                }
                RecoilDelta /= 100f;
                recoilForceUp = (int)Math.Round(recoilForceUp + recoilForceUp * RecoilDelta);
                recoilForceBack = (int)Math.Round(recoilForceBack + recoilForceBack * RecoilDelta);
                return items;
            }
        }

        [JsonIgnore]
        private float ergonomics = 0;

        [JsonIgnore]
        private int recoilForceUp = 0;

        [JsonIgnore]
        private int recoilForceBack = 0;
    }
}