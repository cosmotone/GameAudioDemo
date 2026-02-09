#if VFX_OUTPUTEVENT_AUDIO
using UnityEngine.Events;
using FMODUnity;
using FMOD.Studio;

namespace UnityEngine.VFX.Utility
{
    [ExecuteAlways]
    [RequireComponent(typeof(VisualEffect))]
    class VFXOutputEventPlayAudio : VFXOutputEventAbstractHandler
    {
        public override bool canExecuteInEditor => true;

        public AudioSource audioSource;
        public EventReference fmodEvent;
        public bool useFMOD = false;

        public override void OnVFXOutputEvent(VFXEventAttribute eventAttribute)
        {
            if (useFMOD)
            {
                if (!fmodEvent.IsNull)
                {
                    RuntimeManager.PlayOneShot(fmodEvent);
                }
            }
            else
            {
                if (audioSource != null)
                    audioSource.Play();
            }
        }
    }
}
#endif