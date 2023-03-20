using UnityEngine;
using VoxelEngine.Utils;

namespace VoxelEngine.Data
{ 
    public class DirtBlock : Block
    {
        public override int GetTexture(World world, BlockState state, int x, int y, int z, Face face)
        {
            return 1;
        }

        public override Color[] GetBreakColors(World world, BlockState state, int x, int y, int z)
        {
            return new Color[]{
                new Color(0.2f, 0.1f, 0.05f),
                new Color(0.4f, 0.3f, 0.25f)
            };
        }
    }
}