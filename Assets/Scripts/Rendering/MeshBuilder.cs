using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Utils;

namespace VoxelEngine.Rendering
{
    public class MeshBuilder
    {
        public const float CROSS_FACTOR = 0.5F / 1.414F;
        public const int ATLAS_SIZE = 8;
        public static Vector2[][] AtlasBuffer;

        private List<Vertex> _vertices = new List<Vertex>();
        private List<int> _triangles = new List<int>();

        public MeshBuilder()
        {
            if(AtlasBuffer != null) return;

            AtlasBuffer = new Vector2[ATLAS_SIZE * ATLAS_SIZE][];

            float size = 1f / ATLAS_SIZE;

            for (int y = 0; y < ATLAS_SIZE; y++)
            {
                for (int x = 0; x < ATLAS_SIZE; x++)
                {
                    AtlasBuffer[y * ATLAS_SIZE + x] = new Vector2[] {
                        new Vector2(x * size,1f - (y * size + size)),
                        new Vector2(x * size + size, 1f - (y * size + size)),
                        new Vector2(x * size, 1f - (y * size)),
                        new Vector2(x * size + size, 1f - (y * size))
                    };
                }
            }
        }

        public void AddTriangle(Vertex a, Vertex b, Vertex c)
        {
            int i = _vertices.Count;
            
            _vertices.Add(a);
            _vertices.Add(b);
            _vertices.Add(c);

            _triangles.Add(i);
            _triangles.Add(i + 1);
            _triangles.Add(i + 2);
        }

        public void AddQuad(Vertex a, Vertex b, Vertex c, Vertex d)
        {
            AddTriangle(a, b, c);
            AddTriangle(c, b, d);
        }

        public void AddBlock(BlockState state, int x, int y, int z, Face face, World world)
        {
            Vector3 pos = new Vector3(x, y, z);
            Vector2[] uv = null;

            if ((face & Face.YN) > 0)
            {
                uv = GetTextureUV(state.block.GetTexture(world, state, x, y, z, Face.YN));
                    
                AddQuad(
                    new Vertex { position = pos + new Vector3(0, 0, 0), uv = uv[0] },
                    new Vertex { position = pos + new Vector3(1, 0, 0), uv = uv[1] },
                    new Vertex { position = pos + new Vector3(0, 0, 1), uv = uv[2] },
                    new Vertex { position = pos + new Vector3(1, 0, 1), uv = uv[3] });
            }

            //y +
            if ((face & Face.YP) > 0)
            {
                uv = GetTextureUV(state.block.GetTexture(world, state, x, y, z, Face.YP));

                AddQuad(
                new Vertex { position = pos + new Vector3(1, 1, 1), uv = uv[3] },
                new Vertex { position = pos + new Vector3(1, 1, 0), uv = uv[1] },
                new Vertex { position = pos + new Vector3(0, 1, 1), uv = uv[2] },
                new Vertex { position = pos + new Vector3(0, 1, 0), uv = uv[0] });
            }

            //x -
            if ((face & Face.XN) > 0)
            {
                uv = GetTextureUV(state.block.GetTexture(world, state, x, y, z, Face.XN));

                AddQuad(
                new Vertex { position = pos + new Vector3(0, 1, 1), uv = uv[3] },
                new Vertex { position = pos + new Vector3(0, 1, 0), uv = uv[2] },
                new Vertex { position = pos + new Vector3(0, 0, 1), uv = uv[1] },
                new Vertex { position = pos + new Vector3(0, 0, 0), uv = uv[0] });
            }

            //x +
            if ((face & Face.XP) > 0)
            {
                uv = GetTextureUV(state.block.GetTexture(world, state, x, y, z, Face.XP));

                AddQuad(
                    new Vertex { position = pos + new Vector3(1, 0, 0), uv = uv[0] },
                    new Vertex { position = pos + new Vector3(1, 1, 0), uv = uv[2] },
                    new Vertex { position = pos + new Vector3(1, 0, 1), uv = uv[1] },
                    new Vertex { position = pos + new Vector3(1, 1, 1), uv = uv[3] });
            }

            //z -
            if ((face & Face.ZN) > 0)
            {
                uv = GetTextureUV(state.block.GetTexture(world, state, x, y, z, Face.ZN));

                AddQuad(
                new Vertex { position = pos + new Vector3(1, 1, 0), uv = uv[3] },
                new Vertex { position = pos + new Vector3(1, 0, 0), uv = uv[1] },
                new Vertex { position = pos + new Vector3(0, 1, 0), uv = uv[2] },
                new Vertex { position = pos + new Vector3(0, 0, 0), uv = uv[0] });
            }

            //z +
            if ((face & Face.ZP) > 0)
            {
                uv = GetTextureUV(state.block.GetTexture(world, state, x, y, z, Face.ZP));

                AddQuad(
                    new Vertex { position = pos + new Vector3(0, 0, 1), uv = uv[0] },
                    new Vertex { position = pos + new Vector3(1, 0, 1), uv = uv[1] },
                    new Vertex { position = pos + new Vector3(0, 1, 1), uv = uv[2] },
                    new Vertex { position = pos + new Vector3(1, 1, 1), uv = uv[3] });
            }
        }

