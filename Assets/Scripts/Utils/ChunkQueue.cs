using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine.Utils
{
    [Serializable]
    public class Queue<T>
    {
        private Material _material;
        #if UNITY_EDITOR
        [SerializeField, HideInInspector]
        private List<T> _renderQueue = new List<T>();
        #else
        private System.Collections.Generic.Queue<T> _renderQueue = new System.Collections.Generic.Queue<T>();
        #endif

        public void Enqueue(T chunk)
        {
            if(!_renderQueue.Contains(chunk))
            #if UNITY_EDITOR
                _renderQueue.Add(chunk);
            #else
            _renderQueue.Enqueue(chunk);
            #endif
        }

        public bool HasQueue()
        {
            return _renderQueue.Count > 0;
        }

        public T Dequeue()
        {
            #if UNITY_EDITOR
            T c = _renderQueue[0];
            _renderQueue.RemoveAt(0);
            return c;
            #else
            return _renderQueue.Dequeue();
            #endif
        }

        public virtual bool Tick() => false;
    }
}