cmake_minimum_required(VERSION 3.29)

project(Logicraft)

include(Utils)

add_subdirectory(foo_prototype)
# add_subdirectory(csharp_interop_prototype)
add_subdirectory(engine_prototype)

if (BUILD_TEST)
    add_subdirectory(unit_tests)
endif ()