namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface IClassEventHandler
{
    bool OnPlayerRequestClass(IPlayer player, uint classId);
}