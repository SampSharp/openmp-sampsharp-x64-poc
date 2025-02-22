namespace SampSharp.Entities;

/// <summary>Provides helper methods for event data.</summary>
public static class EventHelper
{
    /// <summary>
    /// Gets a value indicating whether the specified <paramref name="eventResponse" /> indicates the event ran successfully. A neutral <c>null</c> response
    /// is not considered a success response.
    /// </summary>
    /// <param name="eventResponse">The event response to check.</param>
    /// <returns><c>true</c> if the specified response indicates success; otherwise, <c>false</c>.</returns>
    public static bool IsSuccessResponse(object eventResponse)
    {
        return eventResponse is not (null or false or 0 or Task<bool> {IsCompleted: true, Result: false} or Task<int>
        {
            IsCompleted: true, Result: 0
        });
    }
}