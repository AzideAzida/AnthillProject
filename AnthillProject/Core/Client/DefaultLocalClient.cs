#pragma warning disable CS8602 //IF IT'S NULL, THEN SO BE IT!

/*/ <=== Add a single asterisk between the slashes to disable the symbol.
#define DoBasicRenderReporting
/**/

using Eto.Drawing;
using Eto.Forms;
using NAudio.Wave;

namespace AnthillProject.Core.Client;
public class DefaultLocalClient : IClient{
    public static Application? App;
    public DefaultLocalClient(){
        OnExit = () => {};
    }
    public event Action OnExit;
    #region Interface implementations
        public void OnServerTick(){
            #if DoBasicRenderReporting
                Console.WriteLine("start");
            #endif

            //If rendering is too slow, the UI thread won't be able to keep up with the server thread and rendering tasks will build up. Too bad!
            App.InvokeAsync(delegate{
                try{
                    Img.Image = Utility.SystemImageToEtoBitmap(Renderer.GetRenderedBitmap());
                }
                catch(Exception ex){
                    Console.WriteLine("Exception ocurred in rendering.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
                #if DoBasicRenderReporting
                    Console.WriteLine("end");
                #endif
            });
        }
        public void Init(){
            App = new Application(Eto.Platforms.Wpf);

            Renderer.Init();
            Img = new ImageView();

            form = new Form{
                ClientSize = new Eto.Drawing.Size(600, 400),
                Title = "AnthillProject Client",
                Content = Img,
                BackgroundColor = Color.FromArgb(40, 40, 40)
            };

            App.Run(form);

            //Since Application.Run is a blocking method, this event won't be raised until the app closes.
            OnExit.Invoke();
        }
        public void OnKeyDown(EventHandler<KeyEventArgs> i){
            form.KeyDown += i;
        }
        public void OnMouseDown(EventHandler<MouseEventArgs> i){//TODO: Fix this. It don't work.
            form.MouseDown += i;
        }
        public void PlayAudioFromIWaveProvider(IWaveProvider i){
            var player = new WasapiPlayerBuilder().Build();
            player.Init(i);
            player.Play();
        }
    #endregion

    public IRenderer Renderer = new DefaultRenderer();
    
    
    private Form? form;
    private ImageView? Img;
}
