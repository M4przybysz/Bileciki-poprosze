using TMPro;
using UnityEngine;

public class PensionerID : DragAndDrop
{
    // All of the buttons
    [SerializeField] GameObject idNumberButton, firstNameButton, lastNameButton, peselButton, benefitNumberButton;

    static readonly Vector2 startPosition = new(500, -170);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start() { base.Start(); }

    public void SetPensionerIDText(PensionerIDData pensionerIDData)
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

    public void ResetPosition() { GetComponent<RectTransform>().anchoredPosition = startPosition; }
}
