using System.Drawing;
using AnthillProject.Core.Math;
using AnthillProject.Core.Entities;
using Eto.Forms;
using AnthillProject.Core;
using AnthillProject.Core.Providing;

namespace AnthillProject.EntityBehaviours;
public class Player : Entity{
    public const float Speed = 1;

    public Player(EntityProviderArgs i) : base(i){}

    public override Vector2 GetSize() =>
        new Vector2(0.25f);

    public override Image GetTexture() =>
        Utility.ArgbBitmap1x1(0, 0, 0);

    public override void Simulate(){
        //entity.Position -= new Vector2(0.01f);
    }
    public override void Init(){
        Program.Instance.Client.OnMouseDown((i, keys) => {
            if(keys.Buttons == MouseButtons.Primary     ) Velocity += new Vector2(0, Speed);
            if(keys.Buttons == MouseButtons.Alternate   ) Velocity += new Vector2(Speed, 0);
        });
    }

    public override string GetProviderIdentifier() => ProviderInstance.GetIdentifier();
    internal static IInstanceProvider<Entity> ProviderInstance = new Provider();
    internal class Provider : IInstanceProvider<Entity>{
        public Entity ProvideInstance(object Args) =>
            new UnknownEntity((EntityProviderArgs)Args);
    }
}