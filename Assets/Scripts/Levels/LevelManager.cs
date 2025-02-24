using System;
using DG.Tweening;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level[] _levels;
    private LevelsData _data;
    private IInput _input;
    private Level _loadedLevel;

    private int LevelToIndex => (_data.LevelNumber - 1) % _levels.Length;

    public event Action LevelStarted;
    public event Action LevelCompleted;
    public event Action LevelFailed;

    public void Init(LevelsData data, IInput input)
    {
        _data = data;
        _input = input;
    }

    public void LoadCurrentLevel()
    {
        LoadLevel(LevelToIndex);
    }

    private void OnLevelCompleted()
    {
        LevelCompleted?.Invoke();
        DestroyLoadedLevel();
        LoadNextLevel();
    }

    private void OnLevelFailed()
    {
        LevelFailed?.Invoke();
        DestroyLoadedLevel();
        LoadCurrentLevel();
    }

    private void LoadNextLevel()
    {
        _data.LevelNumber++;
        LoadLevel(LevelToIndex);
    }

    private void LoadLevel(int index)
    {
        _loadedLevel = Instantiate(_levels[index]);
        _loadedLevel.Init(_data.LevelNumber, _input);
        RegisterLevelEventsListeners();
        LevelStarted?.Invoke();
    }

    private void DestroyLoadedLevel()
    {
        UnregisterLevelEventsListeners();
        DOTween.KillAll();
        Destroy(_loadedLevel.gameObject);
    }

    private void RegisterLevelEventsListeners()
    {
        _loadedLevel.Completed += OnLevelCompleted;
        _loadedLevel.Failed += OnLevelFailed;
    }

    private void UnregisterLevelEventsListeners()
    {
        _loadedLevel.Completed -= OnLevelCompleted;
        _loadedLevel.Failed -= OnLevelFailed;
    }
}