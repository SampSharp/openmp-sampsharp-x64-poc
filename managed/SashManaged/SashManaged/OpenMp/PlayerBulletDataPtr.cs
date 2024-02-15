using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct PlayerBulletDataPtr
{
    private readonly PlayerBulletData* _data;

    public ref PlayerBulletData Value => ref *_data;
}