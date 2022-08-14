using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class TestsApplicationManager : IApplicationManager
    {
        public bool WeaponBuildWindowOpened = false;
        public bool ContainerWindowOpened = false;
        public bool ThemeChanged = false;
        public bool ItemViewWindowsClosed = false;
        public string LastOpenedUrl = null;

        public RelayCommand CloseApplication => throw new NotImplementedException();

        public void ChangeTheme() => ThemeChanged = true;

        public void CloseItemViewWindows(List<string> idsList = null) => ItemViewWindowsClosed = true;

        public void OpenContainerWindow(object obj, StashEditMode editMode) => ContainerWindowOpened = true;

        public void OpenUrl(string url) => LastOpenedUrl = url;

        public void OpenWeaponBuildWindow(object obj, StashEditMode editMode) => WeaponBuildWindowOpened = true;
    }
}