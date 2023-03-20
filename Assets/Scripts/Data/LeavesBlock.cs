using UnityEngine;
using VoxelEngine.Utils;

namespace VoxelEngine.Data
{ 
    public class LeavesBlock : Block
    {
        public override bool IsOpaque(World world, BlockState state, int x, int y, int z)
        {
            return false;
        }
        
        public override int GetTexture(World world, BlockState state, int x, int y, int z, Face face)
        {
            return 5;
        }
        
        public override Color[] GetBreakColors(World world, BlockState state, int x, int y, int z)
        {
            return new Color[]{
                new Color(0f, 0.5f, 0f),
                new Color(0.4f, 0.8f, 0.4f)
            };
        }
    }
}