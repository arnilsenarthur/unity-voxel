using UnityEngine;

namespace VoxelEngine.Utils
{
    public class DestroyAfter : MonoBehaviour
    {
        public float delay;

        void FixedUpdate()
        {
            if((delay -= Time.fixedDeltaTime) <= 0)
                Destroy(gameObject);
        }
    }
}