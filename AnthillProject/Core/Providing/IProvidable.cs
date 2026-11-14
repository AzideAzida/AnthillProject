namespace AnthillProject.Core.Providing;
public interface IProvidable{
    byte[] ArbitraryData{get; set;}
    string GetProviderIdentifier();
}