//
// Created by charles on 08/11/2025.
//

#include "game_object.h"

#include <iostream>

game_object::game_object(std::string_view _name)
    : m_name(_name)
{}

 game_object::game_object(game_object&& _other) noexcept
: m_name(std::move(_other.m_name)), m_components(std::move(_other.m_components))
{
    std::unique_lock other_lock(_other.m_game_object_mtx);
}

game_object& game_object::operator=(game_object&& _other) noexcept
{
    std::unique_lock other_lock(_other.m_game_object_mtx);
    std::unique_lock lock(m_game_object_mtx);

    if (this == &_other)
    {
        return *this;
    }

    m_name = std::move(_other.m_name);
    m_components = std::move(_other.m_components);

    return *this;
}

void game_object::start()
{
    std::shared_lock lock(m_game_object_mtx);

    for (uptr_component_t& component: m_components)
    {
        component->start();
    }
}

void game_object::update()
{
    std::shared_lock lock(m_game_object_mtx);

    for (uptr_component_t& component: m_components)
    {
        component->update();
    }
}

component& game_object::add_component(void* _gc_handle)
{
    std::unique_lock lock(m_game_object_mtx);

    auto new_components{std::make_unique<component>(_gc_handle)};
    return *m_components.emplace_back(std::move(new_components));
}

std::string_view game_object::get_name() const
{
    std::shared_lock lock(m_game_object_mtx);
    return m_name;
}

const std::vector<game_object::uptr_component_t>& game_object::get_components() const
{
    std::shared_lock lock(m_game_object_mtx);
    return m_components;
}

void* add_component(void* _internal_game_object, void* _gc_handle)
{
    auto* casted_internal_gm{static_cast<game_object*>(_internal_game_object)};
    component& new_component{casted_internal_gm->add_component(_gc_handle)};
    new_component.start();

    return &new_component;
}