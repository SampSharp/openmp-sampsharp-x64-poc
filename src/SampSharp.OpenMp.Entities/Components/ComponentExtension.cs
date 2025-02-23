using SampSharp.OpenMp.Core;

namespace SampSharp.Entities;

[Extension(0x53d3b0b0bbb28a7f)]
public class ComponentExtension : Extension
{
    public ComponentExtension(Component component)
    {
        Component = component;
    }

    public Component Component { get; }
    public bool IsOmpEntityDestroyed { get; private set; }

    protected override void Cleanup()
    {
        IsOmpEntityDestroyed = true;

        if (!Component.IsDestroying)
        {
            Component.DestroyEntity();
        }
    }
}