using System.ComponentModel;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public abstract class PmcBindableViewModel : BindableViewModel
    {
        public RelayCommand UpdateModelBindingCommand => new(obj => UpdateModelBinding());

        public virtual void ApplyFilter()
        { }

        private void ProfileUpdated(object sender, PropertyChangedEventArgs e) => UpdateModelBinding();

        private void PmcChanged(object sender, PropertyChangedEventArgs e) => ApplyFilter();

        private void UpdateModelBinding()
        {
            if (Profile?.Characters?.Pmc == null)
                return;

            Profile.Characters.Pmc.PropertyChanged -= PmcChanged;
            Profile.Characters.Pmc.PropertyChanged += PmcChanged;

            Profile.PropertyChanged -= ProfileUpdated;
            Profile.PropertyChanged += ProfileUpdated;

            ApplyFilter();
        }
    }
}