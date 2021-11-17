namespace SPT_AKI_Profile_Editor.Core
{
    public class GridFilters
    {
        public GridFilters()
        {
            QuestsTab = new();
            HideoutTab = new();
            SkillsTab = new();
        }
        public QuestsTab QuestsTab { get; set; }
        public HideoutTab HideoutTab { get; set; }
        public SkillsTab SkillsTab { get; set; }
    }
}