using System.Drawing;
using AnthillProject.Core.Entities;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Math;

namespace AnthillProject.Core.Client;
//TODO: This doesn't work on Linux yet! Currently my number 1 priority to change that.
public class DefaultRenderer : IRenderer{
    protected virtual Bitmap GetTileTexture(Tile tile) =>
        Utility.ResizeImage(
            tile.GetTexture(),
            GetTileTextureSize(),
            GetTileTextureSize()
        );
    
    protected virtual Bitmap GetEntityTexture(Entity entity){
        var size = entity.GetSize();
        return Utility.ResizeImage(entity.GetTexture(), 
            (int)(size.x * GetTileTextureSize()),
            (int)(size.y * GetTileTextureSize())
        );
    }
    
    protected virtual byte GetImageHeightInTiles() => //This defines how many tiles by how many tiles the image will be.
        8;
    protected virtual byte GetImageLengthInTiles() => //This defines how many tiles by how many tiles the image will be.
        8;
    protected virtual int GetTileTextureSize() => //Tile textures will get resized to whatever this returns.
        64;
    protected virtual int CanvasHeight => GetTileTextureSize() * GetImageHeightInTiles(); //GetRenderedBitmap will not necessarily return an image with these dimensions!
    protected virtual int CanvasLength => GetTileTextureSize() * GetImageLengthInTiles(); //GetRenderedBitmap will not necessarily return an image with these dimensions!

    protected virtual float InvertYPosition(float i) =>
        CanvasHeight - i - GetTileTextureSize();
    


    public virtual Bitmap GetRenderedBitmap(){
        var Canvas = new Bitmap(CanvasLength, CanvasHeight);

        ChunkProfile Profile;
        
        Profile = new ChunkProfile(Program.Instance.CurrentEntity, GetImageLengthInTiles(), GetImageHeightInTiles());

        using var graphics = Graphics.FromImage(Canvas);
        
        float RelativizeX() => MathF.Floor(Utility.GetDecimals(Program.Instance.CurrentEntity.Position.x) * GetTileTextureSize());
        float RelativizeY() => MathF.Floor(Utility.GetDecimals(Program.Instance.CurrentEntity.Position.y) * GetTileTextureSize());
        
        //Tile rendering.
        for(byte x = 0; x < GetImageLengthInTiles(); x++)
            for(byte y = 0; y < GetImageHeightInTiles(); y++){
                graphics.DrawImage(GetTileTexture(Profile.GetTile(new Coordinates(x, y))),
                                    (x * GetTileTextureSize() - RelativizeX()),
                    InvertYPosition((y * GetTileTextureSize() - RelativizeY()))
                );
            }

        //Entity rendering.
        foreach(var i in Profile.LoadedEntities){
            if(i == null)
                continue;

            var size = i.GetSize();

            //Checking if the entity's size is too small.
            if(size.x <= 0 || size.y <= 0)
                continue;
            
            graphics.DrawImage(GetEntityTexture(i),
                                GetTileTextureSize() * (i.Position.x - (size.x / 2)) - RelativizeX()                        ,
                InvertYPosition(GetTileTextureSize() * (i.Position.y + (size.y / 2)) - RelativizeY()) + GetTileTextureSize()
            );
        }

        graphics.DrawString(Program.Instance.CurrentEntity.Position.ToString(), new Font("Tahoma",8), Brushes.Black, 64, 64);

        return Canvas;
    }

    public virtual void Init(){}
}