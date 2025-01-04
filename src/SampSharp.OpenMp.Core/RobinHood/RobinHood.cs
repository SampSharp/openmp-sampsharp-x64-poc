﻿using System.Runtime.InteropServices;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

internal static class RobinHood
{
    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern FlatPtrHashSetIterator FlatPtrHashSet_begin(nint data);
    
    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern FlatPtrHashSetIterator FlatPtrHashSet_end(nint data);
    
    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern FlatPtrHashSetIterator FlatPtrHashSet_inc(FlatPtrHashSetIterator value);

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern Size FlatPtrHashSet_size(nint data);
    
    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern FlatPtrHashSetIterator FlatHashSetStringView_begin(nint data);
    
    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern FlatPtrHashSetIterator FlatHashSetStringView_end(nint data);
    
    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern FlatPtrHashSetIterator FlatHashSetStringView_inc(FlatPtrHashSetIterator value);

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern Size FlatHashSetStringView_size(nint data);
    
    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern void FlatHashSetStringView_emplace(nint data, StringView value);

}