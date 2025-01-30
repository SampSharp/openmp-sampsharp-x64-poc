#include <sdk.hpp>

// IEventDispatcher<void *>
extern "C" SDK_EXPORT bool __CDECL IEventDispatcher_addEventHandler(IEventDispatcher<void *>& dispatcher, void ** handler, event_order_t priority)
{
    return dispatcher.addEventHandler(handler, priority);
}

extern "C" SDK_EXPORT bool __CDECL IEventDispatcher_hasEventHandler(IEventDispatcher<void *>& dispatcher, void ** handler, event_order_t& priority)
{
    return dispatcher.hasEventHandler(handler, priority);
}

extern "C" SDK_EXPORT bool __CDECL IEventDispatcher_removeEventHandler(IEventDispatcher<void *>& dispatcher, void ** handler)
{
    return dispatcher.removeEventHandler(handler);
}

extern "C" SDK_EXPORT size_t __CDECL IEventDispatcher_count(const IEventDispatcher<void *>& dispatcher)
{
    return dispatcher.count();
}
