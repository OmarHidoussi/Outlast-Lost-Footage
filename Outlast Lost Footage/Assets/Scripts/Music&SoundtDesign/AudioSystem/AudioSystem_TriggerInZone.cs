using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Collider))]
public class AudioSystem_TriggerInZone : MonoBehaviour
{

    #region Variables

    [Space]
    [Header("Audio Properties")]
    public AudioSource source;
    public AudioClip audioClip;
    public AudioMixerGroup MixerGroup;
    public float Volume;
    public float Pitch = 1f;

    [Space]
    [Header("FadeIn/Out")]
    public float BlendSpeed;

    [Space]
    public bool PlayOnEnter;
    public bool PlayOnAwake;

    [Space]
    public bool DestroyOnEnter;
    public bool DestroyOnExit;
    public bool Loop;

    private BoxCollider col;
    private bool HasExited, HasEntered;

    #endregion

    #region BuiltInMethods

    private void OnDrawGizmosSelected()
    {
        col = GetComponent<BoxCollider>();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(col.center + transform.position, (col.size + new Vector3(BlendSpeed, BlendSpeed, BlendSpeed) * 0.2f));
    }

    void Awake()
    {
        if (PlayOnAwake)
        {
            source.playOnAwake = true;
            source.volume = Volume;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();

        if (MixerGroup != null)
            source.outputAudioMixerGroup = MixerGroup;


        source.loop = Loop;
        source.clip = audioClip;

        if (!PlayOnAwake)
        {
            source.volume = 0;
        }

        source.pitch = Pitch;

        if(audioClip != null)
        {
            source.PlayOneShot(audioClip);
            HasExited = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HasExited = false;

            if (PlayOnEnter)
                HasEntered = true;
            
            if (DestroyOnEnter)
                StartCoroutine(destroy());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (HasEntered)
                StartCoroutine(FadeIn());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HasExited = true;

            StartCoroutine(FadeOut());

            if (DestroyOnExit)
                StartCoroutine(destroy());

        }
    }

    #endregion

    #region CustomMethods

    IEnumerator FadeIn()
    {
        while (source.volume < Volume && !HasExited)
        {
            source.volume = Mathf.Lerp(source.volume, Volume, BlendSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        while (source.volume > 0.0f && HasExited)
        {
            source.volume = Mathf.Lerp(source.volume, 0.0f, BlendSpeed *  Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(audioClip.length);
        Destroy(this.gameObject);
    }

    #endregion

}
