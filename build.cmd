
REM We build using MSVC instead of CLANG until this issue has been resolved https://github.com/dotnet/runtime/issues/111788
cmake -S . -B build -G "Visual Studio 18 2026" -A x64 -DCMAKE_POLICY_VERSION_MINIMUM=3.5
cmake --build build --config RelWithDebInfo