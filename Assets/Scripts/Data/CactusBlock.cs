using UnityEngine;
using VoxelEngine.Utils;

namespace VoxelEngine.Data
{ 
    public class CactusBlock : Block
    {
        public override int GetTexture(World world, BlockState state, int x, int y, int z, Face face)
        {
            switch (face)
            {
                case Face.YP:
                    return 8;

                case Face.YN:
                    return 10;

                default:
                    return 9;
            }
        }

        public override int GetBlockModel(World world, BlockState state, int x, int y, int z)
        {
            return 1;
        }

        public override bool IsOpaque(World world, BlockState state, int x, int y, int z)
        {
            return false;
        }

        public override void UpdateBlockState(World world, BlockState state, int x, int y, int z)
        {
            BlockState bellow = world.GetBlockState(x, y - 1, z);

            if(bellow.block != Blocks.Cactus && bellow.block != Blocks.Sand)
            {
                world.BreakBlock(x, y, z);
                return;
            }
            
            if(world.GetBlockState(x + 1, y, z).block != Blocks.Air ||
                world.GetBlockState(x - 1, y, z).block != Blocks.Air ||
                world.GetBlockState(x, y, z + 1).block != Blocks.Air ||
                world.GetBlockState(x, y, z - 1).block != Blocks.Air)
            {
                world.BreakBlock(x, y, z);
            }
        }

        public override Color[] GetBreakColors(World world, BlockState state, int x, int y, int z)
        {
            return new Color[]{
                new Color(0f, 0.2f, 0f),
                new Color(0.2f, 0.6f, 0.2f)
            };
        }

        public override bool CanPlaceBlock(World world, BlockState state, int x, int y, int z)
        {
            BlockState bellow = world.GetBlockState(x, y - 1, z);

            if(bellow.block != Blocks.Cactus && bellow.block != Blocks.Sand)
            {
                return false;
            }
            
            if(world.GetBlockState(x + 1, y, z).block != Blocks.Air ||
                world.GetBlockState(x - 1, y, z).block != Blocks.Air ||
                world.GetBlockState(x, y, z + 1).block != Blocks.Air ||
                world.GetBlockState(x, y, z - 1).block != Blocks.Air)
            {
                 return false;
            }

            return true;
        }
    }
}