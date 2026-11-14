namespace AnthillProject.Core.Persistent;

public class DefaultPersistentDataManager : IPersistentDataManager{
    public DefaultPersistentDataManager(string path = "./PersistentData"){
        DataPath = path;
    }
    protected string GetFullPath(string path) =>
        Path.Combine(DataPath, path);
    public string DataPath{get;}

    public void CreateDirectory(string path) =>
        Directory.CreateDirectory(GetFullPath(path));

    public void DeleteDirectory(string path) =>
        Directory.Delete(GetFullPath(path));

    public void DeleteFile(string path) =>
        File.Delete(GetFullPath(path));

    public bool DirectoryExists(string path) =>
        Directory.Exists(GetFullPath(path));

    public bool FileExists(string path) =>
        File.Exists(GetFullPath(path));

    public byte[] ReadFileAsBytes(string path) =>
        File.ReadAllBytes(GetFullPath(path));

    public string ReadFileAsString(string path) =>
        File.ReadAllText(GetFullPath(path));

    public void WriteToFile(string path, string fileContents) =>
        File.WriteAllText(GetFullPath(path), fileContents);
    

    public void WriteToFile(string path, byte[] fileContents) =>
        File.WriteAllBytes(GetFullPath(path), fileContents);
    
}
