//
// Created by charles on 08/11/2025.
//

#ifndef LOGICRAFT_COMPONENT_H
#define LOGICRAFT_COMPONENT_H

#include "interopcpp_export.h"

using update_ptr_t = void(*)(void*);
using start_ptr_t = void(*)(void*);
using destroy_ptr_t = void(*)(void*);

inline start_ptr_t g_start_ptr{nullptr};
inline update_ptr_t g_update_ptr{nullptr};
inline destroy_ptr_t g_destroy_ptr{nullptr};

class component
{
public:
    component() = default;
    explicit component(void* _gc_handle);
    component(const component&) = default;
    component(component&&) noexcept = default;
    ~component();

    component& operator=(const component&) = default;
    component& operator=(component&&) noexcept = default;

    void start();
    void update();

private:
    void* m_gc_handle{nullptr};
};

extern "C"
{
    INTEROPCPP_EXPORT void set_global_component_callbacks(start_ptr_t _start_ptr, update_ptr_t _update_ptr, destroy_ptr_t _destroy_ptr);
}

#endif //LOGICRAFT_COMPONENT_H