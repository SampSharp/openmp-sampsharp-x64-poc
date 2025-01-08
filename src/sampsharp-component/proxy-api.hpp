#pragma once

//
// macros for definition of exported proxy functions
//

// expand variadic args as a numbered parameter list. e.g. _EXPAND_PARAM(a, b, X, Y) -> aXb _2, aYb _1
#define _EXPAND_PARAM(prefix,postfix,...) _EXPAND_PARAM_N(__VA_ARGS__,14,13,12,11,10,9,8,7,6,5,4,3,2,1,0)(prefix,postfix,__VA_ARGS__)
#define _EXPAND_PARAM_N(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, n, ...) _EXPAND_PARAM ## n
#define _EXPAND_PARAM1(prefix, postfix, type, ...) prefix##type##postfix _1
#define _EXPAND_PARAM2(prefix, postfix, type, ...) prefix##type##postfix _2, _EXPAND_PARAM1(prefix, postfix, __VA_ARGS__)
#define _EXPAND_PARAM3(prefix, postfix, type, ...) prefix##type##postfix _3, _EXPAND_PARAM2(prefix, postfix, __VA_ARGS__)
#define _EXPAND_PARAM4(prefix, postfix, type, ...) prefix##type##postfix _4, _EXPAND_PARAM3(prefix, postfix, __VA_ARGS__)
#define _EXPAND_PARAM5(prefix, postfix, type, ...) prefix##type##postfix _5, _EXPAND_PARAM4(prefix, postfix, __VA_ARGS__)
#define _EXPAND_PARAM6(prefix, postfix, type, ...) prefix##type##postfix _6, _EXPAND_PARAM5(prefix, postfix, __VA_ARGS__)
#define _EXPAND_PARAM7(prefix, postfix, type, ...) prefix##type##postfix _7, _EXPAND_PARAM6(prefix, postfix, __VA_ARGS__)
#define _EXPAND_PARAM8(prefix, postfix, type, ...) prefix##type##postfix _8, _EXPAND_PARAM7(prefix, postfix, __VA_ARGS__)
#define _EXPAND_PARAM9(prefix, postfix, type, ...) prefix##type##postfix _9, _EXPAND_PARAM8(prefix, postfix, __VA_ARGS__)
#define _EXPAND_PARAM10(prefix, postfix, type, ...) prefix##type##postfix _10, _EXPAND_PARAM10(prefix, postfix, __VA_ARGS__)
#define _EXPAND_PARAM11(prefix, postfix, type, ...) prefix##type##postfix _11, _EXPAND_PARAM11(prefix, postfix, __VA_ARGS__)
#define _EXPAND_PARAM12(prefix, postfix, type, ...) prefix##type##postfix _12, _EXPAND_PARAM12(prefix, postfix, __VA_ARGS__)
#define _EXPAND_PARAM13(prefix, postfix, type, ...) prefix##type##postfix _13, _EXPAND_PARAM13(prefix, postfix, __VA_ARGS__)
#define _EXPAND_PARAM14(prefix, postfix, type, ...) prefix##type##postfix _14, _EXPAND_PARAM14(prefix, postfix, __VA_ARGS__)

// expand variadic args as a numbered argument list. e.g. _EXPAND_ARG(,X, Y) -> _2, _1
#define _EXPAND_ARG(prefix, ...) _EXPAND_ARG_N(__VA_ARGS__,14,13,12,11,10,9,8,7,6,5,4,3,2,1,0)(prefix,__VA_ARGS__)
#define _EXPAND_ARG_N(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, n, ...) _EXPAND_ARG ## n
#define _EXPAND_ARG1(prefix,type, ...) prefix _1
#define _EXPAND_ARG2(prefix,type, ...) prefix _2, _EXPAND_ARG1(prefix,__VA_ARGS__)
#define _EXPAND_ARG3(prefix,type, ...) prefix _3, _EXPAND_ARG2(prefix,__VA_ARGS__)
#define _EXPAND_ARG4(prefix,type, ...) prefix _4, _EXPAND_ARG3(prefix,__VA_ARGS__)
#define _EXPAND_ARG5(prefix,type, ...) prefix _5, _EXPAND_ARG4(prefix,__VA_ARGS__)
#define _EXPAND_ARG6(prefix,type, ...) prefix _6, _EXPAND_ARG5(prefix,__VA_ARGS__)
#define _EXPAND_ARG7(prefix,type, ...) prefix _7, _EXPAND_ARG6(prefix,__VA_ARGS__)
#define _EXPAND_ARG8(prefix,type, ...) prefix _8, _EXPAND_ARG7(prefix,__VA_ARGS__)
#define _EXPAND_ARG9(prefix,type, ...) prefix _9, _EXPAND_ARG8(prefix,__VA_ARGS__)
#define _EXPAND_ARG10(prefix,type, ...) prefix _10, _EXPAND_ARG9(prefix,__VA_ARGS__)
#define _EXPAND_ARG11(prefix,type, ...) prefix _11, _EXPAND_ARG10(prefix,__VA_ARGS__)
#define _EXPAND_ARG12(prefix,type, ...) prefix _12, _EXPAND_ARG11(prefix,__VA_ARGS__)
#define _EXPAND_ARG13(prefix,type, ...) prefix _13, _EXPAND_ARG12(prefix,__VA_ARGS__)
#define _EXPAND_ARG14(prefix,type, ...) prefix _14, _EXPAND_ARG13(prefix,__VA_ARGS__)

