using System;
using VoxelEngine.Data;

namespace VoxelEngine
{
    [Serializable]
    public struct BlockState
    {
        public static BlockState Empty => default(BlockState);

        public int id;  
        public Block block 
        {
            get => Blocks.BLOCKS[id];
            set => id = value == null ? 0 : value.id;
        }

        public byte data;
    }
}