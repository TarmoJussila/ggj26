using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Logbound.Rats
{
    public class RatAudio : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioSource _audioSource;

        private void Start()
        {
            _audioSource.Play();
            _audioSource.time = Mathf.Lerp(0f, 30f, Random.Range(0f, 1f));
        }}
}