using System;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    [SerializeField] private WinPanel _winPanel;
    [SerializeField] private LosePanel _losePanel;
    [SerializeField] private LevelNumber _levelNumber;
    [SerializeField] private RopesPanel _ropesPanel;
    [SerializeField] private ScissorsView _scissorsView;
    [SerializeField] private Hand _hand;

    public void Init(int levelNumber, IInput input, Action onContinueButtonClickedAction,
        Action onRestartButtonClickedAction)
    {
        _levelNumber.SetValue(levelNumber);
        _ropesPanel.CreateElements();
        _winPanel.Init(onContinueButtonClickedAction);
        _losePanel.Init(onRestartButtonClickedAction);
        _scissorsView.Init(input);
        _hand.Init(input);
    }

    public void ShowWinPanel()
    {
        _ropesPanel.Hide(1f);
        _winPanel.Show(1f);
    }

    public void ShowLosePanel()
    {
        _ropesPanel.Hide(.25f);
        _losePanel.Show(.5f);
    }

    public void MarkRopesPanelElementAsCompleted()
    {
        _ropesPanel.MarkElementAsCompleted();
    }
}