        public void AddCactus(BlockState state, int x, int y, int z, World world)
        {
            Vector3 pos = new Vector3(x, y, z);
            Vector2[] uv = null;

            uv = GetTextureUV(state.block.GetTexture(world, state, x, y, z, Face.YN));

            float offset = 1f / 16f;

            AddQuad(
                new Vertex { position = pos + new Vector3(0, 0, 0), uv = uv[0] },
                new Vertex { position = pos + new Vector3(1, 0, 0), uv = uv[1] },
                new Vertex { position = pos + new Vector3(0, 0, 1), uv = uv[2] },
                new Vertex { position = pos + new Vector3(1, 0, 1), uv = uv[3] });

            uv = GetTextureUV(state.block.GetTexture(world, state, x, y, z, Face.YP));

            AddQuad(
                new Vertex { position = pos + new Vector3(1, 1, 1), uv = uv[3] },
                new Vertex { position = pos + new Vector3(1, 1, 0), uv = uv[1] },
                new Vertex { position = pos + new Vector3(0, 1, 1), uv = uv[2] },
                new Vertex { position = pos + new Vector3(0, 1, 0), uv = uv[0] });
            
            uv = GetTextureUV(state.block.GetTexture(world, state, x, y, z, Face.XN));

            AddQuad(
                new Vertex { position = pos + new Vector3(offset, 1, 1), uv = uv[3] },
                new Vertex { position = pos + new Vector3(offset, 1, 0), uv = uv[2] },
                new Vertex { position = pos + new Vector3(offset, 0, 1), uv = uv[1] },
                new Vertex { position = pos + new Vector3(offset, 0, 0), uv = uv[0] });
            
            AddQuad(
                new Vertex { position = pos + new Vector3(1 - offset, 0, 0), uv = uv[0] },
                new Vertex { position = pos + new Vector3(1 - offset, 1, 0), uv = uv[2] },
                new Vertex { position = pos + new Vector3(1 - offset, 0, 1), uv = uv[1] },
                new Vertex { position = pos + new Vector3(1 - offset, 1, 1), uv = uv[3] });
            
            AddQuad(
                new Vertex { position = pos + new Vector3(1, 1, offset), uv = uv[3] },
                new Vertex { position = pos + new Vector3(1, 0, offset), uv = uv[1] },
                new Vertex { position = pos + new Vector3(0, 1, offset), uv = uv[2] },
                new Vertex { position = pos + new Vector3(0, 0, offset), uv = uv[0] });
                    
            AddQuad(
                new Vertex { position = pos + new Vector3(0, 0, 1 - offset), uv = uv[0] },
                new Vertex { position = pos + new Vector3(1, 0, 1 - offset), uv = uv[1] },
                new Vertex { position = pos + new Vector3(0, 1, 1 - offset), uv = uv[2] },
                new Vertex { position = pos + new Vector3(1, 1, 1 - offset), uv = uv[3] });
        }

