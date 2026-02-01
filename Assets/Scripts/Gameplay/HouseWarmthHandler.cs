using Logbound.Utilities;
using UnityEngine;

namespace Logbound.Gameplay
{
    public class HouseWarmthHandler : Singleton<HouseWarmthHandler>
    {
        [SerializeField] private Fireplace _fireplace;
        [SerializeField] private Door[] _doors;
        [SerializeField] private float _baseInsulation = 10f; // How much warmer the house is than outside (degrees)
        [SerializeField] private float _fireplaceMaxHeat = 30f; // Max heat contribution from fireplace (degrees)
        [SerializeField] private float _maxDoorHeatLoss = 15f; // Max heat loss when all doors are open (degrees)
        
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
            
            // Calculate heat loss from open doors
            float doorHeatLoss = CalculateDoorHeatLoss();
            
            _indoorTemperature = baseIndoorTemp + fireplaceHeat - doorHeatLoss;
        }
        
        private float CalculateDoorHeatLoss()
        {
            if (_doors == null || _doors.Length == 0)
                return 0f;
            
            int openDoorCount = 0;
            int totalDoorsAffectingWarmth = 0;
            
            foreach (Door door in _doors)
            {
                if (door != null && door.AffectsHouseWarmth)
                {
                    totalDoorsAffectingWarmth++;
                    if (door.IsOpen)
                    {
                        openDoorCount++;
                    }
                }
            }
            
            if (totalDoorsAffectingWarmth == 0)
                return 0f;
            
            // Calculate heat loss as a proportion of doors that affect warmth
            float openDoorRatio = (float)openDoorCount / totalDoorsAffectingWarmth;
            return openDoorRatio * _maxDoorHeatLoss;
        }
    }
}
