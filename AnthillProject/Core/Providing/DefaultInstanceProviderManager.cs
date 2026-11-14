#pragma warning disable CS8604 // Possible null reference argument.
namespace AnthillProject.Core.Providing;

public class DefaultInstanceProviderManager : IInstanceProviderManager{
    //Unholy generics nesting.
    private readonly Dictionary<
        string, Dictionary<
            string, IInstanceProvider<
                IProvidable,
                IInstanceProviderArgs<IProvidable>
            >
        >
    >
    InstanceProviderRegistry;
    public TInstance ProvideInstance<TInstance, TArgs>(string ID, TArgs Args)
        where TInstance : IProvidable
        where TArgs : IInstanceProviderArgs<TInstance> =>
            (TInstance)InstanceProviderRegistry[typeof(TInstance).FullName][ID].ProvideInstance((IInstanceProviderArgs<IProvidable>)Args);
        

    //Who the fuck knows how this works.
    public void RegisterInstanceProvider<TInstance, TArgs>(IInstanceProvider<TInstance, TArgs> InstanceProvider)
        where TInstance : IProvidable
        where TArgs : IInstanceProviderArgs<TInstance>{
            var InstanceTypeName = typeof(TInstance).FullName;

        if (!InstanceProviderRegistry.ContainsKey(InstanceTypeName))
                InstanceProviderRegistry.Add(InstanceTypeName,
                    new Dictionary<
                        string, IInstanceProvider<
                            IProvidable,
                            IInstanceProviderArgs<IProvidable>
                        >
                    >()
                );

        InstanceProviderRegistry[InstanceTypeName].Add(
                InstanceProvider.GetIdentifier(),
                (
                    IInstanceProvider<
                        IProvidable,
                        IInstanceProviderArgs<IProvidable>
                    >
                )InstanceProvider
            );
        }
    
    public DefaultInstanceProviderManager(){
        InstanceProviderRegistry = new Dictionary<string, Dictionary<string, IInstanceProvider<IProvidable, IInstanceProviderArgs<IProvidable>>>>();
    }
}