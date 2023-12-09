namespace SPT_AKI_Profile_Editor.Core.ProgressTransfer
{
    public class Info : Group
    {
        public Info()
        {
            Pmc.GroupStateChanged = new(_ => NotifyGroupStateChanged());
            Scav.GroupStateChanged = new(_ => NotifyGroupStateChanged());
        }

        public override bool? GroupState
        {
            get => FullState ? true : NotFullState;
            set
            {
                var updatedValue = value ?? false;
                Pmc.GroupState = updatedValue;
                Scav.GroupState = updatedValue;
                NotifyGroupStateChanged();
            }
        }

        public InfoGroup Pmc { get; set; } = new(true);

        public InfoGroup Scav { get; set; } = new(false);

        internal override bool FullState
            => Pmc.GroupState == true && Scav.GroupState == true;

        internal override bool? NotFullState
            => Pmc.GroupState == false && Scav.GroupState == false ? false : null;
    }
}