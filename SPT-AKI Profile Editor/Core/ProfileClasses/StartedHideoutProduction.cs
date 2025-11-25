using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class StartedHideoutProduction: BindableEntity
    {
        [JsonConstructor]
        public StartedHideoutProduction(string recipeId, float progress, float productionTime, double startTimestamp)
        {
            RecipeId = recipeId;
            Progress = progress;
            ProductionTime = productionTime;
            StartTimestamp = startTimestamp;

            var production = AppData.ServerDatabase?.HideoutProduction.FirstOrDefault(x => x.Id == recipeId);
            if (production != null)
            {
                ProductItem = AppData.ServerDatabase.ItemsDB.TryGetValue(production.EndProduct, out ServerClasses.TarkovItem value)
                    ? value.GetExaminedItem()
                    : new ExaminedItem(production.EndProduct, production.EndProduct, null);
            }
        }

        public string RecipeId { get; set; }

        public float Progress { get; set; }

        public float ProductionTime { get; set; }

        public double StartTimestamp { get; set; }

        public ExaminedItem ProductItem { get; }

        public bool IsFinished => Progress >= ProductionTime;

        public string Status
        {
            get
            {
                var key = IsFinished ? "tab_hideout_craft_status_finished" : "tab_hideout_craft_status_in_progress";
                return AppData.AppLocalization.GetLocalizedString(key);
            }
        }

        public void SetFinished()
        {
            if (IsFinished)
                return;
            var remainingSeconds = (ProductionTime - Progress) * 3600;
            StartTimestamp -= remainingSeconds;
            Progress = ProductionTime;
            OnPropertyChanged(nameof(Status));
        }
    }
}