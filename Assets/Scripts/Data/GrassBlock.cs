using UnityEngine;
using VoxelEngine.Utils;

namespace VoxelEngine.Data
{ 
    public class GrassBlock : Block
    {
        public override int GetTexture(World world, BlockState state, int x, int y, int z, Face face)
        {
            switch(face)
            {
                case Face.YP:
                    return 3;

                case Face.YN:
                    return 1;

                default:
                    return 2;
            }
        } 

        public override Color[] GetBreakColors(World world, BlockState state, int x, int y, int z)
        {
            return new Color[]{
                new Color(0.2f, 0.6f, 0.2f),
                new Color(0.4f, 0.3f, 0.25f)
            };
        }
    }
}