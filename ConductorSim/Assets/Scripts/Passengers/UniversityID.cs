using TMPro;
using UnityEngine;

public class UniversityID : DragAndDrop
{
    // Data comparator
    [SerializeField] DataComparator dataComparator;

    // Front and back
    [SerializeField] GameObject front, back;

    // All of the buttons
    [SerializeField] GameObject albumNumberButton, signatureButton, studentNameButton, dateOfBirthButton, addressButton, releaseDateButton, releaserNameButton;
    [SerializeField] GameObject[] yearButtons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        back.SetActive(false);
        front.SetActive(true);
    }

    public void SwitchSides()
    {
        front.SetActive(!front.activeSelf);
        back.SetActive(!back.activeSelf);
    }

    public void SetUniversityID(UniversityIDData universityIDData)
    {
        if(universityIDData != null)
        {
            albumNumberButton.GetComponentInChildren<TextMeshProUGUI>().text = universityIDData.albumNumber;
            signatureButton.GetComponentInChildren<TextMeshProUGUI>().text = universityIDData.signature;
            studentNameButton.GetComponentInChildren<TextMeshProUGUI>().text = universityIDData.name;
            dateOfBirthButton.GetComponentInChildren<TextMeshProUGUI>().text = universityIDData.dateOfBirth;
            addressButton.GetComponentInChildren<TextMeshProUGUI>().text = UniversityIDData.address;
            releaseDateButton.GetComponentInChildren<TextMeshProUGUI>().text = universityIDData.releaseDate;
            releaserNameButton.GetComponentInChildren<TextMeshProUGUI>().text = universityIDData.releaserName;

            // Clear all year buttons
            foreach(GameObject obj in yearButtons) 
            { 
                obj.GetComponent<AdvancedButton>().interactable = false; 
                obj.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }

            // Assign year buttons
            for(int i = universityIDData.startYearsAgo, j = 0; i >= 0; i--, j += 2)
            {
                yearButtons[j].GetComponentInChildren<TextMeshProUGUI>().text = "200" + (int.Parse(SchoolIDData.expirationYear) - i).ToString();
                if(yearButtons[j + 1] != null && i != 0) { yearButtons[j + 1].GetComponentInChildren<TextMeshProUGUI>().text = "200" + (int.Parse(SchoolIDData.expirationYear) - i).ToString(); }

                if(i == 0) { yearButtons[j].GetComponent<AdvancedButton>().interactable = true; }
            }
        }
        // else { print("There's no data to load!"); }
    }
}
