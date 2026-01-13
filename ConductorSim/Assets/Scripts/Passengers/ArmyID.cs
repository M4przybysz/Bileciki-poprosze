using TMPro;
using UnityEngine;

public class ArmyID : DragAndDrop
{
    // Front and back
    [SerializeField] GameObject front, back;

    // All of the buttons
    [SerializeField] GameObject militaryRankButton, lastNameButton, firstNameButton, dateOfBirthButton, peselButton, releaseDateButton, 
        genderButton, seriesAndNumberButton, expirationDateButton, militaryUnitButton;
    
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

    public void SetArmyID(ArmyIDData armyIDData)
    {
        if(armyIDData != null)
        {
            militaryRankButton.GetComponentInChildren<TextMeshProUGUI>().text = armyIDData.militaryRank;
            lastNameButton.GetComponentInChildren<TextMeshProUGUI>().text = armyIDData.lastName;
            firstNameButton.GetComponentInChildren<TextMeshProUGUI>().text = armyIDData.firstName;
            dateOfBirthButton.GetComponentInChildren<TextMeshProUGUI>().text = armyIDData.dateOfBirth;
            peselButton.GetComponentInChildren<TextMeshProUGUI>().text = armyIDData.pesel;
            releaseDateButton.GetComponentInChildren<TextMeshProUGUI>().text = armyIDData.releaseDate;
            genderButton.GetComponentInChildren<TextMeshProUGUI>().text = armyIDData.gender;
            seriesAndNumberButton.GetComponentInChildren<TextMeshProUGUI>().text = armyIDData.seriesAndNumber;
            expirationDateButton.GetComponentInChildren<TextMeshProUGUI>().text = armyIDData.expirationDate;
            militaryUnitButton.GetComponentInChildren<TextMeshProUGUI>().text = ArmyIDData.militaryUnit;
        }
        // else { print("There's no data to load!"); }
    }
}
