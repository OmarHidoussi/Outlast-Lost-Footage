using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Collider))]
public class AudioSystem_TriggerSimple : MonoBehaviour
{

    #region Variables

    [Space]
    [Header("Audio Properties")]
    public AudioSource source;
    public AudioClip audioClip;
    public AudioMixerGroup MixerGroup;
    public float Volume;
    public float Pitch = 1f;

    [Range(-0.2f,0.2f)]
    public float Randomize;

    [Space]
    public bool DestroyOnEnter;
    public bool DestroyOnExit;
    public bool DestroyOnload;

    private BoxCollider col;
    private bool HasPlayed = false;

    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Start()
    {

        col = GetComponent<BoxCollider>();

        if (MixerGroup != null)
            source.outputAudioMixerGroup = MixerGroup;

        /*if (!DestroyOnload)
            DontDestroyOnLoad(gameObject);*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!source.isPlaying && !HasPlayed)
            {
                source.volume = Volume;
                source.pitch = Pitch;
                source.PlayOneShot(audioClip);

                HasPlayed = true;

                if (DestroyOnEnter)
                    StartCoroutine(destroy());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HasPlayed = false;

            if (DestroyOnExit)
                StartCoroutine(destroy());
        }
    }

    #endregion

    #region CustomMethods

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(audioClip.length);
        Destroy(this.gameObject);
    }

    #endregion

}
