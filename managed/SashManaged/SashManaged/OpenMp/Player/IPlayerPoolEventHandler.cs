namespace SashManaged.OpenMp;

[OpenMpEventHandler2(NativeTypeName = "PoolEventHandler")]
public interface IPlayerPoolEventHandler : IPoolEventHandler<IPlayer>, IEventHandler2;