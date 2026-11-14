namespace AnthillProject.Core.Providing;

public interface IInstanceProvider<out TInstance>
    where TInstance : IProvidable{
        string GetIdentifier(){
            var type = GetType();
            var full = type.FullName;

            if(full == null)
                return type.Name;
            return full;
        }
        TInstance ProvideInstance(object Args);
    }