using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using VoxelEngine.Utils;
using VoxelEngine.Data;

namespace VoxelEngine.Rendering
{
    [Serializable]
    public class WorldRenderer : Queue<Chunk>
    {
        [SerializeField, HideInInspector]
        public Material material;
        public World world;

        private void _Render(Chunk chunk)
        {
            if(chunk == null) return;

            MeshFilter filter = GetOrAddComponent<MeshFilter>(chunk);
            MeshRenderer renderer = GetOrAddComponent<MeshRenderer>(chunk);
            MeshCollider collider = GetOrAddComponent<MeshCollider>(chunk);

            MeshBuilder builder = new MeshBuilder();

            for (int x = 0; x < World.CHUNK_SIZE; x++)
            {
                for (int z = 0; z < World.CHUNK_SIZE; z++)
                {
                    for (int y = 0; y < World.CHUNK_HEIGHT; y++)
                    {
                        BlockState state = chunk.GetBlockState(x, y, z);
                        Block block = state.block;

                        if (block.ShouldRender(world, state, x, y, z))
                        {
                            switch(block.GetBlockModel(world, state, x, y, z))
                            {
                                case 0:
                                    {
                                        Face face = Face.None;

                                        if (IsTransparent(chunk, x - 1, y, z, state)) face |= Face.XN;
                                        if (IsTransparent(chunk, x + 1, y, z, state)) face |= Face.XP;
                                        if (IsTransparent(chunk, x, y - 1, z, state)) face |= Face.YN;
                                        if (IsTransparent(chunk, x, y + 1, z, state)) face |= Face.YP;
                                        if (IsTransparent(chunk, x, y, z - 1, state)) face |= Face.ZN;
                                        if (IsTransparent(chunk, x, y, z + 1, state)) face |= Face.ZP;

                                        builder.AddBlock(state, x, y, z, face, world);
                                    }
                                    break;

                                case 1:
                                    builder.AddCactus(state, x, y, z, world);
                                    break;

                                case 2:
                                    builder.AddCross(state, x, y, z, world);
                                    break;
                            }
                        }
                    }
                }
            }

            Mesh mesh = builder.ToMesh();
            filter.mesh = mesh;
            collider.sharedMesh = mesh;

            renderer.material = material;
        }

        public void Render(GameObject target, IBlockHolder holder)
        {
            if(holder == null) return;

            MeshFilter filter = GetOrAddComponent<MeshFilter>(target);
            MeshRenderer renderer = GetOrAddComponent<MeshRenderer>(target);

            MeshBuilder builder = new MeshBuilder();

            BlockState state = holder.GetBlockState();
            Block block = state.block;

            if (block.ShouldRender(world, state, 0, 0, 0))
            {
                switch(block.GetBlockModel(world, state, 0, 0, 0))
                {
                    case 0:
                        {
                            builder.AddBlock(state, 0, 0, 0, Face.All, world);
                        }
                        break;

                    case 1:
                        builder.AddCactus(state, 0, 0, 0, world);
                        break;

                    case 2:
                        builder.AddCross(state, 0, 0, 0, world);
                        break;
                }
            }


            Mesh mesh = builder.ToMesh();
            filter.mesh = mesh;

            renderer.material = material;
        }

        public static T GetOrAddComponent<T>(Component component) where T : Component
        {
            T t = component.GetComponent<T>();
            return t == null ? component.gameObject.AddComponent<T>() : t;
        }

        public static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
        {
            T t = gameObject.GetComponent<T>();
            return t == null ? gameObject.AddComponent<T>() : t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsTransparent(Chunk chunk, int x, int y, int z, BlockState blockState)
        {
            BlockState state = chunk.GetBlockState(x, y, z);
            return state.id != blockState.id && !state.block.IsOpaque(world, state, x, y, z);
        }

        #region Rendering
        public void Render(Chunk chunk)
        {
            if(chunk != null && chunk.decorated)
                Enqueue(chunk);
        }
        
        public void RenderNeighbours(Chunk chunk)
        {
            Vector2Int pos = chunk.position;
            RenderNeighbours(pos.x, pos.y);
        }

        public void RenderNeighbours(int x, int z)
        {
            Render(world.GetChunk(x + 1, z));
            Render(world.GetChunk(x - 1, z));
            Render(world.GetChunk(x, z + 1));
            Render(world.GetChunk(x, z - 1));
        }

        #endregion

        public override bool Tick()
        {
            if(HasQueue())
            {
                _Render(Dequeue());
                return true;
            }

            return false;
        }
    
    }
}