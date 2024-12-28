namespace SashManaged.OpenMp;

[OpenMpEventHandler(NativeTypeName = "PoolEventHandler")]
public partial interface IPlayerPoolEventHandler : IPoolEventHandler<IPlayer>;