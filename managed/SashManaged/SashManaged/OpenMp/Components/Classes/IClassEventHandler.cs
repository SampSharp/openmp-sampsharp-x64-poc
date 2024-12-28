namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public partial interface IClassEventHandler
{
    bool OnPlayerRequestClass(IPlayer player, uint classId);
}