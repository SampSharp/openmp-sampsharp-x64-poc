
REM We build using MSVC instead of CLANG until this issue has been resolved https://github.com/dotnet/runtime/issues/111788
cmake -S . -B build -G "Visual Studio 17 2022" -A x64
cmake --build build --config RelWithDebInfo