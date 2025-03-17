using System.Reflection;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities;

public record SampSharpEnvironment(Assembly EntryAssembly, ICore Core, IComponentList Components);