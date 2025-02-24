using UnityEngine;

namespace _Project.Scripts.Utilities
{
    public class RandomRotator : MonoBehaviour
    {
        [SerializeField] private bool _rotateX;
        [SerializeField] private bool _rotateZ;
        [SerializeField] private bool _rotateY;

        [ContextMenu(nameof(RotateChildren))]
        private void RotateChildren()
        {
            foreach (Transform obj in GetComponentsInChildren<Transform>())
            {
                if (obj == transform) continue;

                float x = _rotateX ? Random.Range(0f, 360f) : transform.eulerAngles.x;
                float y = _rotateY ? Random.Range(0f, 360f) : transform.eulerAngles.y;
                float z = _rotateZ ? Random.Range(0f, 360f) : transform.eulerAngles.z;
                obj.eulerAngles = new Vector3(x, y, z);
            }
        }
    }
}