cmake_minimum_required(VERSION 3.29)

project(Logicraft)

include(GoogleTest)

add_executable_with_namespace(Prototype UnitTest)

target_sources(UnitTest PRIVATE "${CMAKE_CURRENT_SOURCE_DIR}/unit_tests.cpp")

target_compile_features(UnitTest PRIVATE cxx_std_20)

get_property(prototype_libraries GLOBAL PROPERTY PROTOTYPE_LIBRARIES)

target_link_libraries(UnitTest
        PRIVATE
            GTest::gtest_main
            ${prototype_libraries}
)

gtest_discover_tests(UnitTest)