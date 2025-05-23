﻿namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPlayerCustomModelsData" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerCustomModelsData
{
    /// <inheritdoc />
    public static UID ExtensionId => new(0xD3E2F572B38FB3F2);

    public partial uint GetCustomSkin();
    public partial void SetCustomSkin(uint skinModel);
    public partial bool SendDownloadUrl(string url);
}