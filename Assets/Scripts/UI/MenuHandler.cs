using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [Header("Level Completed Screen")]
    [SerializeField] private GameObject levelCompletedWrapper;
    [SerializeField] private TextMeshProUGUI levelCompletedText;
    [SerializeField] private Button levelCompletedButton;

    public Action OnNextLevelEvent;

    private void OnEnable()
    {
        levelCompletedButton.onClick.AddListener(GoNextLevel);
    }

    public void LevelCompletedScreen(int completedLevel)
    {
        levelCompletedWrapper.SetActive(true);
        levelCompletedText.text = completedLevel.ToString();
    }

    private void GoNextLevel()
    {
        levelCompletedWrapper.SetActive(false);
        OnNextLevelEvent?.Invoke();
    }
}
