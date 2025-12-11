function(add_csharp_target TARGET_DIRECTORY TARGET_NAME)
    set(OPTIONS GENERATE_RUN_TARGET)
    set(ONE_VALUE_KEYWORD)
    set(MULTI_VALUE_KEYWORD SHARED_LIBRARY_DEPS)
    cmake_parse_arguments(ACT "${OPTIONS}" "${ONE_VALUE_KEYWORD}" "${MULTI_VALUE_KEYWORD}" ${ARGN})

    add_custom_target(${TARGET_NAME}_build ALL
            COMMAND dotnet
            build "${TARGET_NAME}.csproj"
            -c ${CMAKE_BUILD_TYPE}
            WORKING_DIRECTORY ${TARGET_DIRECTORY}
            DEPENDS ${ACT_SHARED_LIBRARY_DEPS}
    )

    if (ACT_GENERATE_RUN_TARGET)
        add_custom_target(${TARGET_NAME}_run
                COMMAND dotnet run
                --project "${TARGET_NAME}.csproj"
                -c ${CMAKE_BUILD_TYPE}
                --no-build
                WORKING_DIRECTORY ${TARGET_DIRECTORY}
                DEPENDS ${TARGET_NAME}_build ${ACT_SHARED_LIBRARY_DEPS}
        )
    endif ()
endfunction()