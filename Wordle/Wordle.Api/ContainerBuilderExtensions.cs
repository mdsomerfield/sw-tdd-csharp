using Autofac;

namespace Wordle.Api;

public static class ContainerBuilderExtensions
{
    public static void BindConfig<T>(this ContainerBuilder builder, IConfigurationSection section) where T : class, new()
    {
        var settings = Activator.CreateInstance<T>();
        section.Bind(settings);
        builder.RegisterInstance(settings);
    }

}