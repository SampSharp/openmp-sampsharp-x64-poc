#include <sdk.hpp>

#define _EXPAND_PARAM0(...)
#define _EXPAND_PARAM1(type, ...) type _1
#define _EXPAND_PARAM2(type, ...) type _2, _EXPAND_PARAM1(__VA_ARGS__)
#define _EXPAND_PARAM3(type, ...) type _3, _EXPAND_PARAM2(__VA_ARGS__)
#define _EXPAND_PARAM4(type, ...) type _4, _EXPAND_PARAM3(__VA_ARGS__)
#define _EXPAND_PARAM5(type, ...) type _5, _EXPAND_PARAM4(__VA_ARGS__)
#define _EXPAND_PARAM6(type, ...) type _6, _EXPAND_PARAM5(__VA_ARGS__)
#define _EXPAND_PARAM7(type, ...) type _7, _EXPAND_PARAM6(__VA_ARGS__)
#define _EXPAND_PARAM8(type, ...) type _8, _EXPAND_PARAM7(__VA_ARGS__)

#define _EXPAND_PARAM_N(_1, _2, _3, _4, _5, _6, _7, _8, _9, n, ...) _EXPAND_PARAM ## n
#define _EXPAND_PARAM(...) _EXPAND_PARAM_N(__VA_ARGS__,9,8,7,6,5,4,3,2,1,0)(__VA_ARGS__)

#define _EXPAND_ARG0(...)
#define _EXPAND_ARG1(type, ...) _1
#define _EXPAND_ARG2(type, ...) _2, _EXPAND_ARG1(__VA_ARGS__)
#define _EXPAND_ARG3(type, ...) _3, _EXPAND_ARG2(__VA_ARGS__)
#define _EXPAND_ARG4(type, ...) _4, _EXPAND_ARG3(__VA_ARGS__)
#define _EXPAND_ARG5(type, ...) _5, _EXPAND_ARG4(__VA_ARGS__)
#define _EXPAND_ARG6(type, ...) _6, _EXPAND_ARG5(__VA_ARGS__)
#define _EXPAND_ARG7(type, ...) _7, _EXPAND_ARG6(__VA_ARGS__)
#define _EXPAND_ARG8(type, ...) _8, _EXPAND_ARG7(__VA_ARGS__)

#define _EXPAND_ARG_N(_1, _2, _3, _4, _5, _6, _7, _8, _9, n, ...) _EXPAND_ARG ## n
#define _EXPAND_ARG(...) _EXPAND_ARG_N(__VA_ARGS__,9,8,7,6,5,4,3,2,1,0)(__VA_ARGS__)

#define CPP_PROXY(type, ret, method, ...); extern "C" SDK_EXPORT ret __CDECL type##_##method(type * __x, _EXPAND_PARAM(__VA_ARGS__)) { return __x -> method (_EXPAND_ARG(__VA_ARGS__)); }
#define CPP_PROXY0(type, ret, method); extern "C" SDK_EXPORT ret __CDECL type##_##method(type * __x) { return __x -> method (); }

CPP_PROXY(IPlayer, void, allowTeleport, bool);
CPP_PROXY(IPlayer, void, setCameraLookAt, Vector3, int);
CPP_PROXY(IPlayer, void, applyAnimation, AnimationData&, PlayerAnimationSyncType);

CPP_PROXY0(ICore, SemanticVersion, getVersion);
CPP_PROXY0(ICore, int, getNetworkBitStreamVersion);
CPP_PROXY0(ICore, IPlayerPool&, getPlayers);
CPP_PROXY0(ICore, IConfig&, getConfig);
CPP_PROXY(ICore, void, setData, SettableCoreDataType, StringView);
