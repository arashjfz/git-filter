using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using GitFilter.Interfaces;
using LibGit2Sharp;
using Unity;

namespace GitFilter
{
    public class ProgramStarter
    {
        private readonly ILogger _logger;
        private readonly IUnityContainer _container;

        public ProgramStarter(ILogger logger,IUnityContainer container)
        {
            _logger = logger;
            _container = container;
        }
        public void Run(Options options)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            Repository repository = new Repository(currentDirectory);
            Branch currentBranch = repository.Head;
            if (!string.IsNullOrEmpty(options.Branch))
            {
                Branch selectedBranch = repository.Branches[options.Branch];
                if (selectedBranch == null)
                {
                    _logger.LogError($"Branch {options.Branch} not found");
                    throw new Exception($"Branch {options.Branch} not found");
                }

                currentBranch = selectedBranch;
            }

            Assembly module = Assembly.LoadFile(options.Module);
            Type moduleType = module.GetTypes().First(t => typeof(ICommitApplier).IsAssignableFrom(t));
            ICommitApplier commitApplier = (ICommitApplier)_container.Resolve(moduleType);
            FilterCrawler filterCrawler = new FilterCrawler(repository,currentBranch,commitApplier);
            filterCrawler.Crawl();
        }
    }
}
