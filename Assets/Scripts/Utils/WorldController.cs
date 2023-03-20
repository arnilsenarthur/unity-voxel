using UnityEngine;

namespace VoxelEngine.Utils
{
    public class WorldController : MonoBehaviour
    {
        [Header("REFERENCES")]
        public World world;

        [Header("SETTINGS")]
        [Range(2, 10)]
        public int viewDistance = 3;

        public void FixedUpdate()
        {
            int bPosX = (int) transform.position.x;
            int bPosZ= (int) transform.position.z;

            Vector2Int pos = Math.WorldToChunkPosition(bPosX, bPosZ);

            world.BeginChunkClearing();

            int radius = viewDistance * 2;
            int rq = radius * radius;

            for (int x = -radius; x <= radius; x++)
            {
                for (int z = -radius; z <= radius; z++)
                {
                    if (x * x + z * z < rq)
                    {
                        world.EnsureChunk(pos.x + x, pos.y + z);
                    }
                }
            }

            world.EndChunkClearing();
        }

    }
}