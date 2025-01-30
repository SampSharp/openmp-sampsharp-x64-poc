namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler(NativeTypeName = "PoolEventHandler")]
public partial interface IPlayerPoolEventHandler : IPoolEventHandler<IPlayer>;