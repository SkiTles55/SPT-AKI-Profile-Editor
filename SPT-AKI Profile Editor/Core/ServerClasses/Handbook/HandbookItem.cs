using System.Linq;
using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class HandbookItem
    {
        [JsonConstructor]
        public HandbookItem(string id, string parentId)
        {
            Id = id;
            ParentId = parentId;
            Item = AppData.ServerDatabase?.ItemsDB?
                .Where(x => x.Key == Id).FirstOrDefault()
                .Value;
        }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("ParentId")]
        public string ParentId { get; set; }

        [JsonIgnore]
        public TarkovItem Item { get; }
    }
}