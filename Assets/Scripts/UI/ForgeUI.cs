using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;

public class ForgeUI : CanvasGroupUI
{
    [SerializeField] protected AudioClip[] _craftClips;
    [Space]
    [SerializeField] private ProjectileConfig[] _recipeConfigs;
    [SerializeField] private ProjectileRecipeUI _recipePrefab;
    [SerializeField] private RectTransform _recipesHandler;
    [SerializeField] private RectTransform _scrollContent;
    [SerializeField] private int _spacing = 50;
    [Space]
    [SerializeField] private DissolveImage _projectileIcon;
    [SerializeField] private DissolveImage[] _resourceIcons;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Sprite _recipeSprite;
    [Space]
    [SerializeField] private DissolveImage _projectileCountIcon;
    [SerializeField] private TextMeshProUGUI _projectileCountText;
    [SerializeField] private DissolveImage[] _resourceCountIcons;
    [SerializeField] private TextMeshProUGUI[] _resourceCountText;
    [Space]
    [SerializeField] private Button _craftBtn;
    [SerializeField] private Button _backBtn;

    private List<ProjectileRecipeUI> _recipes;
    private ProjectileConfig _curProjectileConfig;

    protected override void Init()
    {
        _craftBtn.onClick.AddListener(Craft);
        _backBtn.onClick.AddListener(BackToMap);
        _recipes = new List<ProjectileRecipeUI>();
        _costText.text = 0.ToString();

        foreach (ProjectileConfig config in _recipeConfigs)
        {
            ProjectileRecipeUI recipe = Instantiate(_recipePrefab);
            recipe.transform.SetParent(_recipesHandler, false);
            recipe.Init(config);
            recipe.OnClick += ChooseRecipe;
            _recipes.Add(recipe);

            if (_curProjectileConfig == null)
                ChooseRecipe(config);
        }

        VerticalLayoutGroup layoutGroup = _recipesHandler.GetComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = _spacing;
        layoutGroup.padding = new RectOffset(_spacing, _spacing, _spacing, _spacing);
        float contentHeight = (_recipePrefab.transform as RectTransform).sizeDelta.y * _recipeConfigs.Length;
        contentHeight += _spacing * 2f + _spacing * (_recipeConfigs.Length - 1);
        _scrollContent.sizeDelta = new Vector2(_scrollContent.sizeDelta.x, contentHeight);

        SLS.Data.Game.ProjectilesCount.OnValueChanged += OnProjectileChanged;
        SLS.Data.Game.Resources.OnValueChanged += OnResourcesChanged;

        base.Init();
    }

    public override void Show()
    {
        base.Show();

        if(_curProjectileConfig != null)
            ChooseRecipe(_curProjectileConfig);
    }

    private void ChooseRecipe(ProjectileConfig config)
    {
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        _curProjectileConfig = config;
        ProjectileRecipe recipe = config.Recipe;
        _projectileIcon.SetSprite(recipe.ForgeIcon);
        _projectileCountIcon.SetSprite(config.Icon);
        //StopAllCoroutines();
        this.LerpCoroutine(
            time: .25f,
            from: float.Parse(Regex.Match(_costText.text, @"\d+").Value),
            to: recipe.Cost,
            action: a => _costText.text = Mathf.RoundToInt(a).ToString() + "<sprite=1>"
        );
        this.LerpCoroutine(
            time: .25f,
            from: float.Parse(_projectileCountText.text),
            to: SLS.Data.Game.ProjectilesCount.Value[config.Index],
            action: a => _projectileCountText.text = Mathf.RoundToInt(a).ToString()
        );

        for(int i = 0; i < _resourceIcons.Length; i++)
        {
            foreach(Resource resource in recipe.Resources)
            {
                for (int k = 0; k < resource.Count; k++)
                {
                    if (i >= _resourceIcons.Length)
                        throw new System.Exception("Attempts to fill more cells with resources than there are");

                    ResourceConfig resConfig = AssetsHolder.Instance.ResourceConfigs.First(x => x.Type == resource.Type);
                    _resourceIcons[i].SetSprite(resConfig.ForgeIcon);

                    int index = i; // нужна из-за того, что когда i передаётся в корутину, то она превращается в 3
                    _resourceCountIcons[index].SetSprite(resConfig.RewardIcon);
                    this.LerpCoroutine(
                        time: .25f,
                        from: float.Parse(_resourceCountText[index].text),
                        to: SLS.Data.Game.Resources.Value.First(x => x.Type == resConfig.Type).Count,
                        action: a => _resourceCountText[index].text = Mathf.RoundToInt(a).ToString()
                    );

                    i++;
                }
            }

            if (i < _resourceIcons.Length)
                _resourceIcons[i].SetSprite(_recipeSprite);
        }

        CheckEnoughResources();
    }

