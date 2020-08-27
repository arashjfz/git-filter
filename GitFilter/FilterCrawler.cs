using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GitFilter.Interfaces;
using LibGit2Sharp;

namespace GitFilter
{
    internal class FilterCrawler
    {
        private readonly Repository _repository;
        private readonly Branch _branch;
        private readonly ICommitApplier _commitApplier;
        private readonly Dictionary<string,Commit> _appliedCommits = new Dictionary<string, Commit>();
        public FilterCrawler(Repository repository, Branch branch,ICommitApplier commitApplier)
        {
            _repository = repository;
            _branch = branch;
            _commitApplier = commitApplier;
        }

        public void Crawl()
        {
            ApplyCommit(_branch.Tip);
            Dictionary<Branch,Commit> ports = new Dictionary<Branch, Commit>();
            foreach (Branch repositoryBranch in _repository.Branches.Where(b => !b.IsRemote))
            {
                if (_appliedCommits.TryGetValue(repositoryBranch.Tip.Sha, out Commit commit))
                    ports.Add(repositoryBranch, commit);
            }

            foreach (var port in ports)
            {
                _repository.CreateBranch($"port/{port.Key.FriendlyName}", port.Value);
            }
            _repository.Index.Replace(_repository.Head.Tip);
        }

        private Commit ApplyCommit(Commit commit)
        {
            List<Commit> newParents = new List<Commit>();
            foreach (Commit commitParent in commit.Parents)
            {
                newParents.Add(ApplyCommit(commitParent));
            }
            if(_appliedCommits.ContainsKey(commit.Sha))
                return _appliedCommits[commit.Sha];
            CommitBuilder commitBuilder = new CommitBuilder();
            
            CommitApplyConfig config = new CommitApplyConfig(commit, newParents, _repository.Index, commitBuilder,_repository);
            config.Index.Replace(commit);
            _commitApplier.Apply(config);
            Tree tree = _repository.Index.WriteToTree();
            Commit newCommit = BuildCommit(commit, commitBuilder, newParents, tree);
            _appliedCommits.Add(commit.Sha,newCommit);
            return newCommit;
        }
        private Commit BuildCommit(Commit oldCommit,CommitBuilder commitBuilder, IEnumerable<Commit> parents, Tree tree)
        {
            Signature author = oldCommit.Author;
            if (commitBuilder.CommitAuthor != null)
                author = commitBuilder.CommitAuthor;
            Signature committer = oldCommit.Committer;
            if (commitBuilder.CommitCommitter != null)
                committer = commitBuilder.CommitCommitter;
            string message = oldCommit.Message;
            if (!string.IsNullOrEmpty(commitBuilder.CommitMessage))
                message = commitBuilder.CommitMessage;
            return _repository.ObjectDatabase.CreateCommit(author, committer, message, tree, parents, false);
        }
    }
    internal class CommitBuilder: ICommitBuilder
    {
        public Signature CommitAuthor { get; set; }
        public Signature CommitCommitter { get; set; }
        public string CommitMessage { get; set; }
        #region Implementation of ICommitBuilder

        public ICommitBuilder Author(Signature author)
        {
            CommitAuthor = author;
            return this;
        }

        public ICommitBuilder Committer(Signature committer)
        {
            CommitCommitter = committer;
            return this;
        }

        public ICommitBuilder Message(string message)
        {
            CommitMessage = message;
            return this;
        }

        #endregion
    }
}