using System.Numerics;
using AnthillProject.Core.Interfaces;
using NAudio.Wave;
using Silk.NET.Input;

namespace AnthillProject.Core.Client;
public interface IClient : IInitializable{
    void PlayAudioFromIWaveProvider(IWaveProvider i);

    event Action<IKeyboard, Key> OnKeyboardKeyDown;
    event Action<IKeyboard, Key> OnKeyboardKeyUp;

    event Action<IMouse, MouseButton> OnMouseDown;
    event Action<IMouse, MouseButton> OnMouseUp;
    event Action<IMouse, MouseButton, Vector2> OnMouseClick;
    event Action<IMouse, MouseButton, Vector2> OnMouseDoubleClick;
    event Action<IMouse, Vector2> OnMouseMove;
    event Action<IMouse, ScrollWheel> OnMouseScroll;
    

    //This event should be invoked when the client closes.
    event Action OnExit;
}