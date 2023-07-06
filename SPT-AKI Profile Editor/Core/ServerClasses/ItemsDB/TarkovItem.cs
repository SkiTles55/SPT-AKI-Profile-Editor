using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Linq;
using System.Windows.Media.Imaging;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class TarkovItem : AddableItem
    {
        [JsonConstructor]
        public TarkovItem(string id, TarkovItemProperties properties, string parent, string type)
        {
            Id = id;
            Properties = properties;
            Parent = parent;
            Type = type;
            SlotsCount = CalculateSlotsCount();
            CanBeAddedToStash = AppData.ServerDatabase.LocalesGlobal.ContainsKey(Id.Name())
                && !Properties.QuestItem
                && !AppData.AppSettings.BannedItems.Contains(Parent)
                && !AppData.AppSettings.BannedItems.Contains(Id);
        }

        [JsonProperty("_id")]
        public override string Id { get; set; }

        [JsonProperty("_props")]
        public TarkovItemProperties Properties { get; set; }

        [JsonProperty("_parent")]
        public override string Parent { get; set; }

        [JsonProperty("_type")]
        public string Type { get; set; }

        [JsonIgnore]
        public override string LocalizedName =>
            AppData.ServerDatabase.LocalesGlobal.ContainsKey(Id.Name()) ? AppData.ServerDatabase.LocalesGlobal[Id.Name()] : Id;

        [JsonIgnore]
        public int SlotsCount { get; }

        [JsonIgnore]
        public bool IsWeapon => Properties?.RecoilForceUp != 0;

        [JsonIgnore]
        public BitmapSource CategoryIcon => AppData.ServerDatabase?.HandbookHelper?.GetItemCategory(Id)?.BitmapIcon;

        public static TarkovItem CopyFrom(TarkovItem item)
        {
            TarkovItem tarkovItem = new(item.Id, item.Properties, item.Parent, item.Type);
            tarkovItem.AppendDogtagProperties();
            return tarkovItem;
        }

        public ExaminedItem GetExaminedItem() => new(Id, LocalizedName, CategoryIcon);

        private int CalculateSlotsCount()
        {
            int slots = 0;
            if (Properties?.Grids != null && !Properties.Grids.Any(x => x.Props == null))
            {
                foreach (var grid in Properties.Grids)
                    slots += grid.Props.CellsH * grid.Props.CellsV;
            }
            return slots;
        }

        private void AppendDogtagProperties()
        {
            if (Properties == null || !Properties.DogTagQualities)
                return;
            DogtagProperties = new(Id == "59f32bb586f774757e1e8442" ? "Bear" : "Usec",
                                   "Nickname",
                                   1,
                                   "5447a9cd4bdc2dbd208b4567");
        }
    }
}