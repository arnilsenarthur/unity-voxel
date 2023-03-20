using UnityEngine;

namespace VoxelEngine.Data
{ 
    public class CrossBlock : Block
    {
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
    }
}