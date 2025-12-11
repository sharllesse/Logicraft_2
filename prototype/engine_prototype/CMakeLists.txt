cmake_minimum_required(VERSION 3.29)

project(Logicraft)

include(Utils)
include(CSharpUtils)

add_subdirectory(engine_core)

add_csharp_target(
        "${CMAKE_CURRENT_SOURCE_DIR}/engine_runtime"
        EngineRuntime
        SHARED_LIBRARY_DEPS EngineCore
)

add_csharp_target(
        "${CMAKE_CURRENT_SOURCE_DIR}/editor"
        Editor
)

add_csharp_target(
        "${CMAKE_CURRENT_SOURCE_DIR}/game_script"
        GameScript
)