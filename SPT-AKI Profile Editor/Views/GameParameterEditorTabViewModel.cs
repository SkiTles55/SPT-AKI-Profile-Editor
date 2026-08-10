using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Globalization;

namespace SPT_AKI_Profile_Editor.Views
{
    public class GameParameterEditorTabViewModel : BindableViewModel
    {
        private string bossChanceText = "0.0";

        public string BossChanceText
        {
            get => bossChanceText;
            set
            {
                bossChanceText = value;
                OnPropertyChanged(nameof(BossChanceText));
            }
        }

        public RelayCommand ApplyBossChanceCommand => new(_ => ApplyBossChance());

        private void ApplyBossChance()
        {
            if (!float.TryParse(BossChanceText, NumberStyles.Float, CultureInfo.InvariantCulture, out float bossChance))
                return;

            string serverRoot = AppData.AppSettings.ServerPath;
            GameParameterEditorService.UpdateBossChance(serverRoot, bossChance);
        }
    }
}
