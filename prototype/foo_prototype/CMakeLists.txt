cmake_minimum_required(VERSION 3.29)

project(Logicraft)

include(Utils)
include(PrototypeUtils)

create_target_directory_variable("${CMAKE_CURRENT_SOURCE_DIR}/src" "${CMAKE_CURRENT_SOURCE_DIR}/include")

add_library_with_namespace(Prototype Foo STATIC)

target_sources(Foo
        PRIVATE
            "${SRC_DIR}/foo.cpp"
            "${INCLUDE_DIR}/foo.h"
)

target_include_directories(Foo
        PUBLIC
            "${INCLUDE_DIR}"
)

register_prototype_library(Prototype::Foo)