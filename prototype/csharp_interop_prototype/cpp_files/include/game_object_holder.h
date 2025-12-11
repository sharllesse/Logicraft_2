//
// Created by charles on 08/11/2025.
//

#ifndef LOGICRAFT_GAME_OBJECT_HOLDER_H
#define LOGICRAFT_GAME_OBJECT_HOLDER_H
#include <memory>
#include <vector>

#include "interopcpp_export.h"
#include "game_object.h"

class game_object_holder
{
    using sptr_game_object_t = std::shared_ptr<game_object>;

public:
    static game_object_holder& get();

    sptr_game_object_t add_game_object(std::string_view _name);

    void update();

private:
    std::vector<sptr_game_object_t> m_game_objects;
};

extern "C"
{
    INTEROPCPP_EXPORT void* create_game_object(const char* _name, int _name_size);
    INTEROPCPP_EXPORT void update_game_objects();
}

#endif //LOGICRAFT_GAME_OBJECT_HOLDER_H