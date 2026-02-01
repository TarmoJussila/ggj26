using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Logbound.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private Button _applyButton;
        [SerializeField] private Toggle _fullscreenToggle;

        private Resolution[] _resolutions;
        private int _selectedIndex = -1;
        public bool FullScreenEnabled { get; set; }

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        private void Start()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _quitButton.onClick.AddListener(OnQuitButtonClicked);
            _applyButton.onClick.AddListener(OnApplyClick);
            _fullscreenToggle.onValueChanged.AddListener(delegate
            {
                ToggleValueChanged(_fullscreenToggle);
            });

            FullScreenEnabled = _fullscreenToggle.isOn;
            var temp = new List<Resolution>();
            _resolutionDropdown.ClearOptions();
            _resolutionDropdown.onValueChanged.AddListener(SelectResolution);

            foreach (var resolution in Screen.resolutions)
            {
                var rate = resolution.refreshRateRatio.numerator / (float)resolution.refreshRateRatio.denominator;
                var text = $"{resolution.width} x {resolution.height} {rate:0.00} Hz";

                if (resolution.height < 720)
                {
                    Debug.Log($"Skipped too small resolution: {text}");
                    continue;
                }
                
                if (resolution.width / (float)resolution.height < 1920f / 1200f)
                {
                    Debug.Log($"Skipped too narrow resolution: {text}");
                    continue;
                }

                _resolutionDropdown.options.Add(new TMP_Dropdown.OptionData
                {
                    text = text
                });
                temp.Add(resolution);
            }
            _resolutions = temp.ToArray();
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
            _quitButton.onClick.RemoveListener(OnQuitButtonClicked);
            _applyButton.onClick.RemoveListener(OnApplyClick);
            _resolutionDropdown.onValueChanged.RemoveListener(SelectResolution);
            _fullscreenToggle.onValueChanged.RemoveAllListeners();
        }

        private void OnQuitButtonClicked()
        {
            Application.Quit();
        }

        private void OnPlayButtonClicked()
        {
            SceneManager.LoadScene("GameScene");
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
                FullScreenEnabled ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed, res.refreshRateRatio);
            Debug.Log($"Applying resolution: {res.width}x{res.height}, {res.refreshRateRatio}Hz, fullscreen {FullScreenEnabled}");
        }

        private void ToggleValueChanged(Toggle fullscreenToggle)
        {
            Debug.Log($"Fullscreen: {fullscreenToggle.isOn}");
            FullScreenEnabled = fullscreenToggle.isOn;
        }
    }
}