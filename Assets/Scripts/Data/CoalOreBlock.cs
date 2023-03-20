using UnityEngine;
using VoxelEngine.Utils;

namespace VoxelEngine.Data
{ 
    public class CoalOreBlock : Block
    {
        public override int GetTexture(World world, BlockState state, int x, int y, int z, Face face)
        {
            return 18;
        }

        public override Color[] GetBreakColors(World world, BlockState state, int x, int y, int z)
        {
            return new Color[]{
                new Color(0f, 0f, 0f),
                new Color(0.7f, 0.7f, 0.7f)
            };
        }
    }
}