namespace AnthillProject.Core.Providing;

//This code would give a python dev a seizure.
public interface IInstanceProviderManager{
    TInstance ProvideInstance<TInstance>(string ID, object Args)
        where TInstance : IProvidable;
    IInstanceProvider<TInstance> GetInstanceProvider<TInstance>(string ID)
        where TInstance : IProvidable;
    void RegisterInstanceProvider<TInstance>(IInstanceProvider<TInstance> InstanceProvider)
        where TInstance : IProvidable;
}
