using DG.Tweening;
using UnityEngine;

public class ToyFinal : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _toyParticles;
    [SerializeField] private CanvasGroup _toyBackground;
    private Overlay _overlay;
    private Toy _toy;

    public void Init(Toy toy, Overlay overlay)
    {
        _toy = toy;
        _overlay = overlay;
    }

    public void AnimateToy()
    {
        const float duration = 1f;
        _toy.MoveTo(transform, duration).OnComplete(() =>
        {
            foreach (ParticleSystem toyParticle in _toyParticles)
                toyParticle.Play();
            _toy.RotateAround();
            _overlay.ShowWinPanel();
        });
        _toyBackground.DOFade(1f, duration * 2f);
    }
}