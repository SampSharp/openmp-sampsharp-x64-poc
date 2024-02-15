cmake_minimum_required(VERSION 3.19)
project(sampsharp LANGUAGES C CXX VERSION 0.0.1)

set(CMAKE_CXX_STANDARD 17)

add_subdirectory(sdk)

include_directories(
	.
)

add_definitions(
	-DHAVE_STDINT_H=1
)

set(NETHOST_LIB_DIR "dotnet")

set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY "D:/projects/openmp_dev/x64/components")
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY "D:/projects/openmp_dev/x64/components")
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY "D:/projects/openmp_dev/x64/components")

set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY_DEBUG "D:/projects/openmp_dev/x64/components")
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY_DEBUG "D:/projects/openmp_dev/x64/components")
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY_DEBUG "D:/projects/openmp_dev/x64/components")

link_directories(${NETHOST_LIB_DIR})

add_library(${PROJECT_NAME} SHARED
	main.cpp
	managed-host.cpp
	sampsharp-component.cpp
	proxies.cpp
	testing.cpp
)

target_link_libraries(${PROJECT_NAME} PRIVATE
    OMP-SDK
	nethost
)

