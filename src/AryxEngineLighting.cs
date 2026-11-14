using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Aryx_F16M_KingViper
{
    public sealed class AryxEngineLighting : MonoBehaviour
    {
        [SerializeField] private Light[] lights;
        [SerializeField] private JetNozzle nozzle;
        [SerializeField] private Turbojet turbojet;
        [Space]
        [SerializeField] private float intensityAtMaxThrust;
        [SerializeField] private float intensityAtMinThrust;
        [SerializeField] private float minThrustToLight = 120000; //newtons
        [SerializeField] private float jitterFactor = 0.1f; //as a percentage of current intensity


        private float maxThrust;
        private float currentThrust;
        private float currentIntensity;
        private float thrustFactor;
        private void Start()
        {
            //you VILL eat ze bugs, you VILL fly ze aryxslop, you VILL assign in the inspector or face a firing squad. I am NOT nullchecking ts (this)
            maxThrust = nozzle.GetMaxThrust() + turbojet.GetMaxThrust();
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].intensity = 0;
            }
        }

        private void Update()
        {
            currentThrust = turbojet.GetThrust() + nozzle.GetTotalThrust();
            if (currentThrust < minThrustToLight)
            {
                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i].intensity = 0;
                }
                return;
            }

            thrustFactor = Mathf.InverseLerp(minThrustToLight, maxThrust, currentThrust);
            currentIntensity = Mathf.Lerp(intensityAtMinThrust, intensityAtMaxThrust, thrustFactor);

            currentIntensity += (Random.value * 2 - 1) * jitterFactor * currentIntensity;

            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].intensity = currentIntensity;
            }
        }

    }
}
