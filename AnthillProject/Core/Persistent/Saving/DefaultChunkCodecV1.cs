using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Providing;
using AnthillProject.Core.Math;
using AnthillProject.Core.Collections;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Entities;
using static AnthillProject.Core.Utilities.BinaryUtils;
using System.Text;

namespace AnthillProject.Core.Persistent.Saving;

public class DefaultChunkCodecV1 : IChunkCodec{
    public IChunk Decode(byte[] bytes){//TODO: Stress-test this under harsher conditions to prove it's good enough.
        int GetIntAtPosition(int i) => IntFromBytesBE([bytes[i], bytes[i + 1], bytes[i + 2], bytes[i + 3]]);
        ushort GetUShortAtPosition(int i) => UShortFromBytesBE([bytes[i], bytes[i + 1]]);
        float GetFloatAtPosition(int i) => FloatFromBytesBE([bytes[i], bytes[i + 1]]);
        Vector2 GetVector2AtPosition(int i) => new Vector2(GetFloatAtPosition(i), GetFloatAtPosition(i + 4));

        int pos = 8;
        byte[][] GetPallete(){
            var Forming = new List<byte[]>();
            var TotalFinalPos = GetIntAtPosition(pos) + pos + 4;
            pos += 4;
            //Console.WriteLine($"{pos}: {TotalFinalPos}");
            while(pos < TotalFinalPos){
                var EntryFinalPos = GetUShortAtPosition(pos) + pos + 2;
                var EntryForming = new List<byte>();
                for(pos += 2; pos < EntryFinalPos; pos++){
                    //Console.WriteLine(pos);
                    EntryForming.Add(bytes[pos]);
                }
                Forming.Add(EntryForming.ToArray());
            }
            return Forming.ToArray();
        }
        var ChunkSizeX = GetIntAtPosition(0);
        var ChunkSizeY = GetIntAtPosition(4);

        var chunk = new Chunk(
            ChunkSizeX,
            ChunkSizeY
        );

        T[,] GetMapRegion<T>(IList<T> pallete){
            var matrix = new T[ChunkSizeX, ChunkSizeY];
            //Console.WriteLine($":{pallete.Count}");
            IEnumerator<CompressedList<T>.Entry> Enumerator(){
                var FinalPos = GetIntAtPosition(pos) + pos + 4;
                //Console.WriteLine($"Final: {FinalPos} Current: {pos}");
                for(pos += 4; pos < FinalPos; pos += 4){
                    //Console.WriteLine(GetUShortAtPosition(pos));
                    yield return new CompressedList<T>.Entry(
                        pallete[GetUShortAtPosition(pos + 2)],
                        GetUShortAtPosition(pos)
                    );
                }
            }
            var count = 0;
            var compressed = new CompressedList<T>(Enumerator());
            //Console.WriteLine($"Compressed count: {compressed.Count}");
            //foreach(var i in compressed)
            //    Console.WriteLine($"{i.Size}, {i.Val}");

            var Decompressed = compressed.Decompress();
            //Console.WriteLine($"Decompressed count: {Decompressed.Count}");
            for(int x = 0; x < ChunkSizeX; x++)
                for(int y = 0; y < ChunkSizeY; y++){
                    //Console.WriteLine(count);
#pragma warning disable CS8601 // Possible null reference assignment.
                    matrix[x, y] = Decompressed[count];
#pragma warning restore CS8601 // Possible null reference assignment.
                    count++;
                }
            return matrix;
        }
        var TileProviderPallete = new List<IInstanceProvider<ITile>>();
        foreach(var i in GetPallete())
            TileProviderPallete.Add(Program.Instance.InstanceProviderManager.GetInstanceProvider<ITile>(Encoding.ASCII.GetString(i)));

        var EntityProviderPallete = new List<IInstanceProvider<IEntity>>();
        foreach(var i in GetPallete())
            EntityProviderPallete.Add(Program.Instance.InstanceProviderManager.GetInstanceProvider<IEntity>(Encoding.ASCII.GetString(i)));

        var DataPallete = GetPallete();

        var TileProviders = GetMapRegion(TileProviderPallete);
        var TileData = GetMapRegion(DataPallete);

        for(int x = 0; x < ChunkSizeX; x++)
            for(int y = 0; y < ChunkSizeY; y++)
                chunk.SetTile(
                    new Coordinates(x, y),
                    TileProviders[x, y].ProvideInstance(new TileProviderArgs(TileData[x, y]))
                );
        
        var EntityRegionSize = GetIntAtPosition(pos);
        pos += 4;
        while(pos < EntityRegionSize){
            var entitypos = GetVector2AtPosition(pos);
            pos += 8;

            var vel = GetVector2AtPosition(pos);
            pos += 8;

            var prov = EntityProviderPallete[GetUShortAtPosition(pos)];
            pos += 2;

            chunk.SummonEntity(prov.ProvideInstance(new EntityProviderArgs(
                entitypos,
                vel,
                DataPallete[GetUShortAtPosition(pos)]
            )));

            pos += 2;
        }

        return chunk;
    }

