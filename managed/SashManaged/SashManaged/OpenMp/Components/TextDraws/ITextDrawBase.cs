using System.Numerics;

namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IExtensible), typeof(IIDProvider))]
public readonly partial struct ITextDrawBase
{
    public partial Vector2 GetPosition();
    public partial ref ITextDrawBase SetPosition(Vector2 position);
    public partial void SetText(StringView text);
    public partial StringView GetText();
    public partial ref ITextDrawBase SetLetterSize(Vector2 size);
    public partial Vector2 GetLetterSize();
    public partial ref ITextDrawBase SetTextSize(Vector2 size);
    public partial Vector2 GetTextSize();
    public partial ref ITextDrawBase SetAlignment(TextDrawAlignmentTypes alignment);
    public partial TextDrawAlignmentTypes GetAlignment();
    public partial ref ITextDrawBase SetColour(Colour colour);
    public partial Colour GetLetterColour();
    public partial ref ITextDrawBase UseBox(bool use);
    public partial bool HasBox();
    public partial ref ITextDrawBase SetBoxColour(Colour colour);
    public partial Colour GetBoxColour();
    public partial ref ITextDrawBase SetShadow(int shadow);
    public partial int GetShadow();
    public partial ref ITextDrawBase SetOutline(int outline);
    public partial int GetOutline();
    public partial ref ITextDrawBase SetBackgroundColour(Colour colour);
    public partial Colour GetBackgroundColour();
    public partial ref ITextDrawBase SetStyle(TextDrawStyle style);
    public partial TextDrawStyle GetStyle();
    public partial ref ITextDrawBase SetProportional(bool proportional);
    public partial bool IsProportional();
    public partial ref ITextDrawBase SetSelectable(bool selectable);
    public partial bool IsSelectable();
    public partial ref ITextDrawBase SetPreviewModel(int model);
    public partial int GetPreviewModel();
    public partial ref ITextDrawBase SetPreviewRotation(Vector3 rotation);
    public partial Vector3 GetPreviewRotation();
    public partial ref ITextDrawBase SetPreviewVehicleColour(int colour1, int colour2);
    public partial PairInt GetPreviewVehicleColour();
    public partial ref ITextDrawBase SetPreviewZoom(float zoom);
    public partial float GetPreviewZoom();
    public partial void Restream();
};