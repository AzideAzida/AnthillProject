namespace AnthillProject.Core.Providing;

//This code would give a python dev a seizure.
public interface IInstanceProviderManager{
    TInstance ProvideInstance<TInstance, TArgs>(string ID, TArgs Args)
        where TInstance : IProvidable
        where TArgs : IInstanceProviderArgs<TInstance>;
    
    void RegisterInstanceProvider<TInstance, TArgs>(IInstanceProvider<TInstance, TArgs> InstanceProvider)
        where TInstance : IProvidable
        where TArgs : IInstanceProviderArgs<TInstance>;
}
