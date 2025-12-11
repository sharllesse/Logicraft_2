//
// Created by charles on 08/11/2025.
//

#ifndef LOGICRAFT_GAME_OBJECT_HOLDER_H
#define LOGICRAFT_GAME_OBJECT_HOLDER_H
#include <memory>
#include <vector>
#include <string_view>

#include "engine.h"
#include "game_object.h"

class game_object_holder
{
    using uptr_game_object_t = std::unique_ptr<game_object>;

public:
    static game_object_holder& get();

    uptr_game_object_t& add_game_object(std::string_view _name);

    void update();

    void destroy();
private:
    std::vector<uptr_game_object_t> m_game_objects;

    mutable std::shared_mutex m_game_objects_mtx;
};

extern "C"
{
    ENGINE_EXPORT void* create_game_object(const char* _name, int _name_size);
    ENGINE_EXPORT void update_all_game_object();
    ENGINE_EXPORT void destroy_all_game_object();
}

#endif //LOGICRAFT_GAME_OBJECT_HOLDER_H