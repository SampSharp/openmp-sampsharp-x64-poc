#include <sdk.hpp>

#include <Server/Components/Actors/actors.hpp>
#include <Server/Components/Checkpoints/checkpoints.hpp>
#include <Server/Components/Classes/classes.hpp>
#include <Server/Components/Console/console.hpp>
#include <Server/Components/Dialogs/dialogs.hpp>
#include <Server/Components/Fixes/fixes.hpp>
#include <Server/Components/GangZones/gangzones.hpp>
#include <Server/Components/Menus/menus.hpp>
#include <Server/Components/Objects/objects.hpp>
#include <Server/Components/Pickups/pickups.hpp>
#include <Server/Components/Recordings/recordings.hpp>
#include <Server/Components/TextDraws/textdraws.hpp>
#include <Server/Components/TextLabels/textlabels.hpp>
#include <Server/Components/Vehicles/vehicles.hpp>
#include <Server/Components/CustomModels/custommodels.hpp>

// macros
//
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

#define PROXYn(type, ret, method, ...); extern "C" SDK_EXPORT ret __CDECL type##_##method(type * __x, _EXPAND_PARAM(__VA_ARGS__)) { return __x -> method (_EXPAND_ARG(__VA_ARGS__)); }
#define PROXY0(type, ret, method); extern "C" SDK_EXPORT ret __CDECL type##_##method(type * __x) { return __x -> method (); }

// test
//
PROXYn(IPlayer, void, allowTeleport, bool);
PROXYn(IPlayer, void, setCameraLookAt, Vector3, int);
PROXYn(IPlayer, void, applyAnimation, AnimationData&, PlayerAnimationSyncType);

PROXY0(ICore, SemanticVersion, getVersion);
PROXY0(ICore, int, getNetworkBitStreamVersion);
PROXY0(ICore, IPlayerPool&, getPlayers);
PROXY0(ICore, IConfig&, getConfig);
PROXYn(ICore, void, setData, SettableCoreDataType, StringView);

//
// https://github.com/openmultiplayer/open.mp-sdk/tree/master/include/Server/Components
//

// Actors
PROXYn(IActor, void, setSkin, int);
PROXY0(IActor, int, getSkin);
PROXYn(IActor, void, applyAnimation, AnimationData&);
PROXY0(IActor, const AnimationData&, getAnimation);
PROXY0(IActor, void, clearAnimations);
PROXYn(IActor, void, setHealth, float);
PROXY0(IActor, float, getHealth);
PROXYn(IActor, void, setInvulnerable, bool);
PROXY0(IActor, bool, isInvulnerable);
PROXYn(IActor, bool, isStreamedInForPlayer, IPlayer&);
PROXYn(IActor, void, streamInForPlayer, IPlayer&);
PROXYn(IActor, void, streamOutForPlayer, IPlayer&);
PROXY0(IActor, const ActorSpawnData&, getSpawnData);

// todo: getEventDispatcher
PROXYn(IActorsComponent, IActor*, create, int, Vector3, float);

// Checkpoints
PROXY0(ICheckpointDataBase, Vector3, getPosition);
PROXYn(ICheckpointDataBase, void, setPosition, Vector3&);
PROXY0(ICheckpointDataBase, float, getRadius);
PROXYn(ICheckpointDataBase, void, setRadius, float);
PROXY0(ICheckpointDataBase, bool, isPlayerInside);
PROXYn(ICheckpointDataBase, void, setPlayerInside, bool);
PROXY0(ICheckpointDataBase, void, enable);
PROXY0(ICheckpointDataBase, void, disable);
PROXY0(ICheckpointDataBase, bool, isEnabled);

PROXY0(IRaceCheckpointData, RaceCheckpointType, getType);
PROXYn(IRaceCheckpointData, void, setType, RaceCheckpointType);
PROXY0(IRaceCheckpointData, Vector3, getNextPosition);
PROXYn(IRaceCheckpointData, void, setNextPosition, Vector3&);

PROXY0(IPlayerCheckpointData, IRaceCheckpointData&, getRaceCheckpoint);
PROXY0(IPlayerCheckpointData, ICheckpointData&, getCheckpoint);

// todo: getEventDispatcher

// Classes
PROXY0(IClass, const PlayerClass&, getClass);
PROXYn(IClass, void, setClass, PlayerClass&);

PROXYn(IClassesComponent, IClass*, create, int, int, Vector3, float, WeaponSlots&);

// todo: getEventDispatcher

// Console
// @todo

// CustomModels
// @todo

// Databases
// @skip

// Dialogs
// @todo

// Fixes
// @todo

// GangZones
// @todo

// LegacyConfig
// @skip

// Menus
// @todo

// Objects
// @todo

// Pawn
// @skip

// Pickups
// @todo

// Recordings
// @todo

// TextLabels
// @todo
	
//Timers
// @skip
	
// Unicode
// @skip
	
// Variables
// @skip
	
// Vehicles
// @todo

//
// https://github.com/openmultiplayer/open.mp-sdk/tree/master/include
// @todo
//