using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logbound.UI
{
    public class MainMenu :MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private Button _applyButton;

        private Resolution[] _resolutions;
        private int _selectedIndex = -1;
        public bool FullScreenEnabled { get; set; }

        private void Start()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _quitButton.onClick.AddListener(OnQuitButtonClicked);
            _applyButton.onClick.AddListener(OnApplyClick);
            
            FullScreenEnabled = Screen.fullScreen;
            _resolutions = Screen.resolutions;
            _resolutionDropdown.ClearOptions();

            foreach (Resolution resolution in _resolutions)
            {
                var rate = resolution.refreshRateRatio.numerator / (float)resolution.refreshRateRatio.denominator;
                var text = $"{resolution.width} x {resolution.height} {rate:0.00} Hz";
                _resolutionDropdown.options.Add(new TMP_Dropdown.OptionData
                {
                    text = text
                });
            }
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
            _quitButton.onClick.RemoveListener(OnQuitButtonClicked);
            _applyButton.onClick.RemoveListener(OnApplyClick);
        }

        private void OnQuitButtonClicked()
        {
            Application.Quit();
        }

        private void OnPlayButtonClicked()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }

        public void SelectResolution(int index)
        {
            _selectedIndex = index;
            Debug.Log($"Selected: {_resolutionDropdown.options[index].text}");
        }

        public void OnApplyClick()
        {
            if (_selectedIndex < 0 || _resolutions == null)
            {
                Debug.Log("Skipped options menu apply, no resolution selected");
                return;
            }

            Screen.fullScreen = FullScreenEnabled;
            var res = _resolutions[_selectedIndex];
            Screen.SetResolution(res.width, res.height,
                FullScreenEnabled ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed, res.refreshRateRatio);
        }
    }
}