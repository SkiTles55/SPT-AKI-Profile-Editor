using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.IO;
using System.Windows.Forms;

namespace SPT_AKI_Profile_Editor.Views
{
    internal class WeaponBuildsViewModel : BindableViewModel
    {
        public static RelayCommand ExportBuild => new(obj =>
          {
              if (obj == null)
                  return;
              if (obj is not WeaponBuild build)
                  return;
              SaveFileDialog saveFileDialog = new();
              saveFileDialog.Filter = "Файл JSON (*.json)|*.json|All files (*.*)|*.*";
              saveFileDialog.FileName = $"Weapon preset {build.Name}";
              saveFileDialog.RestoreDirectory = true;
              if (saveFileDialog.ShowDialog() == DialogResult.OK)
              {
                  App.Worker.AddAction(new WorkerTask
                  {
                      Action = () => { Profile.ExportBuild(build.Name, saveFileDialog.FileName); },
                      Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                      Description = AppLocalization.GetLocalizedString("tab_presets_export")
                  });
              }
          });

        public static RelayCommand ExportBuilds => new(obj =>
          {
              FolderBrowserDialog folderBrowserDialog = new();
              folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
              folderBrowserDialog.ShowNewFolderButton = true;
              if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
              {
                  foreach (var build in Profile.WeaponBuilds)
                  {
                      App.Worker.AddAction(new WorkerTask
                      {
                          Action = () => { Profile.ExportBuild(build.Key, Path.Combine(folderBrowserDialog.SelectedPath, $"Weapon preset {build.Value.Name}.json")); },
                          Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                          Description = AppLocalization.GetLocalizedString("tab_presets_export")
                      });
                  }
              }
          });

        public static RelayCommand ImportBuilds => new(obj =>
          {
              OpenFileDialog openFileDialog = new();
              openFileDialog.Filter = "Файл JSON (*.json)|*.json|All files (*.*)|*.*";
              openFileDialog.RestoreDirectory = true;
              openFileDialog.Multiselect = true;

              if (openFileDialog.ShowDialog() == DialogResult.OK)
              {
                  foreach (var file in openFileDialog.FileNames)
                  {
                      App.Worker.AddAction(new WorkerTask
                      {
                          Action = () => { Profile.ImportBuild(file); },
                          Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                          Description = AppLocalization.GetLocalizedString("tab_presets_import")
                      });
                  }
              }
          });

        public RelayCommand RemoveBuild => new(async obj =>
                                 {
                                     if (obj == null)
                                         return;
                                     if (await Dialogs.YesNoDialog(this, "remove_preset_dialog_title", "remove_preset_dialog_caption"))
                                         Profile.RemoveBuild(obj.ToString());
                                 });

        public RelayCommand RemoveBuilds => new(async obj =>
         {
             if (await Dialogs.YesNoDialog(this, "remove_preset_dialog_title", "remove_presets_dialog_caption"))
                 Profile.RemoveBuilds();
         });
    }
}