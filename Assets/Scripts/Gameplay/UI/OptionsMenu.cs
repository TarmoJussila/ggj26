using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logbound.Gameplay.UI
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private Toggle _invertVerticalLookToggle;
        [SerializeField] private TextMeshProUGUI _toggleLabel;
        [SerializeField] private TextMeshProUGUI _wallOfText;

        public bool InvertVerticalLook { get; private set; }

        private SplitScreenPlayer _player;

        private void OnEnable()
        {
            _invertVerticalLookToggle.isOn = InvertVerticalLook;
        }

        private void Awake()
        {
            _player = transform.parent.GetComponent<SplitScreenPlayer>();
            if (_player == null)
            {
                Debug.LogError("Failed to find SplitScreenPlayer ref");
            }
        }

        private void Start()
        {
            _invertVerticalLookToggle.onValueChanged.AddListener(OnInvertVerticalLookChanged);
            if (_player == null)
            {
                Debug.LogError("Failed to find SplitScreenPlayer ref");
                return;
            }
            if (_player.MouseInput)
            {
                _toggleLabel.text = "Press Enter to toggle inverted vertical look axis";
                _wallOfText.text = @"
Move - WASD / Arrow keys
Look - Mouse
Jump - Space
Interact / Pick up item - E
Use carried item - Mouse 1
Drop item - G
Throw item - F
Quit to main menu - R (hold)
Options menu - ESC";
            }
            else
            {
                _toggleLabel.text = "Press A to toggle inverted vertical look axis";
                _wallOfText.text = @"
Move - Left stick
Look - Right stick
Jump - A
Interact / Pick up item - X
Use carried item - Right trigger / Right shoulder
Drop item - B
Throw item - Y
Quit to main menu - Left shoulder (hold)
Options menu - Start";
            }
        }

        private void OnDestroy()
        {
            _invertVerticalLookToggle.onValueChanged.RemoveListener(OnInvertVerticalLookChanged);
        }

        private void OnInvertVerticalLookChanged(bool arg0)
        {
            InvertVerticalLook = arg0;
        }
    }
}