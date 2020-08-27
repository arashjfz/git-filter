using System;
using System.IO;
using GitFilter.Interfaces;
using LibGit2Sharp;

namespace GitFilter.TestApplier
{
    public class TestApplier:ICommitApplier
    {
        #region Implementation of ICommitApplier

        public void Apply(CommitApplyConfig config)
        {
            using (MemoryStream s = new MemoryStream())
            {
                Blob blob = config.Repository.ObjectDatabase.CreateBlob(s);
                config.Index.Add(blob,$"{config.Old.Sha}.txt",Mode.NonExecutableFile);
            }
            
        }

        #endregion
    }
}
