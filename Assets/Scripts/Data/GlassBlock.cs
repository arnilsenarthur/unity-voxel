using UnityEngine;
using VoxelEngine.Utils;

namespace VoxelEngine.Data
{ 
    public class GlassBlock : Block
    {
        public override bool IsOpaque(World world, BlockState state, int x, int y, int z)
        {
            return false;
        }
        
        public override int GetTexture(World world, BlockState state, int x, int y, int z, Face face)
        {
            return 14;
        }
        
        public override Color[] GetBreakColors(World world, BlockState state, int x, int y, int z)
        {
            return new Color[]{
                new Color(0.8f, 0.8f, 1f),
                new Color(1f, 1f, 1f)
            };
        }
    }
}