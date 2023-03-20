using UnityEngine;
using VoxelEngine.Utils;

namespace VoxelEngine.Data
{ 
    public class FlowerBlock : CrossBlock
    {
        public override int GetTexture(World world, BlockState state, int x, int y, int z, Face face)
        {
            return state.data == 0 ? 12 : 13;
        }

        public override int GetBlockModel(World world, BlockState state, int x, int y, int z)
        {
            return 2;
        }

        public override bool IsOpaque(World world, BlockState state, int x, int y, int z)
        {
            return false;
        }

        public override Vector3Int GetBlockPlacePosition(World world, BlockState state, int x, int y, int z, Vector3Int offset)
        {
            return new Vector3Int(x, y, z);
        }

        public override Color[] GetBreakColors(World world, BlockState state, int x, int y, int z)
        {
            return new Color[]{
                state.data == 0 ? Color.red : Color.yellow,
                new Color(0.2f, 0.6f, 0.2f)
            };
        }

        public override void UpdateBlockState(World world, BlockState state, int x, int y, int z)
        {
            BlockState bellow = world.GetBlockState(x, y - 1, z);

            if(bellow.block != Blocks.Grass)
            {
                world.BreakBlock(x, y, z);
            }
        }

        public override bool CanPlaceBlock(World world, BlockState state, int x, int y, int z)
        {
            BlockState bellow = world.GetBlockState(x, y - 1, z);

            return bellow.block == Blocks.Grass;
        }
    }
}