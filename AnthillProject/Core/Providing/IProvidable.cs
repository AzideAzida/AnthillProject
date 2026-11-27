namespace AnthillProject.Core.Providing;
public interface IProvidable{
    byte[] GetArbitraryData();
    string GetProviderIdentifier();
}