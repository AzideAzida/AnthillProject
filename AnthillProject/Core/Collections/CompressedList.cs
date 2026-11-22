using System.Collections;

namespace AnthillProject.Core.Collections;
public sealed class CompressedList<T> : IList<CompressedList<T>.Entry>{
    public readonly struct Entry{
        public readonly T? Val;
        public readonly int Size;
        public Entry(T? val, int size){
            Val = val;
            Size = size;
        }
    }
    private readonly List<Entry> Entries;

    public int Count =>
        Entries.Count;

    public bool IsReadOnly =>
        false;

    public Entry this[int index]{
        get => Entries[index];
        set => Entries[index] = value;
    }

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

    public int IndexOf(Entry item) =>
        Entries.IndexOf(item);

    public void Insert(int index, Entry item) =>
        Entries.Insert(index, item);

    public void RemoveAt(int index) =>
        Entries.RemoveAt(index);

    public void Add(Entry item) =>
        Entries.Add(item);

    public void Clear() =>
        Entries.Clear();

    public bool Contains(Entry item) =>
        Entries.Contains(item);

    public void CopyTo(Entry[] array, int arrayIndex) =>
        Entries.CopyTo(array, arrayIndex);

    public bool Remove(Entry item) =>
        Entries.Remove(item);

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

    //Keep in mind that this disposes the enumerator.
    public CompressedList(IEnumerator<Entry> Input){
        Entries = new List<Entry>();
        
        while(Input.MoveNext())
            Entries.Add(Input.Current);

        Input.Dispose();
    }
    public CompressedList(List<Entry> i) => Entries = i;
    public CompressedList(IEnumerable<T> Input) : this(Input.GetEnumerator()){}
    public CompressedList(IEnumerable<Entry> i) : this(new List<Entry>(i)){}
}