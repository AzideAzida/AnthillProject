using System.Drawing;
using AnthillProject.Core.Interfaces;

namespace AnthillProject.Core.Client;
public interface IRenderer : IInitializable{
    //All rendering should be done here.
    Bitmap GetRenderedBitmap();
}
