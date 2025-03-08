# openmp-sampsharp-x64-poc

proof-of-concept code. Attempt to run x64 .NET code with an up-to-date well supported runtime as an open.mp component.

Goal
----

The goal of this project is to create a replacement for SA-MP-based SampSharp libraries that can be easily used to
generate a bridge between .NET and the open.mp component/data structure.

Status
------

See open issues: https://github.com/SampSharp/openmp-sampsharp-x64-poc/issues

Contents  
--------  

The project consists of several parts:  
1) The open.mp component written in c++ which hosts the dotnet runtime (x64) and provides C declarations for all C++
functions using very simple proxy functions. The component also contains some small C++ class implementations required
for registering event handlers which can be created and deleted using declared C functions. open.mp uses the robin hood
library for hash tables which are exposed in the open.mp API. Therefore we also provide proxy functions for a subset of
functions exposed in its API. The component is currently fully functional on Windows, but lacks Linux support.  
2) The source generator generates various structures. The source generator generates the following code structures:  
   - Implementations of event handler infrastructure (registration/unregistration) using the `[OpenMpEventHandler]`
   attribute  
   - P/Invokes + marshalling of functions exposed in the open.mp api using the `[OpenMpApi]` attribute  
   - An `Initialize` entry point for the open.mp to invoke. The entry point should have a fixed namespace, class and
   method name and should wire up the communication between the open.mp component and the .NET portion of SampSharp. It
   should also generate a `Main` entry point. This entry point should either be empty or provide a console message that
   the game mode should be launched through open.mp. The `Main` entry point is required in order for .NET to write a
   runtimeconfig which we need to run the gamemode.
3) The analyzer provides useful diagnostics for helping writing proper SampSharp code that interfaces with open.mp. It
   should provide warnings/errors when the written code cannot be marshalled or no P/Invoke can be generated.  
4) The SampSharp.OpenMp.Core provides the open.mp API and data structures in .NET. These APIs are usable but not as
   user-friendly as we'd like to see in SampSharp.  
5) Lastly we need a user-friendly library which allows the user to easily write game modes in .NET. It will be based on
   the ECS library SampSharp.Entities. Since the ECS framework seems more flexible and performant due to the way events
   propagate I'll not be rewriting SampSharp.GameMode for open.mp. Maintaining two versions of SampSharp greatly
   increases the amount of effort required from maintainers. When the open.mp version of SampSharp is ready, the old
   SampSharp will go into maintenance mode and only receive critical bug fixes.  

Benefits  
--------  

There are a number of key differences between the libraries in this repo and the libraries in the main
SampSharp repo:
- We use the open.mp API instead of the the Pawn scripting API for communicating between .NET and the
SA-MP server backend. Aside from the big performance benefit this change provides, we also gain an increased flexibility
due to getting access to the entire server API instead of the subset exposed to Pawn.  
- We build x64 binaries instead of x86. While open.mp does not yet ship their x64 server binaries from the releases
page, daily builds are available for x64
(https://github.com/openmultiplayer/open.mp/actions/workflows/build.yml?query=branch%3Amaster). .NET does however not
provide x86 binaries for Linux (they do for Windows). While 3rd party x86 .NET binaries are available, these are not
supported by MS and may have stability issues.  
- The bridge between the .NET API and server API are generated at compile-time instead of at run-time. This means that
the initial performance of the server should be better, it easier to diagnose issues in generated code and easier to
improve the generated code since we generate c# code instead of intermediate  code (IL).  

Building the project
--------------------

### Requirements

On Windows, the following software is required to build the project
- [Install CMake 3.19+](https://cmake.org/download/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/en-us/vs/)
  - Install the Development for desktop with .NET
  - Install the Development for desktop with C++
- [Open.MP Server x64](https://github.com/openmultiplayer/open.mp/actions?query=branch%3Amaster)
  - Find and click on the last sucessful run in `master` branch
  - Scroll to the bottom of the page and download the `open.mp-win-x64-v*` file for Windows or `open.mp-linux-x86_64-v*` for Linux

### Instructions

- Clone the repository including all submodules: `git clone https://github.com/SampSharp/openmp-sampsharp-x64-poc
--recursive` 
- **NOTE*** If you forgot to clone the submodules (directories in `external/sdk` empty), you can do it after cloning the
  repository: `git submodule update --init --recursive` 

#### open.mp component (Windows)

```
./build.cmd
```

After building, copy the built library (`build/artifacts/SampSharp.dll`) to the components directory of your open.mp server

#### open.mp component (Linux)
```
./build.sh
```

After building, copy the built library (`build/artifacts/libSampSharp.so`) to the components directory of your open.mp server

#### .NET libraries (any platform)
```
dotnet build SampSharp.sln
```

#### .NET libraries (run/develop on Windows)
  - Open the `<root>/SampSharp.sln` file with the Visual Studio
  - Launch a `TestMode.*` project
  - The application will ask whether you'd like SampSharp to create a `launchSettings.json` file for you. Press `y` and enter the path to your open.mp server
  - Run the `open.mp` profile from Visual Studio
