// SampSharp
// Copyright 2022 Tim Potze
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Diagnostics;
using System.Reflection;
using SampSharp.Entities.Utilities;
using SampSharp.OpenMp.Core;

namespace SampSharp.Entities;

/// <summary>Represents a system which invokes timers on every tick.</summary>
public class TimerSystem : ITickingSystem, ITimerService
{
    private static readonly TimeSpan _lowIntervalThreshold = TimeSpan.FromSeconds(1.0 / 50); // 50Hz

    private readonly IServiceProvider _serviceProvider;

    private readonly List<TimerInfo> _timers = [];
    private long _lastTick;
    private bool _didInitialize;

    /// <summary>Initializes a new instance of the <see cref="TimerSystem" /> class.</summary>
    public TimerSystem(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Stop(TimerReference timer)
    {
        ArgumentNullException.ThrowIfNull(timer);
        timer.Info.IsActive = false;
        _timers.Remove(timer.Info);
    }

    public TimerReference Start(Action<IServiceProvider> action, TimeSpan interval)
    {
        return Start((sp, _) => action(sp), interval);
    }

    public TimerReference Start(Action<IServiceProvider, TimerReference> action, TimeSpan interval)
    {
        ArgumentNullException.ThrowIfNull(action);

        if (!IsValidInterval(interval))
        {
            throw new ArgumentOutOfRangeException(nameof(interval), interval, "The interval should be a nonzero positive value.");
        }

        var invoker = new TimerInfo(intervalTicks: interval.Ticks, nextTick: Stopwatch.GetTimestamp() + interval.Ticks, invoke: null!, true);

        var reference = new TimerReference(invoker, action.Target, action.Method);

        invoker.Invoke = () => action(_serviceProvider, reference);
        invoker.Reference = reference;

        _timers.Add(invoker);

        return reference;
    }

    [Event]
    internal void OnGameModeInit(RuntimeInformation info)
    {
        _lastTick = Stopwatch.GetTimestamp();

        
        CreateTimersFromAssemblies(info.EntryAssembly, _lastTick);

        _didInitialize = true;
    }

    public void Tick()
    {
        if (!_didInitialize || _timers.Count == 0)
        {
            return;
        }

        var timestamp = Stopwatch.GetTimestamp();

        // Don't user foreach for performance reasons
        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = 0; i < _timers.Count; i++)
        {
            var timer = _timers[i];

            while ((timer.NextTick > _lastTick || timestamp < _lastTick) && timer.NextTick <= timestamp)
            {
                try
                {
                    timer.Invoke();
                }
                catch (Exception ex)
                {
                    var context = "timer";

                    if (timer.Reference?.Method != null)
                    {
                        context = $"timer@{timer.Reference.Method.DeclaringType}.{timer.Reference.Method.Name}";
                    }

                    SampSharpExceptionHandler.HandleException(context, ex);
                }

                timer.NextTick += timer.IntervalTicks;
            }
        }

        _lastTick = timestamp;
    }

    internal static bool IsValidInterval(TimeSpan interval)
    {
        return interval >= TimeSpan.FromTicks(1);
    }

    private void CreateTimersFromAssemblies(Assembly entryAssem, long tick)
    {
        // Find methods with TimerAttribute in any ISystem in any assembly.
        var events = new AssemblyScanner()
            .IncludeAssembly(entryAssem)
            .IncludeReferencedAssemblies()
            .IncludeNonPublicMembers()
            .Implements<ISystem>()
            .ScanMethods<TimerAttribute>();

        // Create timer invokers and store timer info in registry.
        foreach (var (method, attribute) in events)
        {
            // TODO: CoreLog.LogDebug("Adding timer on {0}.{1}.", method.DeclaringType, method.Name);

            if (!IsValidInterval(attribute.IntervalTimeSpan))
            {
                // TODO: CoreLog.Log(CoreLogLevel.Error, $"Timer {method} could not be registered the interval {attribute.IntervalTimeSpan} is invalid.");
                continue;
            }

            var service = _serviceProvider.GetService(method.DeclaringType!);

            if (service == null)
            {
                // TODO: CoreLog.Log(CoreLogLevel.Debug, "Skipping timer registration because service could not be loaded.");
                continue;
            }

            var parameterInfos = method.GetParameters()
                .Select(info => new MethodParameterSource(info) { IsService = true })
                .ToArray();

            var compiled = MethodInvokerFactory.Compile(method, parameterInfos);

            if (attribute.IntervalTimeSpan < _lowIntervalThreshold)
            {
                // TODO: CoreLog.Log(CoreLogLevel.Warning, $"Timer {method.DeclaringType}.{method.Name} has a low interval of {attribute.IntervalTimeSpan}.");
            }

            var timer = new TimerInfo(
                intervalTicks: attribute.IntervalTimeSpan.Ticks, 
                nextTick: tick + attribute.IntervalTimeSpan.Ticks, 
                invoke: () => compiled(service, null, _serviceProvider, null), 
                isActive: true);

            timer.Reference = new TimerReference(timer, service, method);

            _timers.Add(timer);
        }
    }
}