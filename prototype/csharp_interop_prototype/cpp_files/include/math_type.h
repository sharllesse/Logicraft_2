//
// Created by charles on 02/11/2025.
//

#ifndef LOGICRAFT_MATH_TYPE_H
#define LOGICRAFT_MATH_TYPE_H

#include <complex>

#include "interopcpp_export.h"

struct quaternion
{
    quaternion() = default;
    quaternion(const quaternion&) noexcept = default;
    quaternion(quaternion&&) noexcept = default;

    quaternion(const float _x, const float _y, const float _z, const float _w)
        : x(_x), y(_y), z(_z), w(_w)
    {}

    ~quaternion() = default;

    float x{0.f}, y{0.f}, z{0.f}, w{0.f};
};

struct INTEROPCPP_EXPORT vector3f
{
    vector3f() = default;
    vector3f(const vector3f&) noexcept = default;
    vector3f(vector3f&&) noexcept = default;

    vector3f(const float _x, const float _y, const float _z)
        : x(_x), y(_y), z(_z)
    {}

    ~vector3f() = default;

    [[nodiscard]] vector3f operator+(const vector3f& other) const
    {
        return vector3f{x + other.x, y + other.y, z + other.z};
    }

    [[nodiscard]] vector3f operator/(const float other) const
    {
        return vector3f{x / other, y / other, z / other};
    }

    [[nodiscard]] float dot_product(const vector3f& other) const
    {
        return x * other.x + y * other.y + z * other.z;
    }

    [[nodiscard]] float length() const
    {
        return std::sqrt(x * x + y * y + z * z);
    }

    [[nodiscard]] vector3f normalize() const
    {
        const float vector_length{length()};
        if (vector_length > 0.f)
        {
            return *this / vector_length;
        }

        return vector3f{};
    }

    [[nodiscard]] quaternion to_identity() const
    {
        return quaternion{x, y, z, 0.f};
    }

    float x{0.f}, y{0.f}, z{0.f};
};

namespace external
{
    extern "C"
   {
       INTEROPCPP_EXPORT vector3f add_vector(const vector3f& _first, const vector3f& _second);
       INTEROPCPP_EXPORT float vector_dot_product(const vector3f& _first, const vector3f& _second);
       INTEROPCPP_EXPORT float vector_length(const vector3f& _vector);
       INTEROPCPP_EXPORT vector3f vector_normalized(const vector3f& _vector);
       INTEROPCPP_EXPORT quaternion vector_to_identity(const vector3f& _vector);
   }
}

#endif //LOGICRAFT_MATH_TYPE_H