namespace SashManaged.OpenMp;

public enum RaceCheckpointType
{
    NORMAL = 0, // Must have nextPosition, else it shows as RACE_FINISH
    FINISH, // Must have no nextPosition, else it shows as RACE_NORMAL
    NOTHING,
    AIR_NORMAL,
    AIR_FINISH,
    AIR_ONE,
    AIR_TWO,
    AIR_THREE,
    AIR_FOUR,
    NONE,
};