        public void AddCross(BlockState state, int x, int y, int z, World world)
        {
            Vector2[] uv = GetTextureUV(state.block.GetTexture(world, state, x, y, z, Face.XN));

            Vector3 pos = new Vector3(x, y, z);

            AddQuad(
                new Vertex { position = pos + new Vector3(0.5f + CROSS_FACTOR, 1, 0.5f + CROSS_FACTOR), uv = uv[3] },
                new Vertex { position = pos + new Vector3(0.5f - CROSS_FACTOR, 1, 0.5f - CROSS_FACTOR), uv = uv[2] },
                new Vertex { position = pos + new Vector3(0.5f + CROSS_FACTOR, 0, 0.5f + CROSS_FACTOR), uv = uv[1] },
                new Vertex { position = pos + new Vector3(0.5f - CROSS_FACTOR, 0, 0.5f - CROSS_FACTOR), uv = uv[0] });

            AddQuad(
                new Vertex { position = pos + new Vector3(0.5f - CROSS_FACTOR, 1, 0.5f + CROSS_FACTOR), uv = uv[3] },
                new Vertex { position = pos + new Vector3(0.5f + CROSS_FACTOR, 1, 0.5f - CROSS_FACTOR), uv = uv[2] },
                new Vertex { position = pos + new Vector3(0.5f - CROSS_FACTOR, 0, 0.5f + CROSS_FACTOR), uv = uv[1] },
                new Vertex { position = pos + new Vector3(0.5f + CROSS_FACTOR, 0, 0.5f - CROSS_FACTOR), uv = uv[0] });

            AddQuad(
                new Vertex { position = pos + new Vector3(0.5f - CROSS_FACTOR, 1, 0.5f - CROSS_FACTOR), uv = uv[3] },
                new Vertex { position = pos + new Vector3(0.5f + CROSS_FACTOR, 1, 0.5f + CROSS_FACTOR), uv = uv[2] },
                new Vertex { position = pos + new Vector3(0.5f - CROSS_FACTOR, 0, 0.5f - CROSS_FACTOR), uv = uv[1] },
                new Vertex { position = pos + new Vector3(0.5f + CROSS_FACTOR, 0, 0.5f + CROSS_FACTOR), uv = uv[0] });

            AddQuad(
                new Vertex { position = pos + new Vector3(0.5f + CROSS_FACTOR, 1, 0.5f - CROSS_FACTOR), uv = uv[3] },
                new Vertex { position = pos + new Vector3(0.5f - CROSS_FACTOR, 1, 0.5f + CROSS_FACTOR), uv = uv[2] },
                new Vertex { position = pos + new Vector3(0.5f + CROSS_FACTOR, 0, 0.5f - CROSS_FACTOR), uv = uv[1] },
                new Vertex { position = pos + new Vector3(0.5f - CROSS_FACTOR, 0, 0.5f + CROSS_FACTOR), uv = uv[0] });
        }

        public Mesh ToMesh()
        {
            int length = _vertices.Count;

            Vector3[] positions = new Vector3[length];
            Vector2[] uvs = new Vector2[length];

            for (int i = 0; i < length; i ++)
            {
                Vertex v = _vertices[i];

                positions[i] = v.position;
                uvs[i] = v.uv;
            }

            Mesh mesh = new Mesh();
            mesh.vertices = positions;
            mesh.triangles = _triangles.ToArray();
            mesh.uv = uvs;

            mesh.RecalculateNormals();
            return mesh;
        }

        public static Vector2[] GetTextureUV(int texture)
        {
            return AtlasBuffer[texture];
        }
    }  
}