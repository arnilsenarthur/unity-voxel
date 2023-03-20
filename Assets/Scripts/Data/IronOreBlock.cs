using UnityEngine;
using VoxelEngine.Utils;

namespace VoxelEngine.Data
{ 
    public class IronOreBlock : Block
    {
        public override int GetTexture(World world, BlockState state, int x, int y, int z, Face face)
        {
            return 17;
        }

        public override Color[] GetBreakColors(World world, BlockState state, int x, int y, int z)
        {
            return new Color[]{
                new Color(0.8f, 0.65f, 0.55f),
                new Color(0.7f, 0.7f, 0.7f)
            };
        }
    }
}