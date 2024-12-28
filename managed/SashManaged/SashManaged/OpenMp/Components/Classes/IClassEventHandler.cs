namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IClassEventHandler : IEventHandler2
{
    bool OnPlayerRequestClass(IPlayer player, uint classId);
}