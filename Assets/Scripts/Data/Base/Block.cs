using UnityEngine;
using VoxelEngine.Utils;

namespace VoxelEngine.Data
{ 
    public class Block
    {
        public int id = 0;

        public virtual bool IsOpaque(World world, BlockState state, int x, int y, int z)
        {
            return true;
        }

        public virtual bool ShouldRender(World world, BlockState state, int x, int y, int z)
        {
            return true;
        }

        public virtual int GetTexture(World world, BlockState state, int x, int y, int z, Face face)
        {
            return 0;
        }

        public virtual int GetBlockModel(World world, BlockState state, int x, int y, int z)
        {
            return 0;
        }

        public virtual Vector3Int GetBlockPlacePosition(World world, BlockState state, int x, int y, int z, Vector3Int offset)
        {
            return new Vector3Int(x, y, z) + offset;
        }

        public virtual void UpdateBlockState(World world, BlockState state, int x, int y, int z)
        {
            
        }

        public virtual Color[] GetBreakColors(World world, BlockState state, int x, int y, int z)
        {
            Color c = GetBreakColor(world, state, x, y, z);
            return new Color[] {c, c};
        }

        public virtual Color GetBreakColor(World world, BlockState state, int x, int y, int z)
        {
            return Color.white;
        }

        public virtual bool CanPlaceBlock(World world, BlockState state, int x, int y, int z)
        {
            return true;
        } 
    }
}