using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SPT_AKI_Profile_Editor.Views
{
    class WeaponBuildsViewModel : INotifyPropertyChanged
    {
        public static AppLocalization AppLocalization => AppData.AppLocalization;
        public static Profile Profile => AppData.Profile;
        public RelayCommand RemoveBuild => new(async obj => {
            if (obj == null)
                return;
            if (await Dialogs.YesNoDialog(this,
                "remove_preset_dialog_title",
                "remove_preset_dialog_caption") == MessageDialogResult.Affirmative)
                Profile.RemoveBuild(obj.ToString());
        });
        public static RelayCommand ExportBuild => new(obj => {
            if (obj == null)
                return;
            if (obj is not WeaponBuild build)
                return;
            SaveFileDialog saveFileDialog = new ();
            saveFileDialog.Filter = "Файл JSON (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog.FileName = $"Weapon preset {build.Name}";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                App.worker.AddAction(new WorkerTask
                {
                    Action = () => { Profile.ExportBuild(build.Name, saveFileDialog.FileName); },
                    Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                    Description = AppLocalization.GetLocalizedString("tab_presets_export")
                });
            }
        });
        public static RelayCommand ImportBuilds => new(obj => {
            OpenFileDialog openFileDialog = new ();
            openFileDialog.Filter = "Файл JSON (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    App.worker.AddAction(new WorkerTask
                    {
                        Action = () => { Profile.ImportBuild(file); },
                        Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                        Description = AppLocalization.GetLocalizedString("tab_presets_import")
                    });
                }
            }
        });

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}