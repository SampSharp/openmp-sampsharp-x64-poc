﻿namespace SashManaged.OpenMp;

[OpenMpApi]
public readonly partial struct IConsoleMessageHandler
{
    public partial void HandleConsoleMessage(string message);
}