cmake_minimum_required(VERSION 3.29)

project(Logicraft
        LANGUAGES
        CXX
        C
        VERSION 1.0.0)

list(APPEND CMAKE_MODULE_PATH "${CMAKE_SOURCE_DIR}/cmake")

option(BUILD_TEST "Build the unit test project" ON)

set(ARTEFACTS_DIR "${CMAKE_SOURCE_DIR}/artefacts" CACHE STRING "Directory containing all the libraries used by the CSharp projects")

add_subdirectory(third_party)
add_subdirectory(prototype)