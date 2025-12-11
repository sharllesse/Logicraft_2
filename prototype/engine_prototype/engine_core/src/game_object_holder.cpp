//
// Created by charles on 08/11/2025.
//

#include "game_object_holder.h"

#include <iostream>

game_object_holder& game_object_holder::get()
{
    static game_object_holder s_gm_holder{};
    return s_gm_holder;
}

game_object_holder::uptr_game_object_t& game_object_holder::add_game_object(std::string_view _name)
{
    std::unique_lock unique_lock(m_game_objects_mtx);

    auto new_game_object{std::make_unique<game_object>(_name)};
    return m_game_objects.emplace_back(std::move(new_game_object));
}

void game_object_holder::update()
{
    //Could be changed to a unique_lock later if a destroy function is implemented.
    //But for now it's just a way to read game_object.
    std::shared_lock shared_lock(m_game_objects_mtx);

    for (uptr_game_object_t& game_object : m_game_objects)
    {
        game_object->update();
    }
}

void game_object_holder::destroy()
{
    std::unique_lock unique_lock(m_game_objects_mtx);

    m_game_objects.clear();
}

void* create_game_object(const char* _name, int _name_size)
{
    return game_object_holder::get().add_game_object({_name, static_cast<std::size_t>(_name_size)}).get();
}

void update_all_game_object()
{
    game_object_holder::get().update();
}

void destroy_all_game_object()
{
    game_object_holder::get().destroy();
}