using UnityEngine;
using VoxelEngine.Utils;

namespace VoxelEngine.Data
{ 
    public class DeadBushBlock : CrossBlock
    {
        public override int GetTexture(World world, BlockState state, int x, int y, int z, Face face)
        {
            return 11;
        }

        public override Color[] GetBreakColors(World world, BlockState state, int x, int y, int z)
        {
            return new Color[]{
                new Color(0.2f, 0.1f, 0f),
                new Color(0.4f, 0.3f, 0.2f)
            };
        }

        public override void UpdateBlockState(World world, BlockState state, int x, int y, int z)
        {
            BlockState bellow = world.GetBlockState(x, y - 1, z);

            if(bellow.block != Blocks.Sand)
            {
                world.BreakBlock(x, y, z);
            }
        }

        public override bool CanPlaceBlock(World world, BlockState state, int x, int y, int z)
        {
            BlockState bellow = world.GetBlockState(x, y - 1, z);

            return bellow.block == Blocks.Sand;
        }
    }
}