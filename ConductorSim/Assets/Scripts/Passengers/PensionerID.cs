using TMPro;
using UnityEngine;

public class PensionerID : DragAndDrop
{
    // All of the buttons
    [SerializeField] GameObject idNumberButton, firstNameButton, lastNameButton, peselButton, benefitNumberButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
    }

    public void SetPensionerID(PensionerIDData pensionerIDData)
    {
        if(pensionerIDData != null)
        {
            idNumberButton.GetComponentInChildren<TextMeshProUGUI>().text = pensionerIDData.pensionerIDNumber;
            firstNameButton.GetComponentInChildren<TextMeshProUGUI>().text = pensionerIDData.firstName;
            lastNameButton.GetComponentInChildren<TextMeshProUGUI>().text = pensionerIDData.lastName;
            peselButton.GetComponentInChildren<TextMeshProUGUI>().text = pensionerIDData.pesel;
            benefitNumberButton.GetComponentInChildren<TextMeshProUGUI>().text = pensionerIDData.benefitNumber;
        }
        // else { print("There's no data to load!"); }
    }
}
