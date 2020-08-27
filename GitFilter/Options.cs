using CommandLine;

namespace GitFilter
{
    public class Options
    {
        [Option('b', "verbose", Required = false, HelpText = "Branch to filter using it")]
        public string Branch { get; set; }
        [Option('m', "module", Required = true, HelpText = "The module to apply action from")]
        public string Module { get; set; }
    }
}