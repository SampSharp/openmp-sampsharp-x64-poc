using System.Runtime.InteropServices;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

public abstract partial class Extension : IDisposable
{
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly Action _free;
    private nint? _unmanagedCounterpart;
    private GCHandle? _gcHandle;

    private IExtensible? _appliedTo;

    protected Extension()
    {
        _free = FreeExtension;
        var id = ExtensionIdProvider.GetId(GetType());
        var free = Marshal.GetFunctionPointerForDelegate(_free);
        var handle = GCHandle.Alloc(this, GCHandleType.Normal);

        _gcHandle = handle;
        _unmanagedCounterpart = CreateUnmanaged(id, free, GCHandle.ToIntPtr(handle));
    }
   
    ~Extension()
    {
        Free();
    }
    
    public void Dispose()
    {
        Detach();

        Free();
        GC.SuppressFinalize(this);
    }

    protected virtual void Cleanup()
    {
    }

    internal static Extension? Get(IExtension ext)
    {
        var handle = GetHandle(ext.Handle);
        var gcHandle = GCHandle.FromIntPtr(handle);

        if (!gcHandle.IsAllocated)
        {
            return null;
        }

        return gcHandle.Target as Extension;
    }

    internal nint GetUnmanaged()
    {
        ObjectDisposedException.ThrowIf(!_unmanagedCounterpart.HasValue, GetType());
        
        return _unmanagedCounterpart.Value;
    }
    
    private void Free()
    {
        FreeUnmanagedCounterpart();
        FreeHandle();
    }

    private void FreeUnmanagedCounterpart()
    {
        if (_unmanagedCounterpart.HasValue)
        {
            Delete(_unmanagedCounterpart.Value);
            _unmanagedCounterpart = null;
            _appliedTo = null;
        }
    }

    private void FreeHandle()
    {
        if (_gcHandle.HasValue)
        {
            _gcHandle.Value.Free();
            _gcHandle = null;
        }
    }
    
    private void Detach()
    {
        if (_appliedTo.HasValue && _unmanagedCounterpart.HasValue)
        {
            _appliedTo.Value.RemoveExtension(new IExtension(_unmanagedCounterpart.Value));
            _appliedTo = null;
        }
    }

    internal void ApplyTo(IExtensible extensible)
    {
        if (_appliedTo.HasValue)
        {
            throw new InvalidOperationException("Can only apply to one extensible");
        }

        _appliedTo = extensible;
    }

    private void FreeExtension()
    {
        try
        {
            _appliedTo = null;
            Cleanup();
        }
        catch(Exception e)
        {
            // TODO: error handling
            Console.WriteLine(e);
        }
        finally
        {
            FreeHandle();
        }
    }

    [LibraryImport("SampSharp", EntryPoint = "ManagedExtensionImpl_create")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial nint CreateUnmanaged(UID id, nint freePointer, nint handle);

    [LibraryImport("SampSharp", EntryPoint = "ManagedExtensionImpl_delete")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void Delete(nint handle);
    
    [LibraryImport("SampSharp", EntryPoint = "ManagedExtensionImpl_getHandle")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial nint GetHandle(nint handle);
}