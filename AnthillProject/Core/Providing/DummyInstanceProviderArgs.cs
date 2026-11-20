namespace AnthillProject.Core.Providing;

public sealed class DummyInstanceProviderArgs : IInstanceProviderArgs{
    public byte[] GetArbitraryData() => [];
}