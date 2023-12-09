using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Core.ProgressTransfer
{
    public abstract class Group : BindableEntity
    {
        public RelayCommand GroupStateChanged { get; set; }

        public virtual bool? GroupState { get; set; }

        internal virtual bool FullState { get; }

        internal virtual bool? NotFullState { get; }

        internal virtual void NotifyGroupStateChanged()
        {
            OnPropertyChanged(nameof(GroupState));
            GroupStateChanged?.Execute(null);
        }
    }
}