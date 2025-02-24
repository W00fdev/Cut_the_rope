using DG.Tweening;
using UnityEngine;

public class Toy : MonoBehaviour
{
    public Sequence MoveTo(Transform target, float time)
    {
        transform.SetParent(target);
        return DOTween.Sequence()
            .Append(transform.DOMove(target.position, time))
            .Join(transform.DOScale(Vector3.one, time))
            .Join(transform.DORotateQuaternion(target.rotation, time));
    }

    public void RotateAround()
    {
        Vector3 oldRotation = transform.localEulerAngles;
        var newRotation = new Vector3(oldRotation.x, oldRotation.y + 180f, oldRotation.z);
        transform.DOLocalRotate(newRotation, 1f)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
    }
}