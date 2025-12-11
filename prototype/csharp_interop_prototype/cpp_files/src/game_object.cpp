//
// Created by charles on 08/11/2025.
//

#include "game_object.h"

#include <ranges>
#include <algorithm>
#include <iostream>

game_object::game_object(std::string_view _name)
    : m_name(_name)
{}

game_object::~game_object()
{
    std::cout << "game object destroyed\n";
}

void game_object::start()
{
    for (sptr_component_t& component: m_components)
    {
        component->start();
    }
}

void game_object::update()
{
    for (sptr_component_t& component: m_components)
    {
        component->update();
    }
}

component& game_object::add_component(void* _gc_handle)
{
    auto new_components{std::make_shared<component>(_gc_handle)};
    m_components.emplace_back(new_components);
    return *new_components;
}

void* add_component(void* _internal_game_object, void* _gc_handle)
{
    const auto casted_internal_gm{static_cast<game_object*>(_internal_game_object)};
    component& new_component{casted_internal_gm->add_component(_gc_handle)};
    new_component.start();

    return &new_component;
}