    public byte[] Encode(IChunk chunk){
        var ChunkSizeX = chunk.GetSizeX();
        var ChunkSizeY = chunk.GetSizeY();

        var current = new List<byte>();
        var temp1 = new List<byte>();
        void ProcessTemp(List<byte> bytes){
            //Marker to be able to tell how long this region is.
            current.AddRange(GetBytesBE(bytes.Count));

            current.AddRange(bytes);
            bytes.Clear();
        }
        
        current.AddRange(GetBytesBE(ChunkSizeX));
        current.AddRange(GetBytesBE(ChunkSizeY));

        var TileProviderPallete = new Dictionary<string, ushort>();
        var EntityProviderPallete = new Dictionary<string, ushort>();
        
        var DataPallete = new Dictionary<byte[], ushort>();

        var TileProviders = new List<string>();
        var TileArbitraryDatas = new List<byte[]>();

        DataPallete.Add([], 0);
        //Creating the provider pallete and arbitrary data pallete for tiles, as well as all data associated with tiles.
        for(int x = 0; x < ChunkSizeX; x++)
            for(int y = 0; y < ChunkSizeY; y++){
                var tile = chunk.GetTile(new Coordinates(x, y));
                var data = tile.GetArbitraryData();
                var id = tile.GetProviderIdentifier();
                TileProviders.Add(id);
                TileArbitraryDatas.Add(data);
                if(!TileProviderPallete.ContainsKey(id))
                    TileProviderPallete.Add(id, (ushort)TileProviderPallete.Count);
                if(!DataPallete.ContainsKey(data))
                    DataPallete.Add(data, (ushort)DataPallete.Count);
                
            }
        

        var entities = chunk.LoadedEntities;

        //Adding to the provider pallete and arbitrary data pallete for entities.
        foreach(var i in entities){
            var data = i.GetArbitraryData();
            var id = i.GetProviderIdentifier();
            if(!EntityProviderPallete.ContainsKey(id))
                EntityProviderPallete.Add(id, (ushort)EntityProviderPallete.Count);
            if(!DataPallete.ContainsKey(data))
                DataPallete.Add(data, (ushort)DataPallete.Count);
        }

        //Writing the tile provider pallete.
        foreach(var i in TileProviderPallete){
            var idEncoded = Encoding.ASCII.GetBytes(i.Key);
            temp1.AddRange(GetBytesBE((ushort)idEncoded.Length));
            temp1.AddRange(idEncoded);
        }
        ProcessTemp(temp1);

        //Writing the entity provider pallete.
        foreach(var i in EntityProviderPallete){
            var idEncoded = Encoding.ASCII.GetBytes(i.Key);
            temp1.AddRange(GetBytesBE((ushort)idEncoded.Length));
            temp1.AddRange(idEncoded);
        }
        ProcessTemp(temp1);

        //Writing the arbitrary data pallete.
        foreach(var i in DataPallete){
            temp1.AddRange(GetBytesBE((ushort)i.Key.Length));
            temp1.AddRange(i.Key);
        }
        ProcessTemp(temp1);

        //Compressing and writing tile providers as pallete entries.
        foreach(var i in new CompressedList<string>(TileProviders).GetEntries()){
            if(i.Val == null) continue;
            temp1.AddRange(GetBytesBE((ushort)i.Size));
            temp1.AddRange(GetBytesBE(TileProviderPallete[i.Val]));
        }
        ProcessTemp(temp1);

        //Compressing and writing arbitrary tile data as pallete entries.
        foreach(var i in new CompressedList<byte[]>(TileArbitraryDatas).GetEntries()){
            if(i.Val == null) continue;
            temp1.AddRange(GetBytesBE((ushort)i.Size));
            temp1.AddRange(GetBytesBE(DataPallete[i.Val]));
        }
        ProcessTemp(temp1);

        //Writing all entities.
        foreach(var i in entities){
            //Position.
            temp1.AddRange(GetBytesBE(i.Position.x));
            temp1.AddRange(GetBytesBE(i.Position.y));

            //Velocity.
            temp1.AddRange(GetBytesBE(i.Velocity.x));
            temp1.AddRange(GetBytesBE(i.Velocity.y));

            //Provider and arbitrary data. Both are pallete entries.
            temp1.AddRange(GetBytesBE(EntityProviderPallete[i.GetProviderIdentifier()]));
            temp1.AddRange(GetBytesBE(DataPallete[i.GetArbitraryData()]));
        }
        ProcessTemp(temp1);
        
        return current.ToArray();
    }
    public byte[] GetArbitraryData() => [];

    public string GetProviderIdentifier() => ProviderInstance.GetIdentifier();
    internal static IInstanceProvider<IChunkCodec> ProviderInstance = new Provider();

    //When this codec becomes obsolete (And it will, it's just a matter of time) please remember to implement GetIdentifier to the same as here, irregardless of the type's full name.
    private sealed class Provider : IInstanceProvider<IChunkCodec>{
        public IChunkCodec ProvideInstance(object Args) =>
            new DefaultChunkCodecV1();
    }
}