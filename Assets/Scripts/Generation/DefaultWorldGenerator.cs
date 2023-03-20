using UnityEngine;
using VoxelEngine.Data;
using VoxelEngine.Utils;

namespace VoxelEngine.Generation
{
    [System.Serializable]
    public class DefaultWorldGenerator : WorldGenerator
    {
        public const float BIOME_SCALE = 0.005f;

        public int seed = 0;

        public BiomeInfo GetBiome(int x, int z)
        {
            float r = Mathf.PerlinNoise((x + 150_000 + seed * 127) * BIOME_SCALE, (z + 150_000 - seed * 15) * BIOME_SCALE) * 20;

            switch (r)
            {
                case > 10:
                    return new BiomeInfo { id = 1, strength = Mathf.Clamp01((r - 10) / 3f) };

                default:
                    return new BiomeInfo { id = 0, strength = Mathf.Clamp01((10 - r) / 3f) };
            }
        }

        public override void OnGenerate(Chunk chunk)
        {
            for (int x = 0; x < World.CHUNK_SIZE; x++)
            {
                for (int z = 0; z < World.CHUNK_SIZE; z++)
                {
                    int wx = chunk.position.x * World.CHUNK_SIZE + x;
                    int wz = chunk.position.y * World.CHUNK_SIZE + z;

                    float r = Mathf.PerlinNoise((wx + 100_000 - seed * 777) * 0.015f, (wz + 100_000 + seed * 777) * 0.015f) * 20 + 10;

                    BiomeInfo biome = GetBiome(wx, wz);

                    //Mountains
                    if (biome.id == 0)
                    {
                        float f = Mathf.PerlinNoise((wx - seed * 777) * 0.017f, (wx + seed * 397) * 0.017f);
                        r += Mathf.Clamp01(f - 0.3f) * 60 * biome.strength;
                    }

                    int ir = (int) r;

                    int dCount = 2 + (int) Mathf.Clamp(Mathf.PerlinNoise((wx - seed * 717) * 0.015f, (wx + seed * 997) * 0.012f) * 5, 0, 4);

                    chunk.SetBlockState(x, 0, z, new BlockState { block = Blocks.Bedrock});

                    for (int y = 1; y <= ir; y++)
                    {
                        float f = Math.PerlinNoise3D((wx - seed * 777) * 0.05f, y * 0.05f, (wz + seed * 193) * 0.05f);
                        
                        if (f > 0.2f && f < 0.6f)
                        {
                            if(y == ir)
                                chunk.SetBlockState(x, y, z, new BlockState { block = biome.id == 0 ? Blocks.Grass : Blocks.Sand });
                            else if(y > r - dCount)
                                chunk.SetBlockState(x, y, z, new BlockState { block = biome.id == 0 ? Blocks.Dirt : Blocks.Sand });
                            else
                                chunk.SetBlockState(x, y, z, new BlockState { block = Blocks.Stone });
                        }
                    }
                }
            }

            world.worldRenderer.Render(chunk);
            world.worldRenderer.RenderNeighbours(chunk);
        }

