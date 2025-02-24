using System;
using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Utilities.Coroutines
{
    public sealed class CoroutineObject<T> : CoroutineObjectBase
    {
        public CoroutineObject(MonoBehaviour owner, Func<T, IEnumerator> routine)
        {
            Owner = owner;
            Routine = routine;
        }

        private Func<T, IEnumerator> Routine { get; }

        public override event Action Finished;

        private IEnumerator Process(T arg)
        {
            yield return Routine.Invoke(arg);
            Coroutine = null;
            Finished?.Invoke();
        }

        public void Start(T arg)
        {
            Stop();
            Coroutine = Owner.StartCoroutine(Process(arg));
        }
    }
}