using System.Runtime.CompilerServices;
using UnityEngine;

namespace VoxelEngine.Utils
{
    public static class Math
    {
        public const int CHUNK_SIZE_L1 = World.CHUNK_SIZE - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int WorldToChunkPosition(int x, int z)
        {
            return new Vector2Int(
                x < 0 ? (x - CHUNK_SIZE_L1) / World.CHUNK_SIZE : x / World.CHUNK_SIZE,
                z < 0 ? (z - CHUNK_SIZE_L1) / World.CHUNK_SIZE : z / World.CHUNK_SIZE
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int WorldToChunkBlockPosition(int x, int y, int z)
        {
            return new Vector3Int(
                ((x % World.CHUNK_SIZE) + World.CHUNK_SIZE) % World.CHUNK_SIZE,
                ((y % World.CHUNK_HEIGHT) + World.CHUNK_HEIGHT) % World.CHUNK_HEIGHT,
                ((z % World.CHUNK_SIZE) + World.CHUNK_SIZE) % World.CHUNK_SIZE
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int ChunkToWorldBlockPosition(int x, int y, int z, int cx, int cz)
        {
            return new Vector3Int(
                x + cx * World.CHUNK_SIZE,
                y,
                z + cz * World.CHUNK_SIZE
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PerlinNoise3D(float x, float y, float z)
        {
            x += 50000;
            y += 30000;
            
            float xy = Mathf.PerlinNoise(x, y);
            float xz = Mathf.PerlinNoise(x, z);
            float yz = Mathf.PerlinNoise(y, z);
            float yx = Mathf.PerlinNoise(y, x);
            float zx = Mathf.PerlinNoise(z, x);
            float zy = Mathf.PerlinNoise(z, y);

            return (xy + xz + yz + yx + zx + zy) / 6;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int GetDirection(Vector3 offset)
        {
            float ax = Mathf.Abs(offset.x);
            float ay = Mathf.Abs(offset.y);
            float az = Mathf.Abs(offset.z);

            if(ax > ay && ax > az)
            {
                return new Vector3Int(offset.x < 0 ? - 1 : 1, 0, 0);
            }
            else if(ay > ax && ay > az)
            {
                return new Vector3Int(0, offset.y < 0 ? - 1 : 1, 0);
            }
            else
            {
                return new Vector3Int(0, 0, offset.z < 0 ? - 1 : 1);
            }
        }
    }
}