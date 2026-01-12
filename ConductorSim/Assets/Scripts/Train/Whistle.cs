using UnityEngine;

public class Whistle : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip station_whistle;
    [SerializeField] AudioClip ride_whistle;

    public void PlayRideWhistle()
    {
        audioSource.resource = ride_whistle;
        audioSource.Play();
    }

    public void PlayStationWhistle()
    {
        audioSource.resource = station_whistle;
        audioSource.Play();
    }
}
