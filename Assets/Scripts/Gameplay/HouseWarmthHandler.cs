using Logbound.Utilities;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class HouseWarmthHandler : Singleton<HouseWarmthHandler>
    {
        [SerializeField] private Fireplace _fireplace;
        [SerializeField] private float _baseInsulation = 10f; // How much warmer the house is than outside (degrees)
        [SerializeField] private float _fireplaceMaxHeat = 30f; // Max heat contribution from fireplace (degrees)
        
        private float _indoorTemperature;

        public float IndoorTemperature => _indoorTemperature;

        private void Update()
        {
            float outdoorTemperature = WeatherTransitionController.Instance.GetCurrentTemperature();
            
            // Base indoor temperature = outdoor + insulation
            float baseIndoorTemp = outdoorTemperature + _baseInsulation;
            
            // Add fireplace heat contribution based on how much burn time is left
            float fireplaceHeat = 0f;
            if (_fireplace != null)
            {
                fireplaceHeat = _fireplace.GetHeatNormalized() * _fireplaceMaxHeat;
            }
            
            _indoorTemperature = baseIndoorTemp + fireplaceHeat;
        }

        /// <summary>
        /// Returns the current indoor temperature of the house.
        /// </summary>
        public float GetIndoorTemperature()
        {
            return _indoorTemperature;
        }

        /// <summary>
        /// Returns true if the indoor temperature is warm enough (0 or higher).
        /// </summary>
        public bool IsWarm()
        {
            return _indoorTemperature >= 0f;
        }

        /// <summary>
        /// Returns a factor from 0 to 1 representing how cold it is inside.
        /// 0 = warm (0°C or higher), 1 = very cold (-50°C or lower)
        /// </summary>
        public float GetColdFactor()
        {
            return Mathf.Clamp01(-_indoorTemperature / 50f);
        }
    }
}
