using UnityEngine;
using VoxelEngine.Utils;

namespace VoxelEngine.Data
{ 
    public class PlankBlock : Block
    {        
        public override int GetTexture(World world, BlockState state, int x, int y, int z, Face face)
        {
            return 15;
        }
        
        public override Color[] GetBreakColors(World world, BlockState state, int x, int y, int z)
        {
            return new Color[]{
                new Color(0.4f, 0.2f, 0.2f),
                new Color(0.6f, 0.5f, 0.4f)
            };
        }
    }
}