using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SPT_AKI_Profile_Editor
{
    public class WeaponBuildWindowViewModel : BindableViewModel
    {
        private readonly InventoryItem _item;
        private readonly StashEditMode _editMode;

        public WeaponBuildWindowViewModel(InventoryItem item, StashEditMode editMode, IDialogCoordinator dialogCoordinator)
        {
            Worker = new Worker(dialogCoordinator, this);
            WindowTitle = item.LocalizedName;
            _item = item;
            _editMode = editMode;
            List<InventoryItem> items = new() { _item };
            List<string> skippedSlots = new() { "patron_in_weapon", "cartridges" };
            switch (editMode)
            {
                case StashEditMode.PMC:
                    items.AddRange(AppData.Profile.Characters.Pmc.Inventory.GetInnerItems(item.Id, skippedSlots));
                    break;
                case StashEditMode.Scav:
                    items.AddRange(AppData.Profile.Characters.Scav.Inventory.GetInnerItems(item.Id, skippedSlots));
                    break;
            }
            WeaponBuild = new WeaponBuild(_item, items.Select(x => InventoryItem.CopyFrom(x)).ToList());
        }

        public Worker Worker { get; }

        public string WindowTitle { get; }

        public WeaponBuild WeaponBuild { get; }

        public RelayCommand RemoveItem => new(async obj =>
        {
            if (await Dialogs.YesNoDialog(this, "remove_stash_item_title", "remove_stash_item_caption"))
            {
                switch (_editMode)
                {
                    case StashEditMode.Scav:
                        Profile.Characters.Scav.Inventory.RemoveItems(new() { _item.Id });
                        break;
                    case StashEditMode.PMC:
                        Profile.Characters.Pmc.Inventory.RemoveItems(new() { _item.Id });
                        break;
                }
            }
        });

        public RelayCommand ExportBuild => new(obj =>
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "Файл JSON (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog.FileName = $"Weapon preset {WeaponBuild.Name}";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Worker.AddAction(new WorkerTask
                {
                    Action = () => { Profile.ExportBuild(WeaponBuild, saveFileDialog.FileName); },
                    Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                    Description = AppLocalization.GetLocalizedString("tab_presets_export")
                });
            }
        });

        public RelayCommand AddToWeaponBuilds => new(obj =>
        {
            Worker.AddAction(new WorkerTask
            {
                Action = () => { Profile.ImportBuild(WeaponBuild); },
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("tab_presets_export")
            });
        });
    }
}