using System.Runtime.CompilerServices;
using UnityEngine;
using VoxelEngine.Utils;

namespace VoxelEngine
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private BlockState[] _blocks = new BlockState[World.CHUNK_VOLUME];

        [Header("REFERENCES")]
        public World world;

        [Header("STATE")]
        public Vector2Int position;
        public bool decorated = false;

        #region Unity Callbacks
        public void Start()
        {
            world.worldGenerator.Enqueue(this);
        }

        public void OnEnable()
        {
            world.RegisterChunk(position.x, position.y, this);

            transform.position = new Vector3(
                position.x * World.CHUNK_SIZE,
                0,
                position.y * World.CHUNK_SIZE
            );
        }
        
        public void OnDisable()
        {
            world.UnregisterChunk(position.x, position.y);
        }
        #endregion

        #region Utils
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool _IsOnBoundsY(int y)
        {
            return y >= 0 && y < World.CHUNK_HEIGHT;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool _IsOnBoundsXZ(int x, int z)
        {
            return x >= 0 && x < World.CHUNK_SIZE && z >= 0 && z < World.CHUNK_SIZE;
        } 
        #endregion

        #region Block State
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BlockState GetBlockState(int x, int y, int z)
        {
            if(_IsOnBoundsY(y))
            {
                if (_IsOnBoundsXZ(x, z))
                    return _blocks[y * World.CHUNK_LAYER + z * World.CHUNK_SIZE + x];
                else
                {
                    Vector3Int pos = Math.ChunkToWorldBlockPosition(x, y, z, position.x, position.y);
                    return world.GetBlockState(pos.x, pos.y, pos.z);
                }
            }

            return BlockState.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BlockState GetBlockState(Vector3Int pos)
        {
            return GetBlockState(pos.x, pos.y, pos.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBlockState(int x, int y, int z, BlockState state)
        {
            if(_IsOnBoundsY(y))
            {
                if (_IsOnBoundsXZ(x, z))
                    _blocks[y * World.CHUNK_LAYER + z * World.CHUNK_SIZE + x] = state;
                else
                {
                    Vector3Int pos = Math.ChunkToWorldBlockPosition(x, y, z, position.x, position.y);
                    world.SetBlockState(pos.x, pos.y, pos.z, state);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBlockState(Vector3Int pos, BlockState state)
        {
            SetBlockState(pos.x, pos.y, pos.z, state);
        }
        #endregion
    }
}