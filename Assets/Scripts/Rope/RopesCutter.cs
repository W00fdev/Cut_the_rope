using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Obi;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RopesCutter : AbstractRopesCutter
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraFailReaction _cameraFailReaction;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private List<BakedRope> _ropes;
    [SerializeField] private Volume _volume;
    [SerializeField] private Image _rewindImage;
    [SerializeField] private Knife _knife;
    [SerializeField] private Box _box;
    [SerializeField] private RopesCutterSettings _settings;

    private Vector3 _cutStartPosition;
    private IInput _input;
    private Overlay _overlay;

    public override IReadOnlyList<BakedRope> Ropes => _ropes;

    private void OnValidate()
    {
        if (_camera == null)
            _camera = Camera.main;

        if (_cameraFailReaction == null)
            _cameraFailReaction = FindObjectOfType<CameraFailReaction>();

        if (_volume == null)
            _volume = FindObjectOfType<Volume>();

        if (_knife == null)
            _knife = FindObjectOfType<Knife>();

        if (_box == null)
            _box = FindObjectOfType<Box>();
    }

    private void OnDestroy()
    {
        UnsubscribeFromInputEvents();
    }

    public override event Action RopeCut;
    public override event Action AllRopesCut;

    public override void Init(IInput input, Overlay overlay)
    {
        _input = input;
        _overlay = overlay;
        foreach (BakedRope bakedRope in _ropes)
            bakedRope.Init();
        SubscribeToInputEvents();
    }

    private void SubscribeToInputEvents()
    {
        _input.ButtonMouseDown += StartDrawingLine;
        _input.ButtonMouseHold += DrawLine;
        _input.ButtonMouseUp += TryCut;
    }

    private void UnsubscribeFromInputEvents()
    {
        _input.ButtonMouseDown -= StartDrawingLine;
        _input.ButtonMouseHold -= DrawLine;
        _input.ButtonMouseUp -= TryCut;
    }

    private void StartDrawingLine()
    {
        _cutStartPosition = _input.MousePosition;
        Vector3 worldPointPosition =
            _camera.ScreenToWorldPoint(new Vector3(_cutStartPosition.x, _cutStartPosition.y, 0.5f));
        _lineRenderer.SetPosition(0, worldPointPosition);
        _lineRenderer.SetPosition(1, worldPointPosition);
        _lineRenderer.enabled = true;
    }

    private void DrawLine()
    {
        if (!_lineRenderer.enabled) return;

        _lineRenderer.SetPosition(1,
            _camera.ScreenToWorldPoint(new Vector3(_input.MousePosition.x, _input.MousePosition.y, 0.5f)));
    }

    private void StopDrawingLine()
    {
        _lineRenderer.enabled = false;
    }

    private BakedRope FindRopeToCut(Vector2 lineStart, Vector2 lineEnd,
        out int elementToCutIndex, out Vector3 cutPosition)
    {
        BakedRope ropeToCut = null;
        cutPosition = default;
        elementToCutIndex = -1;

        foreach (BakedRope bakedRope in _ropes)
        {
            ObiRope obiRope = bakedRope.Rope;
            obiRope.gameObject.SetActive(true);

            for (var i = 0; i < obiRope.elements.Count; i++)
            {
                ObiStructuralElement element = obiRope.elements[i];
                Vector3 screenPos1 = _camera.WorldToScreenPoint(obiRope.solver.positions[element.particle1]);
                Vector3 screenPos2 = _camera.WorldToScreenPoint(obiRope.solver.positions[element.particle2]);

                if (!SegmentSegmentIntersection(screenPos1, screenPos2, lineStart, lineEnd, out float _, out float _))
                    continue;

                cutPosition = (obiRope.solver.positions[element.particle2] +
                               obiRope.solver.positions[element.particle1]) / 2;
                float distanceToRope = Vector3.SqrMagnitude(_camera.transform.position - cutPosition);
                float distanceToHit = float.PositiveInfinity;

                if (Physics.Raycast(_camera.transform.position, (cutPosition - _camera.transform.position).normalized,
                        out RaycastHit hit))
                    distanceToHit = Vector3.SqrMagnitude(_camera.transform.position - hit.point);

                if (distanceToRope >= distanceToHit) continue;

                ropeToCut = bakedRope;
                elementToCutIndex = i;
                break;
            }

            obiRope.gameObject.SetActive(false);

            if (ropeToCut != null)
                break;
        }

        return ropeToCut;
    }

    private void TryCut()
    {
        if (!_lineRenderer.enabled) return;

        StopDrawingLine();
        StartCoroutine(ScreenSpaceCut(_cutStartPosition, _input.MousePosition));
    }

    private IEnumerator ScreenSpaceCut(Vector2 lineStart, Vector2 lineEnd)
    {
        BakedRope ropeToCut = FindRopeToCut(lineStart, lineEnd, out int elementToCutIndex, out Vector3 cutPosition);
        var cutDelay = new WaitForSeconds(_settings.KnifeCutDuration);
        var lastCutDelay = new WaitForSeconds(_settings.KnifeCutDuration * 1.6753251144f);

        if (ropeToCut == null)
        {
            _cameraFailReaction.Play();
            yield break;
        }

        _input.Disable();
        float cutDuration = _ropes.Count == 1 ? _settings.KnifeCutDuration * 1.6753251144f : _settings.KnifeCutDuration;

        ShowKnife(cutPosition, ropeToCut.Color, cutDuration);

        if (_ropes.Count == 1)
            yield return lastCutDelay;
        else
            yield return cutDelay;
        _knife.Hide();

        if (IsRopeToCutRight(ropeToCut))
        {
            yield return CutRope(ropeToCut, elementToCutIndex);
            _input.Enable();
        }
        else
        {
            //_rewindImage.enabled = true;
            //_volume.enabled = true;
            _cameraFailReaction.Play();
            _overlay.ShowLosePanel();
            yield return FakeCutRope(ropeToCut, elementToCutIndex);
            //_rewindImage.enabled = false;
            //_volume.enabled = false;
            _input.Disable();
        }
    }

    private IEnumerator FakeCutRope(BakedRope ropeToCut, int elementToCutIndex)
    {
        BakedRope clonedRope = Instantiate(ropeToCut, ropeToCut.transform.parent);
        ropeToCut.HideMesh();
        clonedRope.Rope.gameObject.SetActive(true);
        //clonedRope.StartRecording();
        clonedRope.Rope.Tear(clonedRope.Rope.elements[elementToCutIndex]);
        clonedRope.Rope.RebuildConstraintsFromElements();
        EnableRopesCollidersExcept(ropeToCut);
        yield return null;
        clonedRope.HideMesh();
        /*Time.timeScale = _settings.SlowMotionTimeScale;
        yield return new WaitForSeconds(_settings.RewindTime);
        clonedRope.Rewind();
        yield return new WaitForSeconds(_settings.RewindTime + 0.1f);
        DisableRopesCollidersExcept(ropeToCut);
        ropeToCut.ShowMesh();
        Destroy(clonedRope.gameObject);
        Time.timeScale = 1f;*/
    }

    private IEnumerator CutRope(BakedRope ropeToCut, int elementToCutIndex)
    {
        DOTween.Sequence().AppendInterval(3f).OnComplete(() => ropeToCut.gameObject.SetActive(false));

        ropeToCut.Rope.gameObject.SetActive(true);
        ropeToCut.Rope.Tear(ropeToCut.Rope.elements[elementToCutIndex]);
        ropeToCut.Rope.RebuildConstraintsFromElements();
        ropeToCut.DisableColliders();
        _ropes.Remove(ropeToCut);

        yield return null;

        ropeToCut.HideMesh();
        RopeCut?.Invoke();

        if (_ropes.Count == 0)
        {
            AllRopesCut?.Invoke();
            enabled = false;
        }
    }

    private bool IsRopeToCutRight(BakedRope rope) => rope == _ropes[0];

    private void ShowKnife(Vector3 cutPosition, Color effectColor, float cutDuration)
    {
        const float knifeIndent = 0.23f;

        Vector3 boxToCutDirection = (cutPosition - _box.transform.position).normalized;
        Vector3 closestPoint = _box.GetClosestPoint(cutPosition);
        Vector3 spawnPosition = cutPosition + boxToCutDirection * knifeIndent;
        Vector3 rightRotation = (closestPoint - cutPosition).normalized;
        _knife.Show(spawnPosition, rightRotation, effectColor, cutDuration);
    }

    private void EnableRopesCollidersExcept(BakedRope ropeToCut)
    {
        foreach (BakedRope bakedRope in _ropes.Where(bakedRope => bakedRope != ropeToCut))
            bakedRope.EnableColliders();
    }

    private void DisableRopesCollidersExcept(BakedRope ropeToCut)
    {
        foreach (BakedRope bakedRope in _ropes.Where(bakedRope => bakedRope != ropeToCut))
            bakedRope.DisableColliders();
    }

    /**
     * line segment 1 is AB = A+r(B-A)
     * line segment 2 is CD = C+s(D-C)
     * if they intesect, then A+r(B-A) = C+s(D-C), solving for r and s gives the formula below.
     * If both r and s are in the 0,1 range, it meant the segments intersect.
     */
    private bool SegmentSegmentIntersection(Vector2 A, Vector2 B, Vector2 C, Vector2 D, out float r, out float s)
    {
        float denom = (B.x - A.x) * (D.y - C.y) - (B.y - A.y) * (D.x - C.x);
        float rNum = (A.y - C.y) * (D.x - C.x) - (A.x - C.x) * (D.y - C.y);
        float sNum = (A.y - C.y) * (B.x - A.x) - (A.x - C.x) * (B.y - A.y);

        if (Mathf.Approximately(rNum, 0) || Mathf.Approximately(denom, 0))
        {
            r = -1;
            s = -1;
            return false;
        }

        r = rNum / denom;
        s = sNum / denom;

        return r is >= 0 and <= 1 && s is >= 0 and <= 1;
    }

#if UNITY_EDITOR
    [ContextMenu(nameof(FindRopes))]
    private void FindRopes()
    {
        _ropes = FindObjectsOfType<BakedRope>().ToList();
    }
#endif
}