using TMPro;
using UnityEngine;

public class PersonalID : DragAndDrop
{
    // Front and back
    [SerializeField] GameObject front, back;

    // All of the buttons
    [SerializeField] GameObject lastNameButton, firstNameButton, familyNameButton, parentsNamesButton, dateOfBirthButton, genderButton, 
        expirationDateButton, addressButton, peselButton, placeOfBirthButton, heightButton, eyeColorButton, releaseDateButton, issuingAuthorityButton;
    
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

    public void SetPersonalID(PersonalIDData personalIDData)
    {
        if(personalIDData != null)
        {
            lastNameButton.GetComponentInChildren<TextMeshProUGUI>().text = personalIDData.lastName;
            firstNameButton.GetComponentInChildren<TextMeshProUGUI>().text = personalIDData.firstName;
            familyNameButton.GetComponentInChildren<TextMeshProUGUI>().text = personalIDData.familyName;
            parentsNamesButton.GetComponentInChildren<TextMeshProUGUI>().text = personalIDData.parentsNames;
            dateOfBirthButton.GetComponentInChildren<TextMeshProUGUI>().text = personalIDData.dateOfBirth;
            genderButton.GetComponentInChildren<TextMeshProUGUI>().text = personalIDData.gender;
            expirationDateButton.GetComponentInChildren<TextMeshProUGUI>().text = personalIDData.expirationDate;
            addressButton.GetComponentInChildren<TextMeshProUGUI>().text = PersonalIDData.address;
            peselButton.GetComponentInChildren<TextMeshProUGUI>().text = personalIDData.pesel;
            placeOfBirthButton.GetComponentInChildren<TextMeshProUGUI>().text = PersonalIDData.placeOfBirth;
            heightButton.GetComponentInChildren<TextMeshProUGUI>().text = personalIDData.height;
            eyeColorButton.GetComponentInChildren<TextMeshProUGUI>().text = personalIDData.eyeColor;
            releaseDateButton.GetComponentInChildren<TextMeshProUGUI>().text = personalIDData.releaseDate;
            issuingAuthorityButton.GetComponentInChildren<TextMeshProUGUI>().text = PersonalIDData.issuingAuthority;
        }
        // else { print("There's no data to load!"); }
    }
}