/// expand variadic args as a numbered initializer list. e.g. _EXPAND_INIT(X, Y) -> X_(_2), Y_(_1)
#define _EXPAND_INIT(...) _EXPAND_INIT_N(__VA_ARGS__,14,13,12,11,10,9,8,7,6,5,4,3,2,1,0)(__VA_ARGS__)
#define _EXPAND_INIT_N(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, n, ...) _EXPAND_INIT ## n
#define _EXPAND_INIT1(type, ...) type##_(_1)
#define _EXPAND_INIT2(type, ...) type##_(_2), _EXPAND_INIT1(__VA_ARGS__)
#define _EXPAND_INIT3(type, ...) type##_(_3), _EXPAND_INIT2(__VA_ARGS__)
#define _EXPAND_INIT4(type, ...) type##_(_4), _EXPAND_INIT3(__VA_ARGS__)
#define _EXPAND_INIT5(type, ...) type##_(_5), _EXPAND_INIT4(__VA_ARGS__)
#define _EXPAND_INIT6(type, ...) type##_(_6), _EXPAND_INIT5(__VA_ARGS__)
#define _EXPAND_INIT7(type, ...) type##_(_7), _EXPAND_INIT6(__VA_ARGS__)
#define _EXPAND_INIT8(type, ...) type##_(_8), _EXPAND_INIT7(__VA_ARGS__)
#define _EXPAND_INIT9(type, ...) type##_(_9), _EXPAND_INIT8(__VA_ARGS__)
#define _EXPAND_INIT10(type, ...) type##_(_10), _EXPAND_INIT9(__VA_ARGS__)
#define _EXPAND_INIT11(type, ...) type##_(_11), _EXPAND_INIT10(__VA_ARGS__)
#define _EXPAND_INIT12(type, ...) type##_(_12), _EXPAND_INIT11(__VA_ARGS__)
#define _EXPAND_INIT13(type, ...) type##_(_13), _EXPAND_INIT12(__VA_ARGS__)
#define _EXPAND_INIT14(type, ...) type##_(_14), _EXPAND_INIT13(__VA_ARGS__)

#define __PROXY_IMPL(type_subject, type_return, method, proxy_name, ...) \
    extern "C" SDK_EXPORT type_return __CDECL \
    proxy_name(type_subject * subject __VA_OPT__(, _EXPAND_PARAM(,,__VA_ARGS__))) \
    { \
        return subject -> method ( \
            __VA_OPT__(_EXPAND_ARG(,__VA_ARGS__)) \
        ); \
    }

#define __PROXY_IMPL_RESULT_PTR(type_subject, type_return, method, proxy_name, ...) \
    extern "C" SDK_EXPORT void __CDECL \
    proxy_name(type_subject * subject __VA_OPT__(, _EXPAND_PARAM(,,__VA_ARGS__)), type_return * result) \
    { \
        *result = subject -> method ( \
            __VA_OPT__(_EXPAND_ARG(,__VA_ARGS__)) \
        ); \
    }

/// proxy function macro. e.g. PROXY(subj, int, foo, bool) -> int subj_foo(subj * x, bool _1) { return x->foo(_1); }
#define PROXY(type_subject, type_return, method, ...) __PROXY_IMPL(type_subject, type_return, method, type_subject##_##method, __VA_ARGS__)

/// proxy function macro. e.g. PROXY_RESULT_PTR(subj, int, foo, bool) -> void subj_foo(subj * x, bool _1, int * result) { *result = x->foo(_1); }
#define PROXY_PTR(type_subject, type_return, method, ...) __PROXY_IMPL_RESULT_PTR(type_subject, type_return, method, type_subject##_##method, __VA_ARGS__)

/// proxy function macro for an overload. output is similar to PROXY macro, except the function name is post-fixed by overload argument
#define PROXY_OVERLOAD(type_subject, type_return, method, overload, ...) __PROXY_IMPL(type_subject, type_return, method, type_subject##_##method##overload, __VA_ARGS__)

