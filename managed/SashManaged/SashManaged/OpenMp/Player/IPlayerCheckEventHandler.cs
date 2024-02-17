namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IPlayerCheckEventHandler
{
    void OnClientCheckResponse(IPlayer player, int actionType, int address, int results);
}