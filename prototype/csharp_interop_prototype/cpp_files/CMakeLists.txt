cmake_minimum_required(VERSION 3.29)

project(Logicraft)

include(Utils)
include(GenerateExportHeader)

create_target_directory_variable(
        "${CMAKE_CURRENT_SOURCE_DIR}/src"
        "${CMAKE_CURRENT_SOURCE_DIR}/include"
)

add_library_with_namespace(Prototype InteropCPP SHARED)

target_include_directories(InteropCPP
        PRIVATE ${INCLUDE_DIR}
        PRIVATE ${CMAKE_CURRENT_BINARY_DIR}
)

set(SRC_FILES
        "${SRC_DIR}/math_api.cpp"
        "${SRC_DIR}/math_type.cpp"
        "${SRC_DIR}/game_object.cpp"
        "${SRC_DIR}/game_object_holder.cpp"
        "${SRC_DIR}/component.cpp"
)

set(INCLUDE_FILES
        "${INCLUDE_DIR}/math_api.h"
        "${INCLUDE_DIR}/math_type.h"
        "${INCLUDE_DIR}/game_object.h"
        "${INCLUDE_DIR}/game_object_holder.h"
        "${INCLUDE_DIR}/component.h"
)

target_sources(InteropCPP PRIVATE ${SRC_FILES} ${INCLUDE_FILES})

target_compile_features(InteropCPP PRIVATE cxx_std_20)

if (CMAKE_CXX_COMPILER_ID STREQUAL "MSVC")
    set_target_properties(InteropCPP PROPERTIES MSVC_RUNTIME_LIBRARY "MultiThreaded$<$<CONFIG:Debug>:Debug>")
else ()
    target_link_options(InteropCPP PRIVATE -static-libstdc++)
endif ()

generate_export_header(InteropCPP)

make_available_as_native_artefact(InteropCPP)