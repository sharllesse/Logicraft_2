cmake_minimum_required(VERSION 3.29)

project(Logicraft)

include(Utils)
include(GenerateExportHeader)

create_target_directory_variable("${CMAKE_CURRENT_SOURCE_DIR}/src" "${CMAKE_CURRENT_SOURCE_DIR}/include")

add_library_with_namespace(Prototype EngineCore SHARED)

target_compile_features(EngineCore PRIVATE cxx_std_20)

if (CMAKE_CXX_COMPILER_ID STREQUAL "MSVC")
    set_target_properties(EngineCore PROPERTIES MSVC_RUNTIME_LIBRARY "MultiThreaded$<$<CONFIG:Debug>:Debug>")
else ()
    target_link_options(EngineCore PRIVATE -static-libstdc++)
endif ()

set(SRC_FILES
        "${SRC_DIR}/component.cpp"
        "${SRC_DIR}/game_object.cpp"
        "${SRC_DIR}/game_object_holder.cpp"
)

set(INCLUDE_FILES
        "${INCLUDE_DIR}/component.h"
        "${INCLUDE_DIR}/game_object.h"
        "${INCLUDE_DIR}/game_object_holder.h"
        "${INCLUDE_DIR}/engine.h"
)

target_sources(EngineCore
        PRIVATE ${SRC_FILES}
        PRIVATE ${INCLUDE_FILES}
)

target_include_directories(EngineCore
        PRIVATE ${INCLUDE_DIR}
        PRIVATE ${CMAKE_CURRENT_BINARY_DIR}
)

generate_export_header(EngineCore
        BASE_NAME engine
        EXPORT_FILE_NAME engine_core_export.h
)

make_available_as_native_artefact(EngineCore)