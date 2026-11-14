using AnthillProject.Core.Interfaces;
using Eto.Forms;
using NAudio.Wave;

namespace AnthillProject.Core.Client;
public interface IClient : IInitializable{
    void PlayAudioFromIWaveProvider(IWaveProvider i);
    void OnKeyDown(EventHandler<KeyEventArgs> i);
    void OnMouseDown(EventHandler<MouseEventArgs> i);

    //This event should be invoked when the client closes.
    event Action OnExit;

    //The server should call this every tick.
    void OnServerTick();
}