/// proxy function macro for an overload. output is similar to PROXY_PTR macro, except the function name is post-fixed by overload argument
#define PROXY_OVERLOAD_PTR(type_subject, type_return, method, overload, ...) __PROXY_IMPL_RESULT_PTR(type_subject, type_return, method, type_subject##_##method##overload, __VA_ARGS__)

#define __PROXY_EVENT_DISPATCHER_IMPL(handler_name, handler_type) \
    __PROXY_IMPL(IEventDispatcher<handler_type>, bool, addEventHandler, IEventDispatcher_##handler_name##_addEventHandler, handler_type *, event_order_t); \
    __PROXY_IMPL(IEventDispatcher<handler_type>, bool, removeEventHandler, IEventDispatcher_##handler_name##_removeEventHandler, handler_type *); \
    __PROXY_IMPL(IEventDispatcher<handler_type>, bool, hasEventHandler, IEventDispatcher_##handler_name##_hasEventHandler, handler_type *, event_order_t); \
    __PROXY_IMPL(IEventDispatcher<handler_type>, size_t, count, IEventDispatcher_##handler_name##_count);

/// proxy for event dispatcher functions and function to get the event dispatcher
#define PROXY_EVENT_DISPATCHER(type_subject, type_handler, method) \
	PROXY(type_subject, IEventDispatcher<type_handler>&, method); \
	__PROXY_EVENT_DISPATCHER_IMPL(type_handler, type_handler)

#define PROXY_EVENT_DISPATCHER_TYPE(type_subject, handler_type, handler_name, method) \
	PROXY(type_subject, IEventDispatcher<handler_type>&, method); \
	__PROXY_EVENT_DISPATCHER_IMPL(handler_name, handler_type)

#define __PROXY_INDEXED_EVENT_DISPATCHER_IMPL(handler_name, handler_type) \
    __PROXY_IMPL(IIndexedEventDispatcher<handler_type>, bool, addEventHandler, IIndexedEventDispatcher_##handler_name##_addEventHandler, handler_type *, size_t, event_order_t); \
    __PROXY_IMPL(IIndexedEventDispatcher<handler_type>, bool, removeEventHandler, IIndexedEventDispatcher_##handler_name##_removeEventHandler, handler_type *, size_t); \
    __PROXY_IMPL(IIndexedEventDispatcher<handler_type>, bool, hasEventHandler, IIndexedEventDispatcher_##handler_name##_hasEventHandler, handler_type *, size_t, event_order_t); \
    __PROXY_IMPL(IIndexedEventDispatcher<handler_type>, size_t, count, IIndexedEventDispatcher_##handler_name##_count_index, size_t); \
    __PROXY_IMPL(IIndexedEventDispatcher<handler_type>, size_t, count, IIndexedEventDispatcher_##handler_name##_count);

/// proxy for event dispatcher functions and function to get the event dispatcher
#define PROXY_INDEXED_EVENT_DISPATCHER(type_subject, type_handler, method) \
	PROXY(type_subject, IIndexedEventDispatcher<type_handler>&, method); \
	__PROXY_INDEXED_EVENT_DISPATCHER_IMPL(type_handler, type_handler)

/// start of event handler proxy class 
#define PROXY_EVENT_HANDLER_BEGIN(handler_type) \
    class handler_type##Impl final : handler_type {

/// end of event handler proxy class + functions for creating/destroying proxy
#define PROXY_EVENT_HANDLER_END(handler_type, ...) \
    public: \
        handler_type##Impl(_EXPAND_ARG(void**, __VA_ARGS__)) : \
            _EXPAND_INIT(__VA_ARGS__) { } \
    }; \
    extern "C" SDK_EXPORT handler_type##Impl* __CDECL handler_type##Impl_create(_EXPAND_ARG(void**, __VA_ARGS__)) \
    { \
        return new handler_type##Impl(_EXPAND_ARG(,__VA_ARGS__)); \
    } \
    extern "C" SDK_EXPORT void __CDECL handler_type##Impl_delete(handler_type##Impl* handler) \
    { \
        delete handler; \
    }

/// event handler function in event handler proxy class
#define PROXY_EVENT_HANDLER_EVENT(type_return, name, ...) \
    private: \
    typedef type_return(CORECLR_DELEGATE_CALLTYPE * name##_fn)(_EXPAND_PARAM(, , __VA_ARGS__)); \
    void** name##_ = nullptr; \
    public: \
    type_return name(_EXPAND_PARAM(, , __VA_ARGS__)) override \
    { \
        return ((name##_fn)name##_)(_EXPAND_ARG(,__VA_ARGS__)); \
    }
