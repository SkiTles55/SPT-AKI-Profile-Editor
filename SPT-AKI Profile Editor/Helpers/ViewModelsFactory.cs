using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Views;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class ViewModelsFactory(IDialogManager dialogManager,
        IWorker worker,
        IApplicationManager applicationManager,
        IWindowsDialogs windowsDialogs,
        RelayCommand saveCommand,
        RelayCommand reloadCommand,
        RelayCommand faqCommand,
        ICleaningService cleaningService,
        IHelperModManager helperModManager)
    {
        public FastModeViewModel FastMode { get; } = new(saveCommand);

        public InfoTabViewModel InfoTab { get; } = new();

        public AchievementsViewModel AchievementsTab { get; } = new();

        public MerchantsTabViewModel MerchantsTab { get; } = new();

        public QuestsTabViewModel QuestsTab { get; } = new(dialogManager, reloadCommand, faqCommand, worker, helperModManager);

        public HideoutTabViewModel HideoutTab { get; } = new(dialogManager);

        public CommonSkillsTabViewModel SkillsTab { get; } = new(dialogManager, reloadCommand, faqCommand, worker, helperModManager);

        public MasteringTabViewModel MasteringTab { get; } = new(dialogManager, reloadCommand, faqCommand, worker, helperModManager);

        public ExaminedItemsTabViewModel ExaminedItemsTab { get; } = new(dialogManager);

        public StashTabViewModel StashTab { get; } = new(dialogManager, worker, applicationManager);

        public ClothingTabViewModel ClothingTab { get; } = new();

        public BuildsTabViewModel BuildsTab { get; } = new(dialogManager, worker, windowsDialogs, applicationManager);

        public CleaningFromModsViewModel CleaningFromModsTab { get; } = new(dialogManager, saveCommand, cleaningService);

        public ProgressTransferTabViewModel ProgressTransferTab { get; } = new(windowsDialogs, worker);

        public BackupsTabViewModel BackupsTab { get; } = new(dialogManager, worker, cleaningService);

        public AboutTabViewModel AboutTab { get; } = new(applicationManager);
    }
}