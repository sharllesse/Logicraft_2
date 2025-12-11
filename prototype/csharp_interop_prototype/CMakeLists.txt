cmake_minimum_required(VERSION 3.29)

project(Logicraft)

include(CSharpUtils)

add_subdirectory(cpp_files)

add_csharp_target(
        "${CMAKE_CURRENT_SOURCE_DIR}/csharp_files"
        InteropTesting
        SHARED_LIBRARY_DEP InteropCPP
)