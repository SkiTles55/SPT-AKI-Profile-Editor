using System.Linq;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class HandbookItem
    {
        private TarkovItem item;

        [JsonConstructor]
        public HandbookItem(string id, string parentId)
        {
            Id = id;
            ParentId = parentId;
            Item = AppData.ServerDatabase?.ItemsDB?
                .Where(x => x.Key == Id).FirstOrDefault()
                .Value;
        }

        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("ParentId")]
        public string ParentId { get; set; }

        [JsonIgnore]
        public TarkovItem Item { get => item; set => item = value; }
    }
}