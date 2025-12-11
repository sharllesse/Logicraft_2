cmake_minimum_required(VERSION 3.29)

project(Logicraft)

include(Utils)
include(FetchContent)

set(FETCHCONTENT_BASE_DIR "${CMAKE_CURRENT_SOURCE_DIR}/fetch_content")

if (BUILD_TEST)
    message(STATUS "Build test enable.")

    find_package(GoogleTest 1.17.0 QUIET)

    if (NOT GoogleTest_FOUND)
        message(STATUS "GoogleTest not found using find_package.")
        message(STATUS "Using FetchContent to declare it.")

        download_package(
                NAME googletest
                GIT_LINK https://github.com/google/googletest.git
                GIT_VERSION v1.17.0
                PACKAGE_OPTIONS
                BOOL gtest_force_shared_crt ON "None"
                BOOL gtest_build_tests OFF "Turning off gtest tests building."
                BOOL gtest_build_samples OFF "Turning off gtest samples building."
                BOOL gmock_build_tests OFF "Turning off gmock tests building."
                MAKE_IT_AVAILABLE TRUE
        )
    endif ()

    enable_testing()
endif ()
