using System;
using UnityEngine;

namespace Divergence
{
    public class MainframeAudioHandler : MonoBehaviour
    {
        [SerializeField]
        private OWAudioSource _arrivalMusicSource;

        [SerializeField]
        private OWAudioSource _ambientMusicSource;

        [SerializeField]
        private OWTriggerVolume _arrivalMusicTrigger;

        [SerializeField]
        private OWTriggerVolume _ambientMusicTrigger;

        private bool _hasPlayedArrivalMusic;

        private void Start()
        {
            _arrivalMusicTrigger = GameObject.Find("DreamWorld_Body/Sector_DreamWorld/Mainframe_ArrivalStinger").GetComponent<OWTriggerVolume>();
            _ambientMusicTrigger = GameObject.Find("DreamWorld_Body/Sector_DreamWorld/Mainframe_Ambience").GetComponent<OWTriggerVolume>();
            _arrivalMusicSource = GameObject.Find("DreamWorld_Body/Sector_DreamWorld/Mainframe_ArrivalStinger").GetComponent<OWAudioSource>();
            _ambientMusicSource = GameObject.Find("DreamWorld_Body/Sector_DreamWorld/Mainframe_Ambience").GetComponent<OWAudioSource>();

            _ambientMusicTrigger.gameObject.SetActive(false);
            _arrivalMusicTrigger.OnEntry += OnEnterMainframeMusicTrigger;
        }

        private void Update()
        {
            if (!_hasPlayedArrivalMusic)
            {
                return;
            }
            if (_arrivalMusicSource.time > 30f || !_arrivalMusicSource.isPlaying)
            {
                if (!Locator.GetDreamWorldController()._outsideLanternBounds)
                {
                    if (!_ambientMusicTrigger.isActiveAndEnabled)
                    {
                        _ambientMusicTrigger.gameObject.SetActive(true);
                    }

                    if (_arrivalMusicSource.isPlaying)
                    {
                        _ambientMusicSource._fadeDuration = 10f;
                    }
                    else
                    {
                        _ambientMusicSource._fadeDuration = 2f;
                    }
                }
                else
                {
                    _hasPlayedArrivalMusic = false;
                }
            }
            if (_hasPlayedArrivalMusic && !_arrivalMusicSource.isPlaying && _arrivalMusicTrigger.isActiveAndEnabled && !Locator.GetDreamWorldController()._outsideLanternBounds)
            {
                _arrivalMusicTrigger.gameObject.SetActive(false);
            }
        }

        private void OnEnterMainframeMusicTrigger(GameObject hitObj)
        {
           if (hitObj.CompareTag("PlayerDetector") && !_hasPlayedArrivalMusic)
           {
               _hasPlayedArrivalMusic = true;
           }
        }
    }
}
