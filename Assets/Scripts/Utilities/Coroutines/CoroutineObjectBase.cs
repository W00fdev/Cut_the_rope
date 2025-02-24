using System;
using UnityEngine;

namespace _Project.Scripts.Utilities.Coroutines
{
    public abstract class CoroutineObjectBase
    {
        protected MonoBehaviour Owner { get; set; }
        protected Coroutine Coroutine { get; set; }

        public bool IsProcessing => Coroutine != null;

        public abstract event Action Finished;

        public void Stop()
        {
            Owner.StopCoroutine(Coroutine);
            Coroutine = null;
        }
    }
}