using System.Collections;

namespace AnthillProject.Core.Collections;
public sealed class CompressedList<T> : IEnumerable<CompressedList<T>.Entry>{
    public readonly struct Entry{
        public readonly T? Val;
        public readonly int Size;
        public Entry(T? val, int size){
            Val = val;
            Size = size;
        }
    }
    private readonly List<Entry> Entries;
    private static bool ValsEqual(T? i1, T? i2){
        if(i1 is null)
            return i2 is null;
        return i1.Equals(i2);
    }
    public Entry[] GetEntries() =>
        Entries.ToArray();

    public List<T?> Decompress(){
        var forming = new List<T?>();

        foreach(var i in Entries)
            for(int e = 0; e < i.Size; e++){
                forming.Add(i.Val);
            }

        return forming;
    }

    public IEnumerator<Entry> GetEnumerator() =>
        Entries.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    //Keep in mind that this disposes the enumerator.
    public CompressedList(IEnumerator<T> Input){
        var entries = new List<Entry>();
        
        var len = 0;
        T? val = default;
        void SaveEntries() =>
            entries.Add(new Entry(val, len));
        
        while(Input.MoveNext()){
            var i = Input.Current;
            if(len == 0)
                val = i;
            if(!ValsEqual(val, i)){
                SaveEntries();
                val = i;
                len = 0;
            }
            len++;
        }
        SaveEntries();
        Entries = entries;

        Input.Dispose();
    }
    public CompressedList(List<Entry> i) => Entries = i;
    public CompressedList(IEnumerable<T> Input) : this(Input.GetEnumerator()){}
    public CompressedList(IEnumerable<Entry> i) : this(new List<Entry>(i)){}
}