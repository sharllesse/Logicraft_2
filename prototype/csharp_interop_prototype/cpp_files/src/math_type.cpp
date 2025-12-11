//
// Created by charles on 02/11/2025.
//

#include "math_type.h"

namespace external
{
    vector3f add_vector(const vector3f & _first, const vector3f & _second)
    {
        return _first + _second;
    }

    float vector_dot_product(const vector3f &_first, const vector3f &_second)
    {
        return _first.dot_product(_second);
    }

    float vector_length(const vector3f &_vector)
    {
        return _vector.length();
    }

    vector3f vector_normalized(const vector3f &_vector)
    {
        return _vector.normalize();
    }

    quaternion vector_to_identity(const vector3f &_vector)
    {
        return _vector.to_identity();
    }
}
