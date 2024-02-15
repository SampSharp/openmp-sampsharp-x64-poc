using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct AnimationData
{
    public readonly float delta; //< The speed to play the animation
    public readonly bool loop; //< If set to 1, the animation will loop. If set to 0, the animation will play once
    public readonly bool lockX; //< If set to 0, the player is returned to their old X coordinate once the animation is complete (for animations that move the player such as walking). 1 will not return them to their old position
    public readonly bool lockY; //< Same as above but for the Y axis. Should be kept the same as the previous parameter
    public readonly bool freeze; //< Setting this to 1 will freeze the player at the end of the animation. 0 will not
    public readonly uint time; //< Timer in milliseconds. For a never-ending loop it should be 0
    public readonly HybridString16 lib; //< The animation library of the animation to apply
    public readonly HybridString24 name; //< The name of the animation to apply

}