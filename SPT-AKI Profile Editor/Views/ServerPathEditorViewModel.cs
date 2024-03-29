﻿using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Views
{
    public class ServerPathEditorViewModel : ClosableDialogViewModel
    {
        private readonly RelayCommand retryCommand;

        public ServerPathEditorViewModel(IEnumerable<ServerPathEntry> paths,
                                         RelayCommand retryCommand,
                                         RelayCommand faqCommand,
                                         object context) : base(context)
        {
            Paths = paths;
            this.retryCommand = retryCommand;
            FAQCommand = faqCommand;
        }

        public IEnumerable<ServerPathEntry> Paths { get; }
        public RelayCommand RetryCommand => new(obj => CloseAndRunRetryCommand());
        public RelayCommand FAQCommand { get; }

        private async void CloseAndRunRetryCommand()
        {
            await CloseDialog();
            retryCommand.Execute(Paths);
        }
    }
}