using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class StartedHideoutProduction
    {
        [JsonConstructor]
        public StartedHideoutProduction(string recipeId, float progress, bool inProgress, double startTimestamp)
        {
            RecipeId = recipeId;
            Progress = progress;
            InProgress = inProgress;
            StartTimestamp = startTimestamp;

            var production = AppData.ServerDatabase?.HideoutProduction.FirstOrDefault(x => x.Id == recipeId);
            if (production != null)
            {
                ProductItem = AppData.ServerDatabase.ItemsDB.ContainsKey(production.EndProduct)
            ? AppData.ServerDatabase.ItemsDB[production.EndProduct].GetExaminedItem()
            : new ExaminedItem(production.EndProduct, production.EndProduct, null);
            }
        }

        public string RecipeId { get; set; }

        public float Progress { get; set; }

        [JsonProperty("inProgress")]
        public bool InProgress { get; set; }

        public double StartTimestamp { get; set; }

        public ExaminedItem ProductItem { get; }
    }
}