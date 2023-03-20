using UnityEngine;
using VoxelEngine.Data;

namespace VoxelEngine.Entities
{
    public class FallingBlock : MonoBehaviour
    {
        public BlockState state;
        public World world;
        public LayerMask layerMask;

        void FixedUpdate()
        {
            if(Physics.Raycast(transform.position, Vector3.down, 0.55f, layerMask))
            {
                Vector3Int blockPos = new Vector3Int(
                    Mathf.FloorToInt(transform.position.x),
                    Mathf.FloorToInt(transform.position.y),
                    Mathf.FloorToInt(transform.position.z)
                );
              
                if(world.GetBlockState(blockPos.x, blockPos.y - 1, blockPos.z).block != Blocks.Air)
                {
                    Destroy(gameObject);
                    world.SetBlockState(blockPos.x, blockPos.y, blockPos.z, state, true);
                }
            }
            else
            {
                Vector3Int blockPos = new Vector3Int(
                    Mathf.FloorToInt(transform.position.x),
                    Mathf.FloorToInt(transform.position.y - 0.55f),
                    Mathf.FloorToInt(transform.position.z)
                );

                if(world.GetBlockState(blockPos.x, blockPos.y, blockPos.z).block != Blocks.Air)
                {
                    Destroy(gameObject);
                    world.SpawnBreakParticle(blockPos.x, blockPos.y + 1, blockPos.z, state);
                }
            }
        }
    }
}
