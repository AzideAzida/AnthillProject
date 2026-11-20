using System.Collections;
using AnthillProject.Core.Interfaces;
using AnthillProject.Core.Math;

namespace AnthillProject.Core.Tiling;
//X goes up when going to the right, and Y goes up when going up.
public abstract class TiledObject<T> : ITiled<T>, ISimulable, IInitializable where T : notnull{
    public readonly int SizeX;
    public readonly int SizeY;
    public TiledObject(int sx, int sy){
        SizeX = sx;
        SizeY = sy;

        Tiles               = new T[SizeX, SizeY];
        Coords              = new Dictionary<T, Coordinates>();
        ListAllCoordinates  = new List      <Coordinates   >();

        for(int y = 0; SizeX > y; y++)//Y loop.
            for(int x = 0; SizeY > x; x++){//X loop.
                var tile = GetDefaultTile();
                var coords = new Coordinates(x, y);
                Tiles[x, y] = tile;
                Coords.Add(tile, coords);
                ListAllCoordinates.Add(coords);
            }
    }
    public virtual void Simulate(){
        foreach(var i in Tiles){
            if(i is ISimulable e)
                e.Simulate();
        }
    }
    public virtual void Init(){
        foreach(var i in Tiles){
            if(i is IInitializable e)
                e.Init();
        }
    }
    
    //When first instantiating a TiledObject, all tiles will be set to whatever this returns.
    public abstract T GetDefaultTile();

    public Coordinates[] GetAllCoordinates() =>
        ListAllCoordinates.ToArray();
    public Coordinates GetCoordinatesOfTile(T i) =>
        this[i];
    public T GetTile(int x, int y) =>
        this[x, y];
    public T GetTile(Coordinates i) =>
        this[i];
    public void SetTile(int x, int y, T i) =>
        this[x, y] = i;
    public T GetRelativeTile(Coordinates coords, T tile) =>
        this[this[tile] + coords];

    public IEnumerator<T> GetEnumerator(){
        foreach(var i in Tiles)
            yield return i;
    }

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public T this[int x, int y]{
        get =>
            Tiles[x, y];
        set =>
            Tiles[x, y] = value;
    }
    public T this[Coordinates i]{
        get =>
            this[i.x, i.y];
        set{
            this[i.x, i.y] = value;
            Coords[value] = i;
        }
    }
    public Coordinates this[T i]{
        get =>
            Coords[i];
    }
    
    private readonly T[,] Tiles;
    private readonly Dictionary<T, Coordinates> Coords;
    private readonly List<Coordinates> ListAllCoordinates;
}