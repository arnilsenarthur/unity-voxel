using UnityEngine;

namespace VoxelEngine.Data
{ 
    public class AirBlock : Block
    {
        public override bool IsOpaque(World world, BlockState state, int x, int y, int z)
        {
            return false;
        }

        public override bool ShouldRender(World world, BlockState state, int x, int y, int z)
        {
            return false;
        }
    }
}