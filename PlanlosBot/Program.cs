using Autofac;
using Autofac.Extensions.DependencyInjection;


namespace PlanlosBot;

class Program
{
    static async Task Main(string[] args)
    {
        ContainerBuilder builder = new();

        builder.RegisterModule<BotModule>();
        
        builder.Populate([]);
        IContainer container = builder.Build();
        await container.Resolve<BotHost>().Launch();
    }
}
