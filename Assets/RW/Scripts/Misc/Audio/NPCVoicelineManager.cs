using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NPCVoicelineManager : MonoBehaviour
{
    public AudioClip[] arrow, attack, comboAttack;
    public AudioClip stun, death;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlaySound(AudioClip[] array)
    {
        audioSource.PlayOneShot(array[Random.Range(0, array.Length)]);
    }
}
