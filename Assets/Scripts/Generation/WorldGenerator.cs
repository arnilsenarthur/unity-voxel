using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine.Generation
{
    [Serializable]
    public abstract class WorldGenerator : VoxelEngine.Utils.Queue<Chunk>
    {
        public World world;

        [SerializeField, HideInInspector]
        private List<Chunk> _toDecorate = new List<Chunk>();

        #region Generation
        public void Generate(Chunk chunk)
        {
            OnGenerate(chunk);
            
            _toDecorate.Add(chunk);
        }
        #endregion

        #region Decoration
        public void Decorate()
        {
            for(int i = 0; i < _toDecorate.Count; i ++)
            {
                Chunk c = _toDecorate[i];

                if(CanDecorate(c))
                {
                    _toDecorate.RemoveAt(i);
                    i --;

                    world.worldRenderer.Render(c);
                    world.worldRenderer.RenderNeighbours(c);
                }
            }  
        }

        public bool CanDecorate(Chunk chunk)
        {
            if (chunk.decorated) return true;

            Vector2Int position = chunk.position;

            if (world.GetChunk(new Vector2Int(position.x + 1, position.y)) == null)
                return false;

            if (world.GetChunk(new Vector2Int(position.x - 1, position.y)) == null)
                return false;

            if (world.GetChunk(new Vector2Int(position.x, position.y + 1)) == null)
                return false;

            if (world.GetChunk(new Vector2Int(position.x, position.y - 1)) == null)
                return false;

            if (world.GetChunk(new Vector2Int(position.x + 1, position.y + 1)) == null)
                return false;

            if (world.GetChunk(new Vector2Int(position.x + 1, position.y - 1)) == null)
                return false;

            if (world.GetChunk(new Vector2Int(position.x - 1, position.y + 1)) == null)
                return false;

            if (world.GetChunk(new Vector2Int(position.x - 1, position.y - 1)) == null)
                return false;

            OnDecorate(chunk);

            chunk.decorated = true;

            return true;
        }
        #endregion

        public override bool Tick()
        {
            Decorate();

            if(HasQueue())
            {
                Generate(Dequeue());
                return true;
            }

            return false;
        }

        #region Abstract Methods
        public abstract void OnGenerate(Chunk chunk);
        public abstract void OnDecorate(Chunk chunk);
        #endregion
    }
}