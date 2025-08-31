using System;
using UnityEngine;

namespace Divergence
{
    public class MainframeAudioHandler : MonoBehaviour
    {
        [SerializeField]
        private OWAudioSource _arrivalMusicSource;

        [SerializeField]
        private OWTriggerVolume _arrivalMusicTrigger;

        private bool _hasPlayedArrivalMusic;

        private void Start()
        {
            _arrivalMusicTrigger = GameObject.Find("DreamWorld_Body/Sector_DreamWorld/Mainframe_ArrivalStinger").GetComponent<OWTriggerVolume>();
            _arrivalMusicSource = GameObject.Find("DreamWorld_Body/Sector_DreamWorld/Mainframe_ArrivalStinger").GetComponent<OWAudioSource>();
            _arrivalMusicTrigger.OnEntry += OnEnterMainframeMusicTrigger;
        }

        private void OnEnterMainframeMusicTrigger(GameObject hitObj)
        {
           if (hitObj.CompareTag("PlayerDetector") && !_hasPlayedArrivalMusic)
           {
               _hasPlayedArrivalMusic = true;
           }
           else if(!_arrivalMusicSource.isPlaying && _hasPlayedArrivalMusic)
           {
               _arrivalMusicTrigger.gameObject.SetActive(false);
           }
        }
    }
}
