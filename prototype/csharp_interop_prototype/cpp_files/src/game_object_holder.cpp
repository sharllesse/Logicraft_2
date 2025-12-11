//
// Created by charles on 08/11/2025.
//

#include "game_object_holder.h"

#include <iostream>

game_object_holder& game_object_holder::get()
{
    static game_object_holder gm_holder{};
    return gm_holder;
}

game_object_holder::sptr_game_object_t game_object_holder::add_game_object(std::string_view _name)
{
    return m_game_objects.emplace_back(std::make_shared<game_object>(_name));
}

void game_object_holder::update()
{
    for (sptr_game_object_t& game_object : m_game_objects)
    {
        game_object->update();
    }
}

void* create_game_object(const char* _name, int _name_size)
{
    return game_object_holder::get().add_game_object({_name, static_cast<std::size_t>(_name_size)}).get();
}

void update_game_objects()
{
    game_object_holder::get().update();
}
