using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace GitFilter.Interfaces
{
    public interface ICommitApplier
    {
        void Apply(CommitApplyConfig config);
    }

    public class CommitApplyConfig
    {
        public Commit Old { get; }
        public IEnumerable<Commit> Parents { get; }
        public Index Index { get; }
        public ICommitBuilder CommitBuilder { get; }
        public Repository Repository { get; }

        public CommitApplyConfig(Commit old,IEnumerable<Commit> parents,Index index,ICommitBuilder commitBuilder,Repository repository)
        {
            Old = old;
            Parents = parents;
            Index = index;
            CommitBuilder = commitBuilder;
            Repository = repository;
        }
    }
    public interface ICommitBuilder
    {
        ICommitBuilder Author(Signature author);
        ICommitBuilder Committer(Signature committer);
        ICommitBuilder Message(string message);
    }
}
