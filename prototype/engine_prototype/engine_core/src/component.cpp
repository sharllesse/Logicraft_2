//
// Created by charles on 08/11/2025.
//

#include "component.h"

//For now nothing can with the multithreading. So it does not need any mutex.

component::component(void *_gc_handle)
    : m_gc_handle(_gc_handle)
{}

component::component(component&& _other) noexcept
    : m_gc_handle(_other.m_gc_handle)
{
    _other.m_gc_handle = nullptr;
}

component::~component()
{
    destroy();
}

component& component::operator=(component&& _other) noexcept
{
    if (this == &_other)
    {
        return *this;
    }

    m_gc_handle = _other.m_gc_handle;
    _other.m_gc_handle = nullptr;
    return *this;
}

void component::start()
{
    if (g_start_ptr != nullptr)
    {
        g_start_ptr(m_gc_handle);
    }
}

void component::update()
{
    if (g_update_ptr != nullptr)
    {
        g_update_ptr(m_gc_handle);
    }
}

void component::destroy()
{
    if (g_destroy_ptr != nullptr)
    {
        g_destroy_ptr(m_gc_handle);
    }
}

void set_global_component_callbacks(start_ptr_t _start_ptr, update_ptr_t _update_ptr, destroy_ptr_t _destroy_ptr)
{
    g_start_ptr = _start_ptr;
    g_update_ptr = _update_ptr;
    g_destroy_ptr = _destroy_ptr;
}
