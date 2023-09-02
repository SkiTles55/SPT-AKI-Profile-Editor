using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor
{
    public class WeaponBuildWindowViewModel : BindableViewModel
    {
        private readonly InventoryItem _item;
        private readonly StashEditMode _editMode;
        private readonly IDialogManager _dialogManager;
        private readonly IWindowsDialogs _windowsDialogs;

        public WeaponBuildWindowViewModel(InventoryItem item,
                                          StashEditMode editMode,
                                          IDialogCoordinator dialogCoordinator,
                                          IWindowsDialogs windowsDialogs,
                                          IDialogManager dialogManager = null,
                                          IWorker worker = null) : this(item,
                                                                        GetInventory(editMode),
                                                                        dialogCoordinator,
                                                                        windowsDialogs,
                                                                        dialogManager,
                                                                        worker)
        {
            _editMode = editMode;
        }

        public WeaponBuildWindowViewModel(InventoryItem item,
                                          EquipmentBuild build,
                                          IDialogCoordinator dialogCoordinator,
                                          IWindowsDialogs windowsDialogs,
                                          IDialogManager dialogManager = null,
                                          IWorker worker = null) : this(item,
                                                                        MakeInventory(build),
                                                                        dialogCoordinator,
                                                                        windowsDialogs,
                                                                        dialogManager,
                                                                        worker)
        {
        }

        private WeaponBuildWindowViewModel(InventoryItem item,
                                           CharacterInventory inventory,
                                           IDialogCoordinator dialogCoordinator,
                                           IWindowsDialogs windowsDialogs,
                                           IDialogManager dialogManager = null,
                                           IWorker worker = null)
        {
            _dialogManager = dialogManager ?? new MetroDialogManager(this, dialogCoordinator);
            _windowsDialogs = windowsDialogs;
            Worker = worker ?? new Worker(_dialogManager);
            WindowTitle = item.LocalizedName;
            _item = item;
            List<InventoryItem> items = new() { _item };
            List<string> skippedSlots = new() { "patron_in_weapon", "cartridges" };
            List<InventoryItem> innerItems = inventory?.GetInnerItems(item.Id, skippedSlots);
            if (innerItems != null)
                items.AddRange(innerItems);
            WeaponBuild = new WeaponBuild(_item, items.Select(x => InventoryItem.CopyFrom(x)).ToList());
        }

        public IWorker Worker { get; }

        public string WindowTitle { get; }

        public WeaponBuild WeaponBuild { get; }

        public RelayCommand RemoveItem => new(async obj =>
        {
            if (await _dialogManager.YesNoDialog("remove_stash_item_title", "remove_stash_item_caption"))
                GetInventory(_editMode)?.RemoveItems(new() { _item.Id });
        });

        public RelayCommand ExportBuild => new(obj =>
        {
            var (success, path) = _windowsDialogs.SaveWeaponBuildDialog(WeaponBuild.Name);
            if (success)
            {
                Worker.AddTask(new WorkerTask
                {
                    Action = () => { UserBuilds.ExportBuild(WeaponBuild, path); },
                    Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                    Description = AppLocalization.GetLocalizedString("tab_presets_export")
                });
            }
        });

        public RelayCommand AddToWeaponBuilds => new(obj =>
        {
            Worker.AddTask(new WorkerTask
            {
                Action = () => { Profile.UserBuilds.ImportBuild(WeaponBuild); },
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("tab_presets_export")
            });
        });

        private static CharacterInventory MakeInventory(EquipmentBuild build)
                                                                    => new() { Items = build.BuildItems.ToArray() };

        private static CharacterInventory GetInventory(StashEditMode editMode) => editMode switch
        {
            StashEditMode.Scav => Profile?.Characters?.Scav?.Inventory,
            _ => Profile?.Characters?.Pmc?.Inventory,
        };
    }
}