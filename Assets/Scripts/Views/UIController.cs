using RPSLS.Core;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPSLS.UI
{
    public class UIController : MonoBehaviour
    {
        [Header("UI Text Elements")]
        [SerializeField] private TMP_Text computerChoiceText;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private TMP_Text highScoreResultsText;
        [SerializeField] private TMP_Text currentScoreResultsText;

        [Header("UI Buttons")]
        [SerializeField] private Button startGameBtn;
        [SerializeField] private Button backBtn;
        [SerializeField] private Button exitBtn;

        [Header("UI Panels")]
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject resultsPanel;
        [SerializeField] private GameObject computerPlayed;

        private void Start()
        {
            UpdateHighScoreText(SaveDataManager.GetKey(Constants.HIGH_SCORE_KEY).ToString());
        }

        private void OnEnable()
        {
            startGameBtn.onClick.AddListener(OnPlayClicked);
            backBtn.onClick.AddListener(OnBackClicked);
            exitBtn.onClick.AddListener(ExitToMainMenu);
            Events.EndGame += EndGame;
            ResetGameUI();
        }

        private void OnDisable()
        {
            startGameBtn.onClick.RemoveListener(OnPlayClicked);
            backBtn.onClick.RemoveListener(OnBackClicked);
            exitBtn.onClick.RemoveListener(ExitToMainMenu);
            Events.EndGame -= EndGame;
        }

        private void ExitToMainMenu()
        {
            OnBackClicked();
            ToggleResultsPanelVisibility(false);
        }

        private void EndGame()
        {
            StartCoroutine(AddDelay(2f));
        }

        private IEnumerator AddDelay(float secs)
        {
            yield return new WaitForSeconds(secs);
            ShowResultsPanel();
        }

        private void OnPlayClicked()
        {
            startPanel.SetActive(false);
            gamePanel.SetActive(true);
            ResetGameUI();
            Events.StartGame?.Invoke();
        }

        private void OnBackClicked()
        {
            startPanel.SetActive(true);
            gamePanel.SetActive(false);
            Events.OnBack?.Invoke();
        }

        // Update UI Elements
        public void UpdateScoreText()
        {
            scoreText.text = $"{Constants.SCORE}{ScoreController.PlayerScore}";
        }

        public void UpdateComputerChoiceText(string value)
        {
            computerChoiceText.text = value;
        }

        public void UpdateHighScoreText(string value)
        {
            highScoreText.text = $"{Constants.HIGH_SCORE}{value}";
        }

        public void UpdateResultsText(string value)
        {
            resultText.text = value;
        }

        // Toggle UI Visibility
        public void ToggleComputerPlayedTextVisibility(bool value)
        {
            computerPlayed.SetActive(value);
            computerChoiceText.gameObject.SetActive(value);
        }

        public void ToggleResultsTextVisibility(bool value)
        {
            resultText.gameObject.SetActive(value);
        }

        public void ToggleResultsPanelVisibility(bool value)
        {
            resultsPanel.SetActive(value);
        }

        public void ShowResultsPanel()
        {
            ToggleResultsPanelVisibility(true);
            var score = SaveDataManager.GetKey(Constants.HIGH_SCORE_KEY).ToString();
            highScoreResultsText.text = score;
            currentScoreResultsText.text = ScoreController.PlayerScore.ToString();
            UpdateHighScoreText(score);
        }

        // Reset Game UI
        public void ResetGameUI()
        {
            scoreText.text = $"{Constants.SCORE}0";
            UpdateComputerChoiceText(string.Empty);
            ToggleComputerPlayedTextVisibility(false);
            ToggleResultsTextVisibility(false);
            ToggleResultsPanelVisibility(false);
        }
    }
}
