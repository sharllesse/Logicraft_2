//
// Created by charles on 08/11/2025.
//

#ifndef LOGICRAFT_GAME_OBJECT_H
#define LOGICRAFT_GAME_OBJECT_H

#include "component.h"
#include "interopcpp_export.h"

#include <memory>
#include <string>
#include <vector>

class game_object final
{
    using sptr_component_t = std::shared_ptr<component>;

public:
    game_object() = default;
    explicit game_object(std::string_view _name);
    game_object(const game_object&) = delete;
    game_object(game_object&&) noexcept = default;
    ~game_object();

    game_object& operator=(const game_object&) = delete;
    game_object& operator=(game_object&&) noexcept = default;

    void start();
    void update();

    component& add_component(void* _gc_handle);

private:
    std::string m_name{"game object"};

    std::vector<sptr_component_t> m_components;
};

extern "C"
{
    INTEROPCPP_EXPORT void* add_component(void* _internal_game_object, void* _gc_handle);
}

#endif //LOGICRAFT_GAME_OBJECT_H