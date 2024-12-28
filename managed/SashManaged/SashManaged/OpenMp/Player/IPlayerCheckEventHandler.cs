namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public partial interface IPlayerCheckEventHandler
{
    void OnClientCheckResponse(IPlayer player, int actionType, int address, int results);
}