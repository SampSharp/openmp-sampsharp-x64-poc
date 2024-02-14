namespace SashManaged.OpenMp;

public enum PlayerBulletHitType : byte
{
    PlayerBulletHitType_None,
    PlayerBulletHitType_Player = 1,
    PlayerBulletHitType_Vehicle = 2,
    PlayerBulletHitType_Object = 3,
    PlayerBulletHitType_PlayerObject = 4,
}