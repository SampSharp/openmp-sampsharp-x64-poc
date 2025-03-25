using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IExtensible), typeof(IIDProvider))]
public readonly partial struct ITextDrawBase
{
    private partial void GetPosition(out Vector2 position);

    public Vector2 GetPosition()
    {
        GetPosition(out var result);
        return result;
    }

    public partial ref ITextDrawBase SetPosition(Vector2 position);
    public partial void SetText(string text);
    public partial string GetText();
    public partial ref ITextDrawBase SetLetterSize(Vector2 size);
    private partial void GetLetterSize(out Vector2 size);

    public Vector2 GetLetterSize()
    {
        GetLetterSize(out var result);
        return result;
    }

    public partial ref ITextDrawBase SetTextSize(Vector2 size);
    private partial void GetTextSize(out Vector2 size);

    public Vector2 GetTextSize()
    {
        GetTextSize(out var result);
        return result;
    }

    public partial ref ITextDrawBase SetAlignment(TextDrawAlignmentTypes alignment);
    public partial TextDrawAlignmentTypes GetAlignment();
    public partial ref ITextDrawBase SetColour(Colour colour);
    private partial void GetLetterColour(out Colour colour);

    public Colour GetLetterColour()
    {
        GetLetterColour(out var result);
        return result;
    }

    public partial ref ITextDrawBase UseBox(bool use);
    public partial bool HasBox();
    public partial ref ITextDrawBase SetBoxColour(Colour colour);
    public partial void GetBoxColour(out Colour colour);

    public Colour GetBoxColour()
    {
        GetBoxColour(out var result);
        return result;
    }

    public partial ref ITextDrawBase SetShadow(int shadow);
    public partial int GetShadow();
    public partial ref ITextDrawBase SetOutline(int outline);
    public partial int GetOutline();
    public partial ref ITextDrawBase SetBackgroundColour(Colour colour);
    private partial void GetBackgroundColour(out Colour colour);

    public Colour GetBackgroundColour()
    {
        GetBackgroundColour(out var result);
        return result;
    }

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
    private partial void GetPreviewVehicleColour(out Pair<int, int> result);

    public (int, int) GetPreviewVehicleColour()
    {
        GetPreviewVehicleColour(out var result);
        return result;
    }

    public partial ref ITextDrawBase SetPreviewZoom(float zoom);
    public partial float GetPreviewZoom();
    public partial void Restream();
}