using UnityEngine;

namespace _Project.Scripts.Utilities
{
    public class DestroyOnAwake : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}