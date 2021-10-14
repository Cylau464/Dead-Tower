using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CommonShopItem : ShopItem
{
    [SerializeField] private TextMeshProUGUI _purchasedText;
    [Space]
    [SerializeField] private Sprite _coinSprite;
    [SerializeField] private Sprite _diamondSprite;
    [Space]
    [SerializeField] private Button _purchaseBtn;
    [SerializeField] private Color _disableTextColor;
    [SerializeField] private Color _enableTextColor;
    [Space]
    [SerializeField] private Slider _damageSliders;
    [SerializeField] private Slider _healthSliders;
    [SerializeField] private Slider _specSliders;

    public override void Init(ShopItemConfig config, bool isPurchased = false)
    {
        _purchasedText.gameObject.SetActive(false);
        base.Init(config, isPurchased);

        _costText.text = config.Stats.Cost.ToString();
        _currencyIcon.sprite = config.Stats.CurrencyType == CurrencyTypes.Coins ? _coinSprite : _diamondSprite;
        _purchaseBtn.onClick.AddListener(Purchase);
        

        if(isPurchased == false)
        {
            CheckEnoughCurrency(config.Stats.CurrencyType, 
                config.Stats.CurrencyType == CurrencyTypes.Coins
                ? SLS.Data.Game.Coins.Value
                : SLS.Data.Game.Diamonds.Value);

            if(config.Stats.CurrencyType == CurrencyTypes.Coins)
                SLS.Data.Game.Coins.OnValueChanged += OnCoinsChanged;
            else
                SLS.Data.Game.Diamonds.OnValueChanged += OnDiamondsChanged;
        }

        if(config.Category == ItemCategory.Tower)
        {
            TowerStats stats = SLS.Data.Game.Towers.Value[(_config as TowerItemConfig).Config.Index].Stats;
            _damageSliders.value = stats.BasicDamage;
            _healthSliders.value = stats.BasicHealth;
            _specSliders.value = stats.BasicAbilityLevel;
        }
        else if(config.Category == ItemCategory.Defender)
        {
            TowerDefenderStats stats = SLS.Data.Game.Defenders.Value[(_config as DefenderItemConfig).Config.Index].Stats;
            _damageSliders.value = stats.BasicDamage;
            _healthSliders.value = stats.BasicHealth;
            _specSliders.value = stats.BasicShootDistance;
        }
        else
        {
            _damageSliders?.gameObject.SetActive(false);
            _healthSliders?.gameObject.SetActive(false);
            _specSliders?.gameObject.SetActive(false);
        }
    }

    protected override void Purchased()
    {
        base.Purchased();
        _purchasedText.gameObject.SetActive(true);
        _purchasedText.text = "Purchased";
        _purchaseBtn.interactable = false;

        if (_config.Stats.CurrencyType == CurrencyTypes.Coins)
            SLS.Data.Game.Coins.OnValueChanged -= OnCoinsChanged;
        else
            SLS.Data.Game.Diamonds.OnValueChanged -= OnDiamondsChanged;
    }

    public override void Purchase()
    {
        switch(_config.Category)
        {
            case ItemCategory.Tower:
                SLS.Data.Game.Towers.Value[(_config as TowerItemConfig).Config.Index].IsPurchased = true;
                SLS.Data.Game.Towers.SaveValue();
                Purchased();
                break;
            case ItemCategory.Defender:
                SLS.Data.Game.Defenders.Value[(_config as DefenderItemConfig).Config.Index].IsPurchased = true;
                SLS.Data.Game.Defenders.SaveValue();
                Purchased();
                break;
            case ItemCategory.Resources:
                Resource[] resources = SLS.Data.Game.Resources.Value;
                ResourcesItemConfig rConfig = _config as ResourcesItemConfig;
                int index = Array.FindIndex(resources, x => x.Type == rConfig.Type);
                resources[index].Count += rConfig.Count;
                SLS.Data.Game.Resources.Value = resources;
                break;
            case ItemCategory.Currency:
                CurrencyItemConfig cConfig = _config as CurrencyItemConfig;

                if (cConfig.Type == CurrencyTypes.Coins)
                    SLS.Data.Game.Coins.Value += cConfig.Count;
                else
                    SLS.Data.Game.Diamonds.Value += cConfig.Count;

                break;
        }

        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        WriteOffCurrency(_config.Stats.CurrencyType, _config.Stats.Cost);
    }

    private void OnCoinsChanged(int newAmount)
    {
        CheckEnoughCurrency(CurrencyTypes.Coins, newAmount);
    }

    private void OnDiamondsChanged(int newAmount)
    {
        CheckEnoughCurrency(CurrencyTypes.Diamonds, newAmount);
    }

    private void CheckEnoughCurrency(CurrencyTypes type, int amount)
    {
        if (amount < _config.Stats.Cost)
            PurchaseAllowed(false);
        else
            PurchaseAllowed(true);
    }

    private void PurchaseAllowed(bool allowed)
    {
        _costText.color = allowed == true ? _enableTextColor : _disableTextColor;
        _purchaseBtn.interactable = allowed;
    }

    private void WriteOffCurrency(CurrencyTypes type, int amount)
    {
        if (type == CurrencyTypes.Coins)
            SLS.Data.Game.Coins.Value -= amount;
        else
            SLS.Data.Game.Diamonds.Value -= amount;
    }

    private void OnDestroy()
    {
        if (_config.Stats.CurrencyType == CurrencyTypes.Coins)
            SLS.Data.Game.Coins.OnValueChanged -= OnCoinsChanged;
        else
            SLS.Data.Game.Diamonds.OnValueChanged -= OnDiamondsChanged;
    }
}