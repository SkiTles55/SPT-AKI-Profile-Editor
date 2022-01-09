using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    class HandbookCategoryViewModel : BindableViewModel
    {
        public HandbookCategoryViewModel(HandbookCategory category)
        {
            Category = category;
        }
        public HandbookCategory Category
        {
            get => category;
            set
            {
                category = value;
                OnPropertyChanged("ItemCategory");
            }
        }

        private HandbookCategory category;
    }
}