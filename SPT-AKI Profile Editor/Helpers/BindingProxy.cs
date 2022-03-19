using System.Windows;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class BindingProxy : Freezable
    {
        #region Overrides of Freezable

        //datagrid headers binding not working without this - https://thomaslevesque.com/2011/03/21/wpf-how-to-bind-to-data-when-the-datacontext-is-not-inherited/
        protected override Freezable CreateInstanceCore() => new BindingProxy();

        #endregion Overrides of Freezable

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        public object Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
    }
}