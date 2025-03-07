﻿using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct ConsoleCommandSenderData
{
    public readonly ConsoleCommandSender Sender;
    private readonly nint _handle;

    public IPlayer? Player => Sender == ConsoleCommandSender.Player ? new IPlayer(_handle) : null;
    public ConsoleMessageHandler? Handler => Sender == ConsoleCommandSender.Custom ? new ConsoleMessageHandler(_handle) : null;
}