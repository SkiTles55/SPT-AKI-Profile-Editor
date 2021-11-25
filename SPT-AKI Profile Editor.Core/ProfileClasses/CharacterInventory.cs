using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterInventory : INotifyPropertyChanged
    {
        [JsonProperty("items")]
        public InventoryItem[] Items
        {
            get => items;
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        [JsonProperty("stash")]
        public string Stash
        {
            get => stash;
            set
            {
                stash = value;
                OnPropertyChanged("Stash");
            }
        }

        [JsonIgnore]
        public string Pockets
        {
            get => Items?
                .Where(x => x.SlotId == AppData.AppSettings.PocketsSlotId)
                .FirstOrDefault()?.Tpl;
            set
            {
                var pocketsSlot = Items?
                .Where(x => x.SlotId == AppData.AppSettings.PocketsSlotId)
                .FirstOrDefault();
                if (pocketsSlot != null)
                {
                    pocketsSlot.Tpl = value;
                    OnPropertyChanged("CharacterPockets");
                }
            }
        }
        [JsonIgnore]
        public string DollarsCount => GetMoneyCountString(AppData.AppSettings.MoneysDollarsTpl);
        [JsonIgnore]
        public string RublesCount => GetMoneyCountString(AppData.AppSettings.MoneysRublesTpl);
        [JsonIgnore]
        public string EurosCount => GetMoneyCountString(AppData.AppSettings.MoneysEurosTpl);
        [JsonIgnore]
        public InventoryItem[] InventoryItems => Items?
            .Where(x => x.ParentId == Stash
            && AppData.ServerDatabase.ItemsDB.ContainsKey(x.Tpl))?
            .ToArray();

        private InventoryItem[] items;
        private string stash;

        public void RemoveItems(string[] itemIds)
        {
            List<InventoryItem> ItemsList = Items.ToList();
            foreach (var id in itemIds)
            {
                var item = ItemsList.Where(x => x.Id == id).FirstOrDefault();
                if (item != null)
                    ItemsList.Remove(item);
            }
            Items = ItemsList.ToArray();
        }

        private string GetMoneyCountString(string moneys) => (Items?
            .Where(x => x.Tpl == moneys)
            .Sum(x => x.Upd.StackObjectsCount ?? 0) ?? 0)
            .ToString("N0");

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}