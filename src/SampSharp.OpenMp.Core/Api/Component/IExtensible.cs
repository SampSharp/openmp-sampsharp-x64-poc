namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi]
public readonly partial struct IExtensible
{
    // workaround for the fact that the SDK doesn't expose the miscExtensions field
    // ref: https://github.com/openmultiplayer/open.mp-sdk/issues/44
    [OpenMpApiOverload("_workaround")]
    public partial IExtension GetExtension(UID id);
    
    private partial bool AddExtension(IExtension extension, bool autoDeleteExt);

    [OpenMpApiOverload("_uid")]
    [OpenMpApiFunction("removeExtension")]
    private partial bool RemoveExtensionInternal(UID id);
    
    [OpenMpApiFunction("removeExtension")]
    private partial bool RemoveExtensionInternal(IExtension extension);
    
    public void RemoveExtension(UID id)
    {
        if (!RemoveExtensionInternal(id))
        {
            throw new ArgumentException($"Failed to remove extension with id '{id}'.", nameof(id));
        }
    }

    public void RemoveExtension(IExtension extension)
    {
        if (!RemoveExtensionInternal(extension))
        {
            throw new ArgumentException("Failed to remove extension", nameof(extension));
        }
    }

    public void RemoveExtension<T>(T extension) where T : Extension
    {
        RemoveExtension(new IExtension(extension.GetUnmanaged()));
    }

    public void AddExtension<T>(T extension) where T : Extension
    {
        var unmanaged = new IExtension(extension.GetUnmanaged());
        try
        {
            if (!AddExtension(unmanaged, true))
            {
                throw new ArgumentException("Failed to add extension", nameof(extension));
            }
        }
        catch
        {
            extension.Dispose();
            throw;
        }
    }
    
    public T GetExtension<T>() where T : Extension
    {
        var result = TryGetExtension<T>();

        if (result == null)
        {
            throw new InvalidOperationException($"Extension of type '{typeof(T).Name}' not found.");
        }

        return result;
    }

    public T? TryGetExtension<T>() where T : Extension
    {
        
        var ext = GetExtension(ExtensionIdProvider<T>.Id);

        if (ext.Handle == nint.Zero)
        {
            return null;
        }


        return Extension.Get(ext) as T;
    }

    public T QueryExtension<T>() where T : unmanaged, IExtensionInterface
    {
        var extension = GetExtension(T.ExtensionId).Handle;
        return Pointer.AsStruct<T>(extension);
    }
}
