namespace AnthillProject.Core.Providing;

public interface IInstanceProvider<out TInstance, TArgs>
    where TInstance : IProvidable
    where TArgs : IInstanceProviderArgs<TInstance>{
        string GetIdentifier(){
            var type = GetType();
            var full = type.FullName;

            if(full == null)
                return type.Name;
            return full;
        }
        TInstance ProvideInstance(TArgs Args);
    }