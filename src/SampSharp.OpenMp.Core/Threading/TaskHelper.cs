namespace SampSharp.OpenMp.Core;

/// <summary>Provides helper methods for dealing with tasks.</summary>
public static class TaskHelper
{
    /// <summary>Returns a task which, when awaited, will switch the continuation to the main thread.</summary>
    /// <returns>A task to </returns>
    public static SyncToMainThreadTask SwitchToMainThread()
    {
        return new SyncToMainThreadTask();
    }

    /// <summary>
    /// Gets a value indicating whether the current thread is the main thread.
    /// </summary>
    /// <returns></returns>
    public static bool IsMainThread()
    {
        return SynchronizationContextExtension.Active.IsMainThread();
    }
}