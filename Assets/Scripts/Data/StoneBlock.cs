using UnityEngine;

namespace VoxelEngine.Data
{ 
    public class StoneBlock : Block
    {
        public override Color[] GetBreakColors(World world, BlockState state, int x, int y, int z)
        {
            return new Color[]{
                new Color(0.3f, 0.3f, 0.3f),
                new Color(0.7f, 0.7f, 0.7f)
            };
        }
    }
}