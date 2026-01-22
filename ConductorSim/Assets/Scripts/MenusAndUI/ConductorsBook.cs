using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class ConductorsBook : MonoBehaviour
{
    [SerializeField] DataComparator dataComparator;
    [SerializeField] GameObject[] pages;
    public AudioSource flipPage;
    public AudioClip[] pageSounds;
    private AudioClip activeSound;
    int pageCounter = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlipPage(int direction)
    {
        pageCounter += direction;
        if(pageCounter < 0) { pageCounter = pages.Length - 1; }
        else if (pageCounter >= pages.Length) { pageCounter = 0; }

        foreach(GameObject page in pages) { page.SetActive(false); }

        pages[pageCounter].SetActive(true);

        //Flip Page Sound

            if (!flipPage.isPlaying)
            {
                activeSound = pageSounds[Random.Range(0, pageSounds.Length)];
                flipPage.PlayOneShot(activeSound);
            }

    }
}
