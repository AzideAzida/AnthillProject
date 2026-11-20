namespace AnthillProject.Core.Persistent;
public interface IPersistentDataManager{
    //All methods here should work with Path.Combine() to combine DataPath property and the path argument
    string DataPath{get;}

    void WriteToFile(string path, string fileContents);
    void WriteToFile(string path, byte[] fileContents);

    string ReadFileAsString(string path);
    byte[] ReadFileAsBytes(string path);

    void DeleteFile(string path);
    bool FileExists(string path);

    void CreateDirectory(string path);
    void DeleteDirectory(string path);
    bool DirectoryExists(string path);

    IEnumerable<string> EnumerateDirectories(string path);
    IEnumerable<string> EnumerateFiles(string path);
}