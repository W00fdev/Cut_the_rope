using UnityEngine;

namespace _Project.Scripts.Utilities.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 SingleValue(float value)
        {
            return new Vector3(value, value, value);
        }
    }
}