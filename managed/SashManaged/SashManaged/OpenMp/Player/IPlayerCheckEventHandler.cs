namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IPlayerCheckEventHandler : IEventHandler2
{
    void OnClientCheckResponse(IPlayer player, int actionType, int address, int results);
}