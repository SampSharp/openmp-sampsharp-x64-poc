using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IBaseObject"/> interface.
/// </summary>
[OpenMpApi(typeof(IExtensible), typeof(IEntity))]
public readonly partial struct IBaseObject
{
    public partial void SetDrawDistance(float drawDistance);
    public partial float GetDrawDistance();
    public partial void SetModel(int model);
    public partial int GetModel();
    public partial void SetCameraCollision(bool collision);
    public partial bool GetCameraCollision();
    public partial void Move(ref ObjectMoveData data);
    public partial bool IsMoving();
    public partial void Stop();
    public partial ref ObjectMoveData GetMovingData();
    public partial void AttachToVehicle(IVehicle vehicle, Vector3 offset, Vector3 rotation);
    public partial void ResetAttachment();
    public partial ref ObjectAttachmentData GetAttachmentData();
    public partial bool GetMaterialData(uint materialIndex, out ObjectMaterialData? output);
    public partial void SetMaterial(uint materialIndex, int model, string textureLibrary, string textureName, Colour colour);

    public partial void SetMaterialText(uint materialIndex, string text, ObjectMaterialSize materialSize, string fontFace, int fontSize, bool bold, Colour fontColour,
        Colour backgroundColour, ObjectMaterialTextAlign align);
}