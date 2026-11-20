using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Providing;
using AnthillProject.Core.Math;
using AnthillProject.Core.Collections;
using AnthillProject.Core.Map.Tiles;
using static AnthillProject.Core.Utility;
using System.Text;

namespace AnthillProject.Core.Persistent.Saving;

public class DefaultChunkCodecV1 : IChunkCodec{
    private readonly struct TileEntry{
        public readonly string Provider;
        public readonly byte[] Data;
        public TileEntry(string prov, byte[] data){
            Provider = prov;
            Data = data;
        }
    }

    public IChunk Decode(byte[] bytes){//TODO: Finish this.
        int GetIntAtPosition(int i) => IntFromBytesBE([bytes[i], bytes[i + 1], bytes[i + 2], bytes[i + 3]]);
        ushort GetUShortAtPosition(int i) => UShortFromBytesBE([bytes[i], bytes[i + 1]]);

        int pos = 8;
        byte[][] GetPallete(){
            var Forming = new List<byte[]>();
            var TotalSize = GetIntAtPosition(pos);
            for(pos += 4; pos < TotalSize;){
                var EntrySize = GetUShortAtPosition(pos);
                var EntryForming = new List<byte>();
                for(pos += 2; pos < EntrySize; pos++)
                    EntryForming.Add(bytes[pos]);
                Forming.Add(EntryForming.ToArray());
            }
            return Forming.ToArray();
        }
        //Getting chunk size & creating chunk.
        IChunk chunk = new Chunk(
            GetIntAtPosition(0),
            GetIntAtPosition(4)
        );
        var ProviderPallete = new List<IInstanceProvider<Tile>>();
        foreach(var i in GetPallete())
            ProviderPallete.Add(Program.Instance.InstanceProviderManager.GetInstanceProvider<Tile>(Encoding.ASCII.GetString(i)));
        var DataPallete = GetPallete();

        var ProviderTileEntries = new List<CompressedList<IInstanceProvider<Tile>>.Entry>(0);

        var TotalSize = GetIntAtPosition(pos);
        
        for(pos += 4; pos < TotalSize; pos += 4){
            ProviderTileEntries.Add(
                new CompressedList<IInstanceProvider<Tile>>.Entry(
                    ProviderPallete[GetUShortAtPosition(pos)],
                    GetUShortAtPosition(pos + 2)
                )
            );
        }
        new CompressedList<IInstanceProvider<Tile>>(ProviderTileEntries).Decompress();

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

        var ProviderPallete = new Dictionary<string, ushort>();
        var DataPallete = new Dictionary<byte[], ushort>();

        var TileProviders = new List<string>();
        var TileArbitraryDatas = new List<byte[]>();

        DataPallete.Add([], 0);
        //Creating the provider pallete and arbitrary data pallete for tiles, as well as all data associated with tiles.
        for(int x = 0; x < ChunkSizeX; x++){
            for(int y = 0; y < ChunkSizeY; y++){
                var tile = chunk.GetTile(new Coordinates(x, y));
                var data = tile.ArbitraryData;
                var id = tile.GetProviderIdentifier();
                TileProviders.Add(id);
                TileArbitraryDatas.Add(data);
                if(!ProviderPallete.ContainsKey(id))
                    ProviderPallete.Add(id, (ushort)ProviderPallete.Count);
                if(!DataPallete.ContainsKey(data))
                    DataPallete.Add(data, (ushort)DataPallete.Count);
                
            }
        }

        var entities = chunk.LoadedEntities;

        //Adding to the provider pallete and arbitrary data pallete for entities.
        foreach(var i in entities){
            var data = i.ArbitraryData;
            var id = i.GetProviderIdentifier();
            TileProviders.Add(id);
            TileArbitraryDatas.Add(data);
            if(!ProviderPallete.ContainsKey(id))
                ProviderPallete.Add(id, (ushort)ProviderPallete.Count);
            if(!DataPallete.ContainsKey(data))
                DataPallete.Add(data, (ushort)DataPallete.Count);
        }

        //Writing the provider pallete.
        foreach(var i in ProviderPallete){
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
            temp1.AddRange(GetBytesBE(ProviderPallete[i.Val]));
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

            //Arbitrary data and provider. Both are pallete entries.
            temp1.AddRange(GetBytesBE(ProviderPallete[i.GetProviderIdentifier()]));
            temp1.AddRange(GetBytesBE(DataPallete[i.ArbitraryData]));
        }
        ProcessTemp(temp1);
        
        return current.ToArray();
    }
    public byte[] ArbitraryData{get => []; set{}}

    public string GetProviderIdentifier() => ProviderInstance.GetIdentifier();
    internal static IInstanceProvider<IChunkCodec> ProviderInstance = new Provider();

    //When this codec becomes obsolete (And it will, it's just a matter of time) please remember to implement GetIdentifier to the same as here, irregardless of the type's full name.
    private sealed class Provider : IInstanceProvider<IChunkCodec>{
        public IChunkCodec ProvideInstance(object Args) =>
            new DefaultChunkCodecV1();
    }
}