using UnityEngine;
using VoxelEngine.Data;

namespace VoxelEngine.Utils
{
    public class InteractionController : MonoBehaviour, IBlockHolder
    {
        [Header("REFERENCES")]
        public World world;
        public new Transform camera;
        public Transform displaySelect;
        public GameObject holdingRenderer;
        public Transform arm;

        [Header("SETTINGS")]
        public LayerMask blockRaycast;

        [Header("INVENTORY")]
        private BlockState[] states = 
        {
            new BlockState{ block = Blocks.Bedrock},
            new BlockState{ block = Blocks.Stone},
            new BlockState{ block = Blocks.Dirt},
            new BlockState{ block = Blocks.Grass},
            new BlockState{ block = Blocks.Sand},
            new BlockState{ block = Blocks.Log},
            new BlockState{ block = Blocks.Leaves},
            new BlockState{ block = Blocks.Cactus},
            new BlockState{ block = Blocks.DeadBush},
            new BlockState{ block = Blocks.Flower, data = 0},
            new BlockState{ block = Blocks.Flower, data = 1},
            new BlockState{ block = Blocks.CoalOre},
            new BlockState{ block = Blocks.IronOre},

            new BlockState{ block = Blocks.Glass},
            new BlockState{ block = Blocks.Plank}
        };

        [Header("STATE")]
        public int stateIndex = 1;
        public float armSwing = 0;

        public void Update()
        {
            Vector3 rcOrigin = camera.transform.position;
            Vector3 rcDirection = camera.transform.forward;

            if (Physics.Raycast(rcOrigin, rcDirection, out RaycastHit hit, 5f, blockRaycast))
            {
                Vector3 position = hit.point - hit.normal * 0.1f;
                Vector3Int blockPos = new Vector3Int(
                    Mathf.FloorToInt(position.x),
                    Mathf.FloorToInt(position.y),
                    Mathf.FloorToInt(position.z)
                );

                displaySelect.transform.position = blockPos + new Vector3(0.5f, 0.5f, 0.5f);

                if(Input.GetMouseButtonDown(0))
                {
                    BreakBlock(blockPos);
                }   
                else if(Input.GetMouseButtonDown(1))
                {
                    BlockState state = world.GetBlockState(blockPos.x, blockPos.y, blockPos.z);
                    blockPos = state.block.GetBlockPlacePosition(world, state, blockPos.x, blockPos.y, blockPos.z , Math.GetDirection(hit.normal.normalized));
                    
                    PlaceBlock(blockPos);
                }
            }
            else
            {
                displaySelect.transform.position = rcOrigin - rcDirection * 10f;
            }

            stateIndex += (int) Input.mouseScrollDelta.y;
            if(stateIndex < 0)
                stateIndex = states.Length - 1;
            else if(stateIndex >= states.Length)
                stateIndex = 0; 

            arm.localEulerAngles = new Vector3(
                Mathf.LerpAngle(
                    arm.localEulerAngles.x,
                    armSwing,
                    Time.deltaTime * 20f
                ), 0, 0);

            armSwing = Mathf.Lerp(
                armSwing,
                0,
                Time.deltaTime * 20f
            );
        }

        public void FixedUpdate()
        {
            world.worldRenderer.Render(holdingRenderer, this);
        }

        public void BreakBlock(Vector3Int blockPos)
        {
            armSwing = -30;
            world.BreakBlock(blockPos.x, blockPos.y, blockPos.z);
        }

        public void PlaceBlock(Vector3Int blockPos)
        {
            armSwing = 50;
            BlockState state = states[stateIndex];
            
            if(state.block.CanPlaceBlock(world, state, blockPos.x, blockPos.y, blockPos.z))
            {
                world.SetBlockState(blockPos.x, blockPos.y, blockPos.z, state, true);
                world.UpdateBlock(blockPos.x, blockPos.y, blockPos.z);
            }
        }

        public BlockState GetBlockState()
        {
            return states[stateIndex];
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}