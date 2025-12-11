# Create an executable and create a alias with the designed NameSpace.
function(add_executable_with_namespace NAMESPACE EXECUTABLE)
    add_executable(${EXECUTABLE})
    add_executable(${NAMESPACE}::${EXECUTABLE} ALIAS ${EXECUTABLE})
endfunction()

# Create a library and create a alias with the designed NameSpace.
function(add_library_with_namespace NAMESPACE EXECUTABLE TYPE)
    add_library(${EXECUTABLE} ${TYPE})
    add_library(${NAMESPACE}::${EXECUTABLE} ALIAS ${EXECUTABLE})
endfunction()

function(create_target_directory_variable SRC_PATH INCLUDE_PATH)
    set(SRC_DIR ${SRC_PATH} PARENT_SCOPE)
    set(INCLUDE_DIR ${INCLUDE_PATH} PARENT_SCOPE)
endfunction()

function(copy_target_dll TARGET_DLL DIRECTORY)
    add_custom_command(TARGET ${TARGET_DLL} POST_BUILD
            COMMAND ${CMAKE_COMMAND} -E make_directory "${DIRECTORY}"
            COMMAND ${CMAKE_COMMAND} -E copy_if_different $<TARGET_FILE:${TARGET_DLL}> "${DIRECTORY}/"
    )
endfunction()

function(make_available_as_native_artefact TARGET)
    set(DIRECTORY ${ARTEFACTS_DIR}/native/${CMAKE_BUILD_TYPE}/${TARGET})
    add_custom_command(TARGET ${TARGET} POST_BUILD
            COMMAND ${CMAKE_COMMAND} -E make_directory ${DIRECTORY}
            COMMAND ${CMAKE_COMMAND} -E copy_if_different $<TARGET_FILE:${TARGET}> "${DIRECTORY}/"
    )
endfunction()

function(download_package)
    set(OPTIONS)
    set(ONE_VALUE_KEYWORD NAME GIT_LINK GIT_VERSION MAKE_IT_AVAILABLE)
    set(MULTI_VALUE_KEYWORD PACKAGE_OPTIONS)
    cmake_parse_arguments(DP "${OPTIONS}" "${ONE_VALUE_KEYWORD}" "${MULTI_VALUE_KEYWORD}" ${ARGN})

    set(PKG_MAX_OPTIONS 4)
    list(LENGTH DP_PACKAGE_OPTIONS PKG_OPTIONS_LENGTH)
    math(EXPR PKG_OPTIONS_MODULO "${PKG_OPTIONS_LENGTH} % ${PKG_MAX_OPTIONS}")

    if (NOT PKG_OPTIONS_MODULO EQUAL 0)
        message(FATAL_ERROR "Package given options arguments are insufficient.")
        return()
    endif ()

    FetchContent_Declare(
            ${DP_NAME}
            GIT_REPOSITORY ${DP_GIT_LINK}
            GIT_TAG ${DP_GIT_VERSION}
    )

    set(INDEX 0)
    while (INDEX LESS PKG_OPTIONS_LENGTH)
        list(GET DP_PACKAGE_OPTIONS ${INDEX} OPTION_TYPE)
        math(EXPR INDEX "${INDEX} + 1")

        list(GET DP_PACKAGE_OPTIONS ${INDEX} OPTION_NAME)
        math(EXPR INDEX "${INDEX} + 1")

        list(GET DP_PACKAGE_OPTIONS ${INDEX} OPTION_VALUE)
        math(EXPR INDEX "${INDEX} + 1")

        list(GET DP_PACKAGE_OPTIONS ${INDEX} OPTION_DESCRIPTION)
        math(EXPR INDEX "${INDEX} + 1")

        set(${OPTION_NAME} ${OPTION_VALUE} CACHE ${OPTION_TYPE} "${OPTION_DESCRIPTION}")
    endwhile ()

    if (DP_MAKE_IT_AVAILABLE)
        FetchContent_MakeAvailable(${DP_NAME})
    endif ()
endfunction()

#function(download_package NAME REPO_LINK REPO_VERSION MAKE_IT_AVAILABLE)
#    FetchContent_Declare(
#            ${NAME}
#            GIT_REPOSITORY ${REPO_LINK}
#            GIT_TAG ${REPO_VERSION}
#    )
#
#    set(OPTIONS_ARGC 4)
#
#    if (ARGC GREATER OPTIONS_ARGC)
#        list(SUBLIST ARGV 4 -1 PACKAGE_OPTIONS)
#        math(EXPR PACKAGE_OPTIONS_ARGC "${ARGC} - ${OPTIONS_ARGC}")
#        math(EXPR PACKAGE_OPTIONS_MODULO "${PACKAGE_OPTIONS_ARGC} % ${OPTIONS_ARGC}")
#
#        if (PACKAGE_OPTIONS_ARGC LESS 4 OR NOT PACKAGE_OPTIONS_MODULO EQUAL 0)
#            message(FATAL_ERROR "Package given arguments are insufficient.")
#        endif ()
#
#        set(INDEX 0)
#        while (INDEX LESS PACKAGE_OPTIONS_ARGC)
#            list(GET PACKAGE_OPTIONS ${INDEX} OPTION_TYPE)
#            math(EXPR INDEX "${INDEX} + 1")
#
#            list(GET PACKAGE_OPTIONS ${INDEX} OPTION_NAME)
#            math(EXPR INDEX "${INDEX} + 1")
#
#            list(GET PACKAGE_OPTIONS ${INDEX} OPTION_VALUE)
#            math(EXPR INDEX "${INDEX} + 1")
#
#            list(GET PACKAGE_OPTIONS ${INDEX} OPTION_DESCRIPTION)
#            math(EXPR INDEX "${INDEX} + 1")
#
#            set(${OPTION_NAME} ${OPTION_VALUE} CACHE ${OPTION_TYPE} "${OPTION_DESCRIPTION}")
#        endwhile ()
#    endif ()
#
#    if (MAKE_IT_AVAILABLE)
#        FetchContent_MakeAvailable(googletest)
#    endif ()
#endfunction()