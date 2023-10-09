using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor
{
    public class WeaponBuildWindowViewModel : BindableViewModel
    {
        private readonly InventoryItem _item;
        private readonly IDialogManager _dialogManager;
        private readonly IWindowsDialogs _windowsDialogs;
        private readonly CharacterInventory _inventory;

        public WeaponBuildWindowViewModel(InventoryItem item,
                                           CharacterInventory inventory,
                                           IDialogCoordinator dialogCoordinator,
                                           IWindowsDialogs windowsDialogs,
                                           bool removeAllowed,
                                           IDialogManager dialogManager = null,
                                           IWorker worker = null)
        {
            _dialogManager = dialogManager ?? new MetroDialogManager(this, dialogCoordinator);
            _windowsDialogs = windowsDialogs;
            Worker = worker ?? new Worker(_dialogManager);
            WindowTitle = item.LocalizedName;
            _item = item;
            _inventory = inventory;
            List<InventoryItem> items = new() { _item };
            List<string> skippedSlots = new() { "patron_in_weapon", "cartridges" };
            List<InventoryItem> innerItems = inventory?.GetInnerItems(item.Id, skippedSlots);
            if (innerItems != null)
                items.AddRange(innerItems);
            WeaponBuild = new WeaponBuild(_item, items.Select(x => InventoryItem.CopyFrom(x)).ToList());
            RemoveAllowed = removeAllowed;
        }

        public IWorker Worker { get; }

        public string WindowTitle { get; }

        public bool RemoveAllowed { get; }

        public WeaponBuild WeaponBuild { get; }

        public RelayCommand RemoveItem => new(async obj =>
        {
            if (await _dialogManager.YesNoDialog("remove_stash_item_title", "remove_stash_item_caption"))
                _inventory?.RemoveItems(new() { _item.Id });
        });

        public RelayCommand ExportBuild => new(obj =>
        {
            var (success, path) = _windowsDialogs.SaveWeaponBuildDialog(WeaponBuild.Name);
            if (success)
                Worker.AddTask(CreateExportTask(() => { UserBuilds.ExportBuild(WeaponBuild, path); }));
        });

        public RelayCommand AddToWeaponBuilds
            => new(obj => Worker.AddTask(CreateExportTask(() => { Profile.UserBuilds.ImportBuild(WeaponBuild); })));

        private static WorkerTask CreateExportTask(Action action)
            => new(action,
                   AppLocalization.GetLocalizedString("progress_dialog_title"),
                   AppLocalization.GetLocalizedString("tab_presets_export"));
    }
}