namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IPlayerChangeEventHandler
{
    void OnPlayerScoreChange(IPlayer player, int score);
    void OnPlayerNameChange(IPlayer player, StringView oldName);
    void OnPlayerInteriorChange(IPlayer player, uint newInterior, uint oldInterior);
    void OnPlayerStateChange(IPlayer player, PlayerState newState, PlayerState oldState);
    void OnPlayerKeyStateChange(IPlayer player, uint newKeys, uint oldKeys);
}