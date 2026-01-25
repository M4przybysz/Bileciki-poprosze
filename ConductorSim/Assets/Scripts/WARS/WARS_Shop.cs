using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WARSShop : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public int price;
        [TextArea(1,2)] public string description;

        // Efekty (konfigurowalne w Inspektorze)
        public bool clearEffects = false;                 // Barszcz / Schabowe
        public float preventFatigueDuration = 0f;         // Sernik / Schabowe (sekundy)
        public float speedBuffPercent = 0f;               // Kawa / Pierogi (0.25 = +25%)
        public float speedBuffDuration = 0f;              // czas dla speed buff (sekundy)
        public float capacityPercent = 0f;                // bonus do "pojemnoœci staminy" (0.25 = 25%)
        public float capacityDuration = 0f;               // czas dla capacity bonus (sekundy)
        public float restoreFatigueAmount = 0f;           // opcjonalne natychmiastowe przywrócenie zmêczenia
    }

    [Header("UI / Panel")]
    [Tooltip("Panel sklepu - przypisz swój Canvas/Panel z przyciskami")]
    public GameObject shopPanel;

    [Header("Produkty (konfiguruj w Inspektorze)")]
    [Tooltip("Jeœli lista jest pusta, zostan¹ utworzone domyœlne pozycje.")]
    public List<ShopItem> items = new List<ShopItem>();

    [Header("Powi¹zania przycisków -> item (opcjonalne)")]
    [Tooltip("Wykorzystaj, aby przypi¹æ konkretne przyciski UI do indeksów produktów. " +
             "Skrypt ustawi tekst przycisku na 'Nazwa (cena)' je¿eli znajdzie komponent Text.")]
    public List<ButtonBinding> buttonBindings = new List<ButtonBinding>();

    [System.Serializable]
    public class ButtonBinding
    {
        public Button button;
        [Tooltip("Indeks produktu z listy 'items'")]
        public int itemIndex;
        public Text labelText; // opcjonalnie: referencja do Text (jeœli nie ustawiona, spróbuje znaleŸæ w dzieciach)
    }

    bool playerInRange = false;
    bool shopOpen = false;

    // cache player controller and fatigue when w zasiêgu
    private PlayerController playerController;
    private PlayerFatigue playerFatigue;

    void Start()
    {
        // fallback - domyœlne pozycje je¿eli nic nie przypisano w Inspektorze
        if (items == null || items.Count == 0)
        {
            items = new List<ShopItem>()
            {
                new ShopItem(){ itemName = "Kawa z mlekiem", price = 5, description = "Ma³a kawa. Odœwie¿a.", speedBuffPercent = 0.25f, speedBuffDuration = 30f },
                new ShopItem(){ itemName = "Barszcz", price = 8, description = "Rozgrzewaj¹cy barszcz.", clearEffects = true },
                new ShopItem(){ itemName = "Sernik", price = 12, description = "S³odki sernik.", preventFatigueDuration = 120f },
                new ShopItem(){ itemName = "Pierogi ruskie", price = 10, description = "Klasyczne pierogi.", speedBuffPercent = 0.25f, speedBuffDuration = 90f },
                new ShopItem(){ itemName = "Schabowe z ziemniakami", price = 18, description = "Treœciwy obiad.", clearEffects = true, preventFatigueDuration = 120f, capacityPercent = 0.25f, capacityDuration = 60f },
                new ShopItem(){ itemName = "Kanapka", price = 6, description = "Szybka przek¹ska.", capacityPercent = 0.25f, capacityDuration = 60f }
            };
        }

        if (shopPanel != null) shopPanel.SetActive(false);

        // ustaw bindingi przycisków (jeœli zosta³y przypisane)
        BindButtonsToItems();
    }

    void BindButtonsToItems()
    {
        foreach (var binding in buttonBindings)
        {
            if (binding == null || binding.button == null) continue;

            binding.button.onClick.RemoveAllListeners();

            int idx = binding.itemIndex; 
            binding.button.onClick.AddListener(() => BuyItemByIndex(idx));

            if (binding.labelText == null)
            {
                binding.labelText = binding.button.GetComponentInChildren<Text>();
            }

            if (binding.labelText != null && idx >= 0 && idx < items.Count)
            {
                var it = items[idx];
                binding.labelText.text = $"{it.itemName} ({it.price} z³)";
            }
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            ToggleShop();
        }
    }

    void ToggleShop()
    {
        shopOpen = !shopOpen;
        if (shopPanel != null) shopPanel.SetActive(shopOpen);
        Time.timeScale = shopOpen ? 0f : 1f; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerController = other.GetComponent<PlayerController>();
            playerFatigue = other.GetComponent<PlayerFatigue>();
            Debug.Log("WARSShop: Naciœnij F aby otworzyæ sklep.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerController = null;
            playerFatigue = null;
            if (shopPanel != null) shopPanel.SetActive(false);
            shopOpen = false;
            Time.timeScale = 1f;
        }
    }

    // PUBLIC API dla UI (przypnij Button.OnClick -> BuyItemByIndex(i))
    public void BuyItemByIndex(int index) => BuyAndApply(index);

    // pomocnicze wygodne metody (opcjonalne)
    public void BuyCoffeeWithMilk()    => BuyAndApply(0);
    public void BuyBarszcz()           => BuyAndApply(1);
    public void BuySernik()            => BuyAndApply(2);
    public void BuyPierogiRuskie()     => BuyAndApply(3);
    public void BuySchaboweZiemniak()  => BuyAndApply(4);
    public void BuyKanapka()           => BuyAndApply(5);

    private void BuyAndApply(int index)
    {
        if (!playerInRange || playerController == null)
        {
            Debug.Log("WARSShop: Brak gracza w zasiêgu sklepu.");
            return;
        }

        if (index < 0 || index >= items.Count)
        {
            Debug.LogWarning("WARSShop: Nieprawid³owy indeks produktu.");
            return;
        }

        var it = items[index];

        if (playerController.GetWallet() < it.price)
        {
            Debug.Log("WARSShop: Za ma³o œrodków.");
            return;
        }

        // Pobierz œrodki
        playerController.AddMoneyToWallet(-it.price);
        Debug.Log($"WARSShop: Kupiono {it.itemName}, zap³acono {it.price}.");

        // Natychmiastowe przywrócenie zmêczenia (jeœli ustawione)
        if (it.restoreFatigueAmount > 0f && playerFatigue != null)
        {
            playerFatigue.RestoreFatigue(it.restoreFatigueAmount);
        }

        // Clear effects
        if (it.clearEffects && playerFatigue != null)
        {
            playerFatigue.ClearEffects();
        }

        // Prevent fatigue for duration
        if (it.preventFatigueDuration > 0f && playerFatigue != null)
        {
            playerFatigue.PreventFatigueFor(it.preventFatigueDuration);
        }

        // Capacity bonus (zmniejsza tempo narastania zmêczenia)
        if (it.capacityPercent > 0f && it.capacityDuration > 0f && playerFatigue != null)
        {
            playerFatigue.ApplyFatigueCapacityBonusPercent(it.capacityPercent, it.capacityDuration);
        }

        // Speed buff
        if (it.speedBuffPercent > 0f && it.speedBuffDuration > 0f && playerController != null)
        {
            playerController.ApplySpeedBuffPercent(it.speedBuffPercent, it.speedBuffDuration);
        }

    }
}
