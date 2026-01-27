using UnityEngine;

public class ConductorsBook : MonoBehaviour
{
    [SerializeField] DataComparator dataComparator;
    [SerializeField] GameObject[] pages;
    public AudioSource flipPage;
    public AudioClip[] pageSounds;
    private AudioClip activeSound;
    int pageCounter = 0;

    public void FlipPage(int direction)
    {
        pageCounter += direction;
        if(pageCounter < 0) { pageCounter = pages.Length - 1; }
        else if (pageCounter >= pages.Length) { pageCounter = 0; }

        foreach(GameObject page in pages) { page.SetActive(false); }

        pages[pageCounter].SetActive(true);

        if (!flipPage.isPlaying) // Play flip page sound
        {
            activeSound = pageSounds[Random.Range(0, pageSounds.Length)];
            flipPage.PlayOneShot(activeSound);
        }

    }
}
