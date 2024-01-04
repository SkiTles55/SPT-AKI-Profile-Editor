namespace SPT_AKI_Profile_Editor.Core.ProgressTransfer
{
    public class Builds : Group
    {
        private bool weaponBuilds = true;
        private bool equipmentBuilds = true;

        public override bool? GroupState
        {
            get => FullState ? true : NotFullState;
            set
            {
                var updatedValue = value ?? false;
                WeaponBuilds = updatedValue;
                EquipmentBuilds = updatedValue;
                NotifyGroupStateChanged();
            }
        }

        public bool WeaponBuilds
        {
            get => weaponBuilds;
            set
            {
                weaponBuilds = value;
                OnPropertyChanged(nameof(WeaponBuilds));
                NotifyGroupStateChanged();
            }
        }

        public bool EquipmentBuilds
        {
            get => equipmentBuilds;
            set
            {
                equipmentBuilds = value;
                OnPropertyChanged(nameof(EquipmentBuilds));
                NotifyGroupStateChanged();
            }
        }

        internal override bool FullState => weaponBuilds && equipmentBuilds;

        internal override bool? NotFullState => weaponBuilds || equipmentBuilds ? null : false;
    }
}