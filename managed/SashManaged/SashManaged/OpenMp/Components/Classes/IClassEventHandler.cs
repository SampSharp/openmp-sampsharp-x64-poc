namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IClassEventHandler
{
    bool OnPlayerRequestClass(IPlayer player, uint classId);
}