using TMPro;
using UnityEngine;

public class SchoolID : DragAndDrop
{
    // Front and back
    [SerializeField] GameObject front, back;

    // All of the buttons
    [SerializeField] GameObject idNumberButton, signatureButton, studentNameButton, dateOfBirthButton, peselButton, addressButton, 
        releaseDateButton, principalNameButton;
    [SerializeField] GameObject[] yearButtons;

    static readonly Vector2 startPosition = new(-300, -170);

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

    public void SetSchoolIDText(SchoolIDData schoolIDData)
    {
        if(schoolIDData != null)
        {
            idNumberButton.GetComponentInChildren<TextMeshProUGUI>().text = schoolIDData.schoolIDNumber;
            signatureButton.GetComponentInChildren<TextMeshProUGUI>().text = schoolIDData.signature;
            studentNameButton.GetComponentInChildren<TextMeshProUGUI>().text = schoolIDData.name;
            dateOfBirthButton.GetComponentInChildren<TextMeshProUGUI>().text = schoolIDData.dateOfBirth;
            peselButton.GetComponentInChildren<TextMeshProUGUI>().text = schoolIDData.pesel;
            addressButton.GetComponentInChildren<TextMeshProUGUI>().text = SchoolIDData.address;
            releaseDateButton.GetComponentInChildren<TextMeshProUGUI>().text = schoolIDData.releaseDate;
            principalNameButton.GetComponentInChildren<TextMeshProUGUI>().text = schoolIDData.principalName;

            // Clear all year buttons
            foreach(GameObject obj in yearButtons) 
            { 
                obj.GetComponent<AdvancedButton>().interactable = false; 
                obj.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }

            // Assign year buttons
            for(int i = schoolIDData.startYearsAgo, j = 0; i >= 0; i--, j++)
            {
                yearButtons[j].GetComponentInChildren<TextMeshProUGUI>().text = "0" + (int.Parse(SchoolIDData.expirationYear) - i).ToString();
                
                if(i == 0) { yearButtons[j].GetComponent<AdvancedButton>().interactable = true; }
            }
        }
        // else { print("There's no data to load!"); }
    }

    public void ResetPosition() { GetComponent<RectTransform>().anchoredPosition = startPosition; }
}
