using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

using CommunityToolkit.Mvvm.ComponentModel;

namespace SourceGit.ViewModels
{
    public class WCFilesPage : ObservableObject, IDisposable
    {
        public WCFilesPage(Repository repo)
        {
            _repo = repo;
        }

        public void Dispose()
        {
            _repo = null;
        }

        private Repository _repo = null;
    }
}
