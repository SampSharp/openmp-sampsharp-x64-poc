using System.Runtime.InteropServices.Marshalling;

namespace SashManaged.OpenMp;

[NativeMarshalling(typeof(ObjectMaterialDataMarshaller))]
public record ObjectMaterialData(
    int Model,
    byte MaterialSize,
    byte FontSize,
    byte Alignment,
    bool Bold,
    Colour MaterialColour,
    Colour BackgroundColour,
    string Text,
    string Font,
    MaterialType Type,
    bool Used)
{
    public Colour FontColour => MaterialColour;
    public string Txd => Text;
    public string Texture => Font;
}