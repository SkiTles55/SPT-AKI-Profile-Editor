﻿using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Views;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class ViewModelsFactory
    {
        public ViewModelsFactory(IDialogManager dialogManager,
                                 IWorker worker,
                                 IApplicationManager applicationManager,
                                 IWindowsDialogs windowsDialogs,
                                 RelayCommand saveCommand,
                                 RelayCommand reloadCommand,
                                 RelayCommand faqCommand,
                                 ICleaningService cleaningService,
                                 IHelperModManager helperModManager)
        {
            FastMode = new(saveCommand);
            InfoTab = new();
            MerchantsTab = new();
            QuestsTab = new(dialogManager, reloadCommand, faqCommand, worker, helperModManager);
            HideoutTab = new(dialogManager);
            SkillsTab = new(dialogManager, reloadCommand, faqCommand, worker, helperModManager);
            MasteringTab = new(dialogManager, reloadCommand, faqCommand, worker, helperModManager);
            ExaminedItemsTab = new(dialogManager);
            StashTab = new(dialogManager, worker, applicationManager);
            ClothingTab = new();
            BuildsTab = new(dialogManager, worker, windowsDialogs, applicationManager);
            CleaningFromModsTab = new(dialogManager, saveCommand, cleaningService);
            ProgressTransferTab = new(windowsDialogs, worker);
            BackupsTab = new(dialogManager, worker, cleaningService);
            AboutTab = new(applicationManager);
        }

        public FastModeViewModel FastMode { get; }

        public InfoTabViewModel InfoTab { get; }

        public MerchantsTabViewModel MerchantsTab { get; }

        public QuestsTabViewModel QuestsTab { get; }

        public HideoutTabViewModel HideoutTab { get; }

        public CommonSkillsTabViewModel SkillsTab { get; }

        public MasteringTabViewModel MasteringTab { get; }

        public ExaminedItemsTabViewModel ExaminedItemsTab { get; }

        public StashTabViewModel StashTab { get; }

        public ClothingTabViewModel ClothingTab { get; }

        public BuildsTabViewModel BuildsTab { get; }

        public CleaningFromModsViewModel CleaningFromModsTab { get; }

        public ProgressTransferTabViewModel ProgressTransferTab { get; }

        public BackupsTabViewModel BackupsTab { get; }

        public AboutTabViewModel AboutTab { get; }
    }
}