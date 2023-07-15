using Newtonsoft.Json;
using System.Linq;

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

        public string Id { get; set; }

        public string ParentId { get; set; }

        [JsonIgnore]
        public TarkovItem Item { get; }
    }
}