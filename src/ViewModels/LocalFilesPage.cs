using System;

using CommunityToolkit.Mvvm.ComponentModel;

namespace SourceGit.ViewModels
{
    public class LocalFilesPage : ObservableObject, IDisposable
    {
        public LocalFilesPage(Repository repo)
        {
            _repo = repo;

            var commitDetail = new CommitDetail(_repo);
            var commit = new Commands.QuerySingleCommit(_repo.FullPath, "HEAD").Result();
            commitDetail.Commit = commit;
            DetailContext = commitDetail;
        }

        public IDisposable DetailContext
        {
            get => _detailContext;
            set => SetProperty(ref _detailContext, value);
        }

        public void Dispose()
        {
            _repo = null;
        }

        private Repository _repo = null;
        private IDisposable _detailContext = null;
    }
}
