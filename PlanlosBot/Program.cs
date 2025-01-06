using Autofac;
namespace PlanlosBot;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        ContainerBuilder builder = new();

        builder.RegisterModule<BotModule>();
    }
}
