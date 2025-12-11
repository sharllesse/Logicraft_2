//
// Created by charles on 08/11/2025.
//

#ifndef LOGICRAFT_GAME_OBJECT_H
#define LOGICRAFT_GAME_OBJECT_H

#include <memory>
#include <string>
#include <vector>
#include <string_view>
#include <shared_mutex>

#include "component.h"
#include "engine.h"

class game_object final
{
    using uptr_component_t = std::unique_ptr<component>;

public:
    game_object() = default;
    explicit game_object(std::string_view _name);
    game_object(const game_object&) = delete;
    game_object(game_object&& _other) noexcept;
    ~game_object() = default;

    game_object& operator=(const game_object&) = delete;
    game_object& operator=(game_object&& _other) noexcept;

    void start();
    void update();

    component& add_component(void* _gc_handle);

    [[nodiscard]] std::string_view get_name() const;
    [[nodiscard]] const std::vector<uptr_component_t>& get_components() const;
private:
    std::string m_name{"game object"};

    std::vector<uptr_component_t> m_components;

    mutable std::shared_mutex m_game_object_mtx;
};

extern "C"
{
    ENGINE_EXPORT void* add_component(void* _internal_game_object, void* _gc_handle);
}

#endif //LOGICRAFT_GAME_OBJECT_H