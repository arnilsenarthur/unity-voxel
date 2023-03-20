using UnityEngine;
using VoxelEngine.Data;

namespace VoxelEngine.Utils
{
    public interface IBlockHolder
    {
        BlockState GetBlockState();
        GameObject GetGameObject();
    }
}