using UnityEngine;
using VoxelEngine.Utils;

namespace VoxelEngine.Data
{ 
    public class SandBlock : Block
    {
        public override int GetTexture(World world, BlockState state, int x, int y, int z, Face face)
        {
            return 4;
        }

        public override Color[] GetBreakColors(World world, BlockState state, int x, int y, int z)
        {
            return new Color[]{
                new Color(0.8f, 0.8f, 0.6f),
                new Color(0.6f, 0.6f, 0.4f)
            };
        }

        public override void UpdateBlockState(World world, BlockState state, int x, int y, int z)
        {
            BlockState bellow = world.GetBlockState(x, y - 1, z);

            if(bellow.block == Blocks.Air)
            {
                world.SetBlockState(x, y, z, BlockState.Empty, true);
                world.SpawnFallingSand(x, y, z, state);
            }
        }
    }
}