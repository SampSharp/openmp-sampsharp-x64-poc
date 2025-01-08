cmake_minimum_required(VERSION 3.19)
project(sampsharp LANGUAGES CXX VERSION 0.0.1)

set(CMAKE_CXX_STANDARD 17)

add_subdirectory("../../external/sdk" "${CMAKE_BINARY_DIR}/external/sdk")
include_directories(
    .
    "../../external/dotnet"
)

add_definitions(
    -DHAVE_STDINT_H=1
)

set(PROJECT_NAME "SampSharp")

# can be specified as arugment cmake -DCOMPONENTS_DIR="path/to/x64/server/components"
if(DEFINED COMPONENTS_DIR AND NOT COMPONENTS_DIR STREQUAL "")
    set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY "${COMPONENTS_DIR}")
    set(CMAKE_LIBRARY_OUTPUT_DIRECTORY "${COMPONENTS_DIR}")
    set(CMAKE_RUNTIME_OUTPUT_DIRECTORY "${COMPONENTS_DIR}")
    
    set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY_DEBUG "${COMPONENTS_DIR}")
    set(CMAKE_LIBRARY_OUTPUT_DIRECTORY_DEBUG "${COMPONENTS_DIR}")
    set(CMAKE_RUNTIME_OUTPUT_DIRECTORY_DEBUG "${COMPONENTS_DIR}")
    
    set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY_RELEASE "${COMPONENTS_DIR}")
    set(CMAKE_LIBRARY_OUTPUT_DIRECTORY_RELEASE "${COMPONENTS_DIR}")
    set(CMAKE_RUNTIME_OUTPUT_DIRECTORY_RELEASE "${COMPONENTS_DIR}")
endif()

link_directories("../../external/dotnet")


# attempt to get static lib linking working...
#
# IF (WIN32)
#     set(LIB_NETHOST "libnethost.lib")
# ELSE()
#     set(LIB_NETHOST "libnethost.a")
# ENDIF()
# 
# find_library(nethost
#     NAMES ${LIB_NETHOST}
#     PATHS "../../external/dotnet"
#     NO_DEFAULT_PATH
# )
# 
# if (NOT nethost)
#     message(FATAL_ERROR "Could not find ${LIB_NETHOST}")
# endif()
# 
# message("resolved ${nethost}")

file(GLOB_RECURSE component_source_list "${CMAKE_CURRENT_SOURCE_DIR}/*.cpp" "${CMAKE_CURRENT_SOURCE_DIR}/*.hpp")

add_library(${PROJECT_NAME} SHARED ${component_source_list})

target_link_libraries(${PROJECT_NAME} PRIVATE 
    OMP-SDK
    nethost
)

