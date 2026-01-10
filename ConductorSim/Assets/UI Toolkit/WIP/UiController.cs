using UnityEngine;
using UnityEngine.UIElements;

public class UiController : MonoBehaviour
{

    [SerializeField] UIDocument uiDocument;

    void Start()
    {

        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null)
        {
            Debug.LogWarning("UiController: UIDocument nie przypisany ani nie znaleziony na tym GameObject.");
            return;
        }

        var root = uiDocument.rootVisualElement;


        VisualElement openBook = root.Q<VisualElement>("Open_Book");
        Button bookButton = root.Q<Button>("Book");
        Button resumeButton = root.Q<Button>("Resume_Button");

        if (openBook == null)
        {
            Debug.LogWarning("UiController: VisualElement 'Open_Book' nie znaleziony. SprawdŸ pole Name w UI Builderze.");
            return;
        }

      
        openBook.style.display = DisplayStyle.None;

        if (bookButton != null)
        {
            bookButton.clicked += () =>
            {
                openBook.style.display = DisplayStyle.Flex;
            };
        }
        else
        {
            Debug.LogWarning("UiController: Button 'Book' nie znaleziony (pole Name).");
        }

        if (resumeButton != null)
        {
            resumeButton.clicked += () =>
            {
                openBook.style.display = DisplayStyle.None;
            };
        }
        else
        {
            Debug.LogWarning("UiController: Button 'Resume_Button' nie znaleziony (pole Name).");
        }
    }

}


