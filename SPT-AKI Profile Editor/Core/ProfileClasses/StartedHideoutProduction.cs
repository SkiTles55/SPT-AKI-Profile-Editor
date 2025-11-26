using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class StartedHideoutProduction : BindableEntity
    {
        [JsonConstructor]
        public StartedHideoutProduction(string recipeId, double progress, double productionTime, long startTimestamp)
        {
            RecipeId = recipeId;
            Progress = progress;
            ProductionTime = productionTime;
            StartTimestamp = startTimestamp;

            var production = AppData.ServerDatabase?.HideoutProduction.FirstOrDefault(x => x.Id == recipeId);
            if (production != null)
            {
                var hideoutProduction = new CharacterHideoutProduction(production, true);

                ProductItem = hideoutProduction.ProductItem;
                AreaLocalizedName = hideoutProduction.AreaLocalizedName;
            }
            else
            {
                AreaLocalizedName = "unknown";
            }
        }

        public string RecipeId { get; set; }

        public double Progress { get; set; }

        public double ProductionTime { get; set; }

        public long StartTimestamp { get; set; }

        public ExaminedItem ProductItem { get; }

        public string AreaLocalizedName { get; }

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
            StartTimestamp -= (long)remainingSeconds;
            Progress = ProductionTime;
            OnPropertyChanged(nameof(Status));
        }
    }
}