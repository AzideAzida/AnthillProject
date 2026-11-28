using AnthillProject.Core.Entities;
using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Math;
using NAudio.Wave;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;
using SkiaSharp;

namespace AnthillProject.Core.Client;
public class SilkDotNetLocalClient : IClient{
    #region Rendering
        private Vector2D<int> DrawingSize;
        protected virtual int Width  => DrawingSize.X;
        protected virtual int Height => DrawingSize.Y;
        protected virtual void Render(){
            GrContext.ResetContext();

            {
                var size = window.GetFullSize();
                DrawingSize = new Vector2D<int>(
                    System.Math.Clamp(size.X, 32, OriginalWindowSize),
                    System.Math.Clamp(size.Y, 32, OriginalWindowSize)
                );
            }

            float InvertYPosition(float i) => Height - i - GetTileTextureHeight();


            void Draw(SKBitmap img, float x, float y){
                Canvas.DrawBitmap(img, x, y, SKSamplingOptions.Default);
            }

            float RelativizeX() => MathF.Floor(Utility.GetDecimals(Program.Instance.CurrentEntity.Position.x) * GetTileTextureWidth () /*- (WinSize.X / 2)*/);
            float RelativizeY() => MathF.Floor(Utility.GetDecimals(Program.Instance.CurrentEntity.Position.y) * GetTileTextureHeight() /*- (WinSize.Y / 2)*/);

            Canvas.Clear(SKColors.Aqua);

            //Tile rendering.
            for(byte x = 0; x < GetImageWidthInTiles(); x++)
                for(byte y = 0; y < GetImageHeightInTiles(); y++){
                    Draw(GetTileTexture(Profile.GetTile(new Coordinates(x, y))),
                                        x * GetTileTextureWidth () - RelativizeX(),
                        InvertYPosition(y * GetTileTextureHeight() - RelativizeY())
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

                Draw(GetEntityTexture(i),
                                    GetTileTextureWidth () * (i.Position.x - (size.x / 2)) - RelativizeX()                        ,
                    InvertYPosition(GetTileTextureHeight() * (i.Position.y + (size.y / 2)) - RelativizeY()) + GetTileTextureHeight()
                );
            }

            Canvas.Flush();
        }
        protected virtual SKBitmap GetTileTexture(ITile tile) =>
        Utility.ResizeImage(
            tile.GetTexture(),
            GetTileTextureWidth(),
            GetTileTextureHeight()
        );
    
        protected virtual SKBitmap GetEntityTexture(IEntity entity){
            var size = entity.GetSize();
            return Utility.ResizeImage(entity.GetTexture(), 
                (int)(size.x * GetTileTextureWidth ()),
                (int)(size.y * GetTileTextureHeight())
            );
        }

        protected virtual byte GetImageWidthInTiles() => //This defines how many tiles by how many tiles the image will be.
            8;
        protected virtual byte GetImageHeightInTiles() => //This defines how many tiles by how many tiles the image will be.
            8;
        protected virtual int GetTileTextureWidth() => //Tile textures will get resized to whatever this returns.
            Width  / GetImageWidthInTiles();
        protected virtual int GetTileTextureHeight() =>
            Height / GetImageHeightInTiles();

        protected ChunkProfile Profile;

        protected SKCanvas Canvas;
        protected GRContext GrContext;
    #endregion

    #pragma warning disable CS8618
    public SilkDotNetLocalClient(){
        OnExit              = (    ) => {};
        OnKeyboardKeyDown   = (i, e) => {};
        OnKeyboardKeyUp     = (i, e) => {};
    }
    #pragma warning restore CS8618

    protected virtual int OriginalWindowSize => 512;
    protected IWindow window;

    #region Interface implementations
        public event Action OnExit;
        
        public event Action<IKeyboard, Key> OnKeyboardKeyDown;
        public event Action<IKeyboard, Key> OnKeyboardKeyUp;

        public event Action<IMouse, MouseButton> OnMouseDown;
        public event Action<IMouse, MouseButton> OnMouseUp;
        public event Action<IMouse, MouseButton, System.Numerics.Vector2> OnMouseClick;
        public event Action<IMouse, MouseButton, System.Numerics.Vector2> OnMouseDoubleClick;
        public event Action<IMouse, System.Numerics.Vector2> OnMouseMove;
        public event Action<IMouse, ScrollWheel> OnMouseScroll;

        public void Init(){
            Thread.Sleep(1000);
            Program.Instance.Server.OnServerTickFinish += () => {
                Profile = new ChunkProfile(Program.Instance.CurrentEntity, GetImageWidthInTiles(), GetImageHeightInTiles());
            };
            //Based of off https://raw.githubusercontent.com/dotnet/Silk.NET/main/src/Lab/Experiments/SilkBackedSkiaRendering/Program.cs
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(OriginalWindowSize, OriginalWindowSize);
            options.Title = "AnthillProject Client";
            options.PreferredStencilBufferBits = 8;
            options.PreferredBitDepth = new Vector4D<int>(8, 8, 8, 8);
            GlfwWindowing.Use();
            window = Window.Create(options);
            window.Load += () => {
                IInputContext input = window.CreateInput();
                
                foreach(var i in input.Keyboards){
                    i.KeyDown       += (a1, a2, a3) => OnKeyboardKeyDown.Invoke(a1, a2)    ;
                    i.KeyUp         += (a1, a2, a3) => OnKeyboardKeyUp  .Invoke(a1, a2)    ;
                }
    
                foreach(var i in input.Mice){
                    i.MouseDown     += OnMouseDown          ;
                    i.MouseUp       += OnMouseUp            ;
                    i.Click         += OnMouseClick         ;
                    i.DoubleClick   += OnMouseDoubleClick   ;
                    i.MouseMove     += OnMouseMove          ;
                    i.Scroll        += OnMouseScroll        ;
                }
            };
            window.Render += d => Render();

            window.Initialize();
            using var grGlInterface = GRGlInterface.Create(name => window.GLContext!.TryGetProcAddress(name, out var addr) ? addr : 0);
            grGlInterface.Validate();
            GrContext = GRContext.CreateGl(grGlInterface);
            var renderTarget = new GRBackendRenderTarget(OriginalWindowSize, OriginalWindowSize, 0, 8, new GRGlFramebufferInfo(0, 0x8058));
            using var surface = SKSurface.Create(GrContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            Canvas = surface.Canvas;

            window.Run();

            //Since Window.Run is a blocking method, the following code won't be executed until the window closes.

            OnExit.Invoke();

            Canvas.Dispose();
            GrContext.Dispose();

            //The following code is to make sure that the server thread stops before this one does.
            var ServerIsntFinished = true;
            Program.Instance.Server.OnServerFinish += () =>
                ServerIsntFinished = false;
            while(ServerIsntFinished);
        }
        public void PlayAudioFromIWaveProvider(IWaveProvider i){
            #if WINDOWS
                var player = new WasapiPlayerBuilder().Build();
                player.Init(i);
                player.Play();
            #endif
        }
    #endregion
}