        public override void OnDecorate(Chunk chunk)
        {
            #region Trees, Cacti and DeadBushes
            //Trees
            System.Random r = new System.Random(chunk.position.GetHashCode() ^ seed);
            for (int t = 0; t < r.Next(24); t++)
            {
                int cx = r.Next(World.CHUNK_SIZE);
                int cz = r.Next(World.CHUNK_SIZE);

                int wx = chunk.position.x * World.CHUNK_SIZE + cx;
                int wz = chunk.position.y * World.CHUNK_SIZE + cz;

                BiomeInfo biome = GetBiome(wx, wz);

                switch (biome.id)
                {
                    case 0:
                        if (biome.strength > 0.1f - r.NextDouble() * 0.1f)
                        {
                            if (r.NextDouble() < 0.3f)
                            {
                                float f = Mathf.PerlinNoise(wx * 0.017f, wz * 0.017f);

                                if (r.NextDouble() < Mathf.Clamp01(f - 0.3f)) continue;

                                int cy = 0;
                                for (cy = World.CHUNK_HEIGHT - 1; cy >= 0; cy--)
                                    if (chunk.GetBlockState(cx, cy, cz).id != 0) { cy++; break; }

                                //Check if is grass block
                                if (chunk.GetBlockState(cx, cy - 1, cz).block != Blocks.Grass)
                                    continue;

                                int h = 4 + r.Next(3);
                                int dfg = r.Next(1) + 1;
                                int type = r.Next(100) % 2;

                                for (int x = -2; x <= 2; x++)
                                {
                                    for (int z = -2; z <= 2; z++)
                                    {
                                        for (int y = cy; y <= cy + h; y++)
                                        {
                                            if (y == cy + h)
                                            {
                                                if (Mathf.Abs(x) + Mathf.Abs(z) < 2 && y > cy + dfg)
                                                    if (chunk.GetBlockState(cx + x, y, cz + z).id == 0)
                                                        chunk.SetBlockState(cx + x, y, cz + z, new BlockState { block = Blocks.Leaves });
                                            }
                                            else if (x == 0 && z == 0)
                                            {
                                                chunk.SetBlockState(cx + x, y, cz + z, new BlockState { block = Blocks.Log });
                                            }
                                            else
                                            {
                                                if (type == 0 || y == cy + h - 1)
                                                {
                                                    if (Mathf.Abs(x) <= 1 && Mathf.Abs(z) <= 1 && y > cy + dfg)
                                                        if (chunk.GetBlockState(cx + x, y, cz + z).id == 0)
                                                            chunk.SetBlockState(cx + x, y, cz + z, new BlockState { block = Blocks.Leaves });
                                                }
                                                else if (!(Mathf.Abs(x) == 2 && Mathf.Abs(z) == 2) && y > cy + dfg)
                                                    if (chunk.GetBlockState(cx + x, y, cz + z).id == 0)
                                                        chunk.SetBlockState(cx + x, y, cz + z, new BlockState { block = Blocks.Leaves });
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                int cy = 0;
                                for (cy = World.CHUNK_HEIGHT - 1; cy >= 0; cy--)
                                    if (chunk.GetBlockState(cx, cy, cz).id != 0) { cy++; break; }

                                //Check if is sand block
                                if (chunk.GetBlockState(cx, cy - 1, cz).block != Blocks.Grass)
                                    continue;

                                chunk.SetBlockState(cx, cy, cz, new BlockState { block = Blocks.Flower, data = (byte) (r.NextDouble() < 0.5f ? 0 : 1) });
                            }
                        }
                        break;

                    case 1:
                        if (biome.strength > 0.05f - r.NextDouble() * 0.05f && r.NextDouble() > 0.5f)
                        {
                            int cy = 0;
                            for (cy = World.CHUNK_HEIGHT - 1; cy >= 0; cy--)
                                if (chunk.GetBlockState(cx, cy, cz).id != 0) { cy++; break; }

                            //Check if is sand block
                            if (chunk.GetBlockState(cx, cy - 1, cz).block != Blocks.Sand)
                                continue;

                            if (r.NextDouble() < 0.25f) //Cacti
                            {
                                int h = 1 + r.Next(4);
                                for (int y = cy; y <= cy + h; y++)
                                {
                                    chunk.SetBlockState(cx, y, cz, new BlockState { block = Blocks.Cactus });
                                }
                            }
                            else
                                chunk.SetBlockState(cx, cy, cz, new BlockState { block = Blocks.DeadBush });
                        }
                        break;
                }

            }
            #endregion
            
            #region Ores
            //Coal Ore
            for (int v = 0; v < r.Next(100) + 50; v++)
            {
                int x = r.Next(16) % 16;
                int y = r.Next(30);
                int z = r.Next(16) % 16;

                if (chunk.GetBlockState(x, y, z).block == Blocks.Stone)
                {
                    int c = 3 + r.Next(10);

                    while (c > 0)
                    {
                        chunk.SetBlockState(x, y, z, new BlockState { block = Blocks.CoalOre });

                        switch (r.Next(6))
                        {
                            case 0: x++; break;
                            case 1: y++; break;
                            case 2: z++; break;
                            case 3: x--; break;
                            case 4: y--; break;
                            default: z--; break;
                        }

                        if (chunk.GetBlockState(x, y, z).block != Blocks.Stone)
                            break;

                        c--;
                    }
                }
            }

            //Iron Ore
            for (int v = 0; v < r.Next(100) + 30; v++)
            {
                int x = r.Next(16) % 16;
                int y = r.Next(30);
                int z = r.Next(16) % 16;

                if (chunk.GetBlockState(x, y, z).block == Blocks.Stone)
                {
                    int c = 3 + r.Next(5);

                    while (c > 0)
                    {
                        chunk.SetBlockState(x, y, z, new BlockState { block = Blocks.IronOre });

                        switch (r.Next(6))
                        {
                            case 0: x++; break;
                            case 1: y++; break;
                            case 2: z++; break;
                            case 3: x--; break;
                            case 4: y--; break;
                            default: z--; break;
                        }

                        if (chunk.GetBlockState(x, y, z).block != Blocks.Stone)
                            break;

                        c--;
                    }
                }
            }
            #endregion
        }
    }
}