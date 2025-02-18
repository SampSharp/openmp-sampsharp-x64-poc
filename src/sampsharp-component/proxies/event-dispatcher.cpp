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

// IIndexedEventDispatcher<void *>
extern "C" SDK_EXPORT bool __CDECL IIndexedEventDispatcher_addEventHandler(IIndexedEventDispatcher<void *>& dispatcher, void ** handler, size_t index, event_order_t priority)
{
    return dispatcher.addEventHandler(handler, index, priority);
}

extern "C" SDK_EXPORT bool __CDECL IIndexedEventDispatcher_hasEventHandler(IIndexedEventDispatcher<void *>& dispatcher, void ** handler, size_t index, event_order_t& priority)
{
    return dispatcher.hasEventHandler(handler, index, priority);
}

extern "C" SDK_EXPORT bool __CDECL IIndexedEventDispatcher_removeEventHandler(IIndexedEventDispatcher<void *>& dispatcher, void ** handler, size_t index)
{
    return dispatcher.removeEventHandler(handler, index);
}

extern "C" SDK_EXPORT size_t __CDECL  IIndexedEventDispatcher_count(const IIndexedEventDispatcher<void *>& dispatcher)
{
    return dispatcher.count();
}

extern "C" SDK_EXPORT size_t __CDECL  IIndexedEventDispatcher_count_index(const IIndexedEventDispatcher<void *>& dispatcher, size_t index)
{
    return dispatcher.count(index);
}
