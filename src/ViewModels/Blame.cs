using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SourceGit.ViewModels
{
    public class Blame : ObservableObject
    {
        public string Title
        {
            get => _title;
            private set => SetProperty(ref _title, value);
        }

        public bool IsBinary
        {
            get => _data != null && _data.IsBinary;
        }

        public Models.BlameData Data
        {
            get => _data;
            private set => SetProperty(ref _data, value);
        }

        private List<string> _Shas = new List<string>();
        private int index = 0;

        public Blame(string repo, string file, string revision)
        {
            _repo = repo;
            _file = file;

            SetBlameData($"{revision.AsSpan(0, 10)}", false);
        }

        private void SetBlameData(string commitSHA, bool fromButtons)
        {
            Title = $"{_file} @ {commitSHA}";

            Task.Run(() =>
            {
                var result = new Commands.Blame(_repo, _file, commitSHA).Result();
                Dispatcher.UIThread.Invoke(() =>
                {
                    Data = result;
                    OnPropertyChanged(nameof(IsBinary));
                });
            });

            if (!fromButtons)
            {
                try
                {
                    if(index != _Shas.Count-1)
                        _Shas.RemoveRange(index + 1, _Shas.Count - index - 1);
                }
                catch (Exception e)
                {
                    
                }

                if (_Shas.Count == 0 || _Shas[index] != commitSHA)
                {
                    _Shas.Add(commitSHA);
                    index = _Shas.Count - 1;
                }
            }
        }

        public void Back()
        {
            --index;
            if (index < 0)
                index = 0;

            NavigateToCommit(_Shas[index], true);
        }

        public void Forward()
        {
            ++index;
            if (index >= _Shas.Count)
                index = _Shas.Count - 1;

            NavigateToCommit(_Shas[index], true);
        }

        public void NavigateToCommit(string commitSHA, bool fromButtons)
        {
            var launcher = App.GetLauncer();
            if (launcher == null)
                return;

            foreach (var page in launcher.Pages)
            {
                if (page.Data is Repository repo && repo.FullPath.Equals(_repo))
                {
                    repo.NavigateToCommit(commitSHA);
                    SetBlameData(commitSHA, fromButtons);
                    break;
                }
            }
        }

        public string GetCommitMessage(string sha)
        {
            if (_commitMessages.TryGetValue(sha, out var msg))
                return msg;

            msg = new Commands.QueryCommitFullMessage(_repo, sha).Result();
            _commitMessages[sha] = msg;
            return msg;
        }

        private string _repo;
        private string _file;
        private string _title;
        private Models.BlameData _data = null;
        private Dictionary<string, string> _commitMessages = new Dictionary<string, string>();
    }
}
