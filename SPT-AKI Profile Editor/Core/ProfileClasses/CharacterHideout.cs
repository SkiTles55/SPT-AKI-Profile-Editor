using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterHideout : BindableEntity
    {
        private HideoutArea[] areas;
        private Dictionary<string, StartedHideoutProduction> production;

        public HideoutArea[] Areas
        {
            get => areas;
            set
            {
                areas = value;
                OnPropertyChanged(nameof(Areas));
            }
        }

        public Dictionary<string, StartedHideoutProduction> Production
        {
            get => production;
            set
            {
                production = value;
                OnPropertyChanged(nameof(Production));
            }
        }

        public void RemoveCraft(string id) => Production?.Remove(id);

        public void SetAllCraftsFinished()
        {
            foreach (var craft in Production?.Values)
                craft.SetFinished();
        }
    }
}