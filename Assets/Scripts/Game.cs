using HomaGames.HomaBelly;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private DefaultInput _input;
    [SerializeField] private LevelManager _levelManager;
    private GameData _gameData;
    private ISaveLoadService _saveLoadService;

    private void Awake()
    {
        Init();
        StartGame();
    }

    private void Init()
    {
        _saveLoadService = new JsonSaveLoadService();
        _gameData = new GameData(nameof(GameData));

        _saveLoadService.Load(_gameData);
        _levelManager.Init(_gameData.LevelsData, _input);
        _levelManager.LevelStarted += OnLevelStarted;
        _levelManager.LevelCompleted += OnLevelCompleted;
        _levelManager.LevelFailed += OnLevelFailed;
    }

    private void StartGame()
    {
        _levelManager.LoadCurrentLevel();
    }

    private void OnLevelStarted()
    {
        if (_saveLoadService.HasChanges(_gameData))
            _saveLoadService.Save(_gameData);
        HomaBelly.Instance.TrackProgressionEvent(ProgressionStatus.Start,
            $"level_{_gameData.LevelsData.LevelNumber}");
    }

    private void OnLevelCompleted()
    {
        HomaBelly.Instance.TrackProgressionEvent(ProgressionStatus.Complete,
            $"level_{_gameData.LevelsData.LevelNumber}");
    }
    
    private void OnLevelFailed()
    {
        HomaBelly.Instance.TrackProgressionEvent(ProgressionStatus.Fail,
            $"level_{_gameData.LevelsData.LevelNumber}");
    }
}