#pragma warning disable CS8604 // Possible null reference argument.
namespace AnthillProject.Core.Providing;

public class DefaultInstanceProviderManager : IInstanceProviderManager{
    //Unholy generics nesting.
    private readonly Dictionary<
        string, Dictionary<
            string, IInstanceProvider<
                IProvidable
            >
        >
    >
    InstanceProviderRegistry;
    public TInstance ProvideInstance<TInstance>(string ID, object Args)
        where TInstance : IProvidable =>
            GetInstanceProvider<TInstance>(ID).ProvideInstance((IInstanceProviderArgs)Args);
        
    public IInstanceProvider<TInstance> GetInstanceProvider<TInstance>(string ID)
        where TInstance : IProvidable =>
            (IInstanceProvider<TInstance>)InstanceProviderRegistry[typeof(TInstance).FullName][ID];

    //Who the fuck knows how this works.
    public void RegisterInstanceProvider<TInstance>(IInstanceProvider<TInstance> InstanceProvider)
        where TInstance : IProvidable{
            var InstanceTypeName = typeof(TInstance).FullName;
            //Console.WriteLine(InstanceProvider.GetIdentifier());

        if (!InstanceProviderRegistry.ContainsKey(InstanceTypeName))
                InstanceProviderRegistry.Add(InstanceTypeName,
                    new Dictionary<
                        string, IInstanceProvider<
                            IProvidable
                        >
                    >()
                );

        InstanceProviderRegistry[InstanceTypeName].Add(
                InstanceProvider.GetIdentifier(),
                (
                    IInstanceProvider<
                        IProvidable
                    >
                )InstanceProvider
            );
        }
    
    public DefaultInstanceProviderManager(){
        InstanceProviderRegistry = new Dictionary<string, Dictionary<string, IInstanceProvider<IProvidable>>>();
    }
}