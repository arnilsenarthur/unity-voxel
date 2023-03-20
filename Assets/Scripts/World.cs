using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using VoxelEngine.Data;
using VoxelEngine.Utils;
using VoxelEngine.Entities;
using VoxelEngine.Rendering;
using VoxelEngine.Generation;

namespace VoxelEngine
{
    public class World : MonoBehaviour
    {
        public const int CHUNK_SIZE = 16;
        public const int CHUNK_HEIGHT = 64;
        public const int CHUNK_LAYER = CHUNK_SIZE * CHUNK_SIZE;
        public const int CHUNK_VOLUME = CHUNK_LAYER * CHUNK_HEIGHT;

        [Header("REFERENCES")]
        public Material worldMaterial;
        public GameObject blockBreakParticle;
        public GameObject fallingSand;

        [Header("SETTINGS")]
        [Range(-10_000, 10_000)]
        public int seed = 0;

        [Header("WORKERS")]
        public WorldRenderer worldRenderer;
        public WorldGenerator worldGenerator;

        [Header("STATE")]
        private Dictionary<Vector2Int, Chunk> _chunks = new Dictionary<Vector2Int, Chunk>();
        [SerializeField, HideInInspector]
        private List<Vector2Int> _toClear = new List<Vector2Int>();

        #region Unity Callbacks
        public void Start()
        {
            worldRenderer = new WorldRenderer{material = worldMaterial, world = this};
        }
        
        public void OnEnable()
        {
            worldGenerator = new DefaultWorldGenerator{world = this, seed = this.seed};
        }

        public void Update()
        {
            worldGenerator.Tick();
            worldRenderer.Tick();
        }
        #endregion

        #region Chunk
        public Chunk LoadChunk(int x, int z)
        {
            GameObject o = new GameObject($"Chunk({x}, {z})");
            o.SetActive(false);
            o.transform.parent = transform;

            Chunk c = o.AddComponent<Chunk>();
            c.world = this;
            c.position = new Vector2Int(x, z);
            o.SetActive(true);

            return c;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Chunk GetChunk(int x,int z)
        {
            return _chunks.GetValueOrDefault(new Vector2Int(x, z), null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Chunk GetChunk(Vector2Int pos)
        {
            return _chunks.GetValueOrDefault(pos, null);
        }

        public void RegisterChunk(int x,int z, Chunk chunk)
        {
            _chunks[new Vector2Int(x, z)] = chunk;
        }

        public void UnregisterChunk(int x, int z)
        {
            _chunks.Remove(new Vector2Int(x, z));
        }

        public void EnsureChunk(int x, int z)
        {
            Vector2Int key = new Vector2Int(x, z);
            _toClear.Remove(key);

            if(_chunks.ContainsKey(key))
                return;

            LoadChunk(x, z);
        }
        #endregion

        #region Chunk Column Clearing
        public void BeginChunkClearing()
        {
            _toClear.AddRange(_chunks.Keys);
        }

        public void EndChunkClearing()
        {
            foreach(Vector2Int key in _toClear)
            {
                Chunk c = _chunks[key];
                GameObject.Destroy(c.gameObject);
                worldRenderer.RenderNeighbours(key.x, key.y);
            } 

            _toClear.Clear();
        }
        #endregion

        #region Block State
        public Chunk GetChunkForBlock(Vector3Int blockPos)
        {
            Vector2Int pos = Math.WorldToChunkPosition(blockPos.x, blockPos.z);
            return GetChunk(pos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BlockState GetBlockState(int x, int y, int z)
        { 
            Vector2Int cPos = Math.WorldToChunkPosition(x, z);
            Chunk chunk = GetChunk(cPos);

            return chunk == null ? new BlockState() : chunk.GetBlockState(Math.WorldToChunkBlockPosition(x, y, z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBlockState(int x, int y, int z, BlockState state, bool update = false)
        {
            Vector2Int cPos = Math.WorldToChunkPosition(x, z);
            Chunk chunk = GetChunk(cPos);

            chunk?.SetBlockState(Math.WorldToChunkBlockPosition(x, y, z), state);

            if(update)
            {
                worldRenderer.Render(chunk);
                worldRenderer.RenderNeighbours(chunk);
                UpdateNeighboursBlocks(x, y, z);
            }
        }

        public void UpdateNeighboursBlocks(int x, int y, int z)
        {
            UpdateBlock(x + 1, y, z);
            UpdateBlock(x - 1, y, z);
            UpdateBlock(x, y + 1, z);
            UpdateBlock(x, y - 1, z);
            UpdateBlock(x, y, z + 1);
            UpdateBlock(x, y, z - 1);
            
        }

        public void UpdateBlock(int x, int y, int z)
        {
            BlockState state = GetBlockState(x, y, z);
            state.block.UpdateBlockState(this, state, x, y, z);
        }
        #endregion

        #region Actions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BreakBlock(int x, int y, int z)
        {
            BlockState state = GetBlockState(x, y, z);
            if(state.block == Blocks.Air)
                return;

            SetBlockState(x, y, z, BlockState.Empty, true);
            SpawnBreakParticle(x, y, z, state);
        }
        #endregion

        #region Particles
        public void SpawnBreakParticle(int x, int y, int z, BlockState state)
        {
            GameObject o = GameObject.Instantiate(blockBreakParticle);
            o.transform.position = new Vector3(x + 0.5f, y + 0.5f, z + 0.5f);

            ParticleSystem system = o.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = system.main;

            Color[] c = state.block.GetBreakColors(this, state, x, y, z);
            main.startColor = new ParticleSystem.MinMaxGradient(c[0], c[1]);
        }
        #endregion

        #region Entities
        public void SpawnFallingSand(int x, int y, int z, BlockState state)
        {
            GameObject o = GameObject.Instantiate(fallingSand);
            o.transform.position = new Vector3(x + 0.5f, y + 0.5f, z + 0.5f);
            FallingBlock fallingBlock = o.GetComponent<FallingBlock>();
            fallingBlock.state = state;
            fallingBlock.world = this;
        }
        #endregion
    }
}