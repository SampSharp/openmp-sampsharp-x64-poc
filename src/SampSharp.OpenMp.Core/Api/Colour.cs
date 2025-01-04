﻿using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Colour(byte r, byte g, byte b, byte a)
{
    public readonly byte R = r;
    public readonly byte G = g;
    public readonly byte B = b;
    public readonly byte A = a;
}