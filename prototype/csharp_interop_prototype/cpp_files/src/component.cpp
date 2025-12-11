//
// Created by charles on 08/11/2025.
//

#include "component.h"

component::component(void *_gc_handle)
    : m_gc_handle(_gc_handle)
{}

component::~component()
{
    if (g_destroy_ptr)
    {
        g_destroy_ptr(m_gc_handle);
    }
}

void component::start()
{
    if (g_start_ptr)
    {
        g_start_ptr(m_gc_handle);
    }
}

void component::update()
{
    if (g_update_ptr)
    {
        g_update_ptr(m_gc_handle);
    }
}

void set_global_component_callbacks(start_ptr_t _start_ptr, update_ptr_t _update_ptr, destroy_ptr_t _destroy_ptr)
{
    g_start_ptr = _start_ptr;
    g_update_ptr = _update_ptr;
    g_destroy_ptr = _destroy_ptr;
}
