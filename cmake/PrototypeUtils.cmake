# Put a library name in the PROTOTYPE_LIBRARIES list which is used for the prototype unit tests executable to link the libraries.
# The PROTOTYPE_LIBRARIES list could have other use.
function(register_prototype_library NAME)
    get_property(prototype_libraries GLOBAL PROPERTY PROTOTYPE_LIBRARIES)
    list(APPEND prototype_libraries ${NAME})
    set_property(GLOBAL PROPERTY PROTOTYPE_LIBRARIES ${prototype_libraries})
endfunction()