    private void Craft()
    {
        AudioController.PlayClipAtPosition(_craftClips[Random.Range(0, _craftClips.Length)], transform.position);
        int[] projectilesCount = SLS.Data.Game.ProjectilesCount.Value;
        projectilesCount[_curProjectileConfig.Index]++;
        SLS.Data.Game.ProjectilesCount.Value = projectilesCount;
        SLS.Data.Game.Coins.Value -= _curProjectileConfig.Recipe.Cost;
        Resource[] resources = SLS.Data.Game.Resources.Value;

        foreach (Resource resource in _curProjectileConfig.Recipe.Resources)
        {
            int index = resources.ToList().IndexOf(resources.First(x => x.Type == resource.Type));
            resources[index].Count -= resource.Count;
        }

        SLS.Data.Game.Resources.Value = resources;
        CheckEnoughResources();
    }

    private void CheckEnoughResources()
    {
        bool canCraft = _curProjectileConfig.Recipe.Cost > SLS.Data.Game.Coins.Value == true ? false : true;

        foreach(Resource res in _curProjectileConfig.Recipe.Resources)
        {
            if (res.Count > SLS.Data.Game.Resources.Value.First(x => x.Type == res.Type).Count)
                canCraft = false;
        }

        _craftBtn.interactable = canCraft;
    }

    private void OnProjectileChanged(int[] projectiles)
    {
        this.LerpCoroutine(
            time: .25f,
            from: float.Parse(_projectileCountText.text),
            to: projectiles[_curProjectileConfig.Index],
            action: a => _projectileCountText.text = Mathf.RoundToInt(a).ToString()
        );
    }

    private void OnResourcesChanged(Resource[] resources)
    {
        List<Resource> usedResources = new List<Resource>();

        foreach(Resource res in _curProjectileConfig.Recipe.Resources)
            usedResources.Add(resources.First(x => x.Type == res.Type));

        for(int i = 0; i < _resourceCountText.Length; i++)
        {
            int index = i; // нужна из-за того, что когда i передаётся в корутину, то она превращается в 3

            if(gameObject.activeInHierarchy == true)
            {
                this.LerpCoroutine(
                    time: .25f,
                    from: float.Parse(_resourceCountText[index].text),
                    to: usedResources[Mathf.Min(i, usedResources.Count - 1)].Count,
                    action: a => _resourceCountText[index].text = Mathf.RoundToInt(a).ToString()
                );
            }
            else
            {
                _resourceCountText[index].text = Mathf.RoundToInt(
                    usedResources[Mathf.Min(i, usedResources.Count - 1)].Count).ToString();
            }
        }
    }

    private void BackToMap()
    {
        Hide();
        AudioController.PlayClipAtPosition(_buttonClip, transform.position);
        MenuSwitcher.Instance.OpenMap();
    }

    private void OnDestroy()
    {
        foreach (ProjectileRecipeUI recipe in _recipes)
            recipe.OnClick -= ChooseRecipe;

        SLS.Data.Game.ProjectilesCount.OnValueChanged -= OnProjectileChanged;
        SLS.Data.Game.Resources.OnValueChanged -= OnResourcesChanged;
    }
}