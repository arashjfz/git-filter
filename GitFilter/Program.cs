using CommandLine;
using Unity;

namespace GitFilter
{
    class Program
    {

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    UnityContainer container = new UnityContainer();
                    container.RegisterType<ProgramStarter>();
                    RegisterTypes(container);
                    container.Resolve<ProgramStarter>().Run(o);
                });
        }

        private static void RegisterTypes(UnityContainer container)
        {
            container.RegisterType<ILogger, ConsoleLogger>();
        }
    }
}
