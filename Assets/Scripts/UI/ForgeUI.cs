using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ForgeUI : CanvasGroupUI
{
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

        base.Init();
    }

    private void ChooseRecipe(ProjectileConfig config)
    {
        _curProjectileConfig = config;
        ProjectileRecipe recipe = config.Recipe;
        _projectileIcon.SetSprite(recipe.ForgeIcon);
        StopAllCoroutines();
        this.LerpCoroutine(
            time: .25f,
            from: float.Parse(_costText.text),
            to: recipe.Cost,
            action: a => _costText.text = Mathf.RoundToInt(a).ToString()
        );
        bool canCraft = recipe.Cost > SLS.Data.Game.Coins.Value == true ? false : true;

        for(int i = 0; i < _resourceIcons.Length; i++)
        {
            foreach(Resource resource in recipe.Resources)
            {
                if (resource.Count > SLS.Data.Game.Resources.Value.First(x => x.Type == resource.Type).Count)
                    canCraft = false;

                for(int k = 0; k < resource.Count; k++)
                {
                    if (i >= _resourceIcons.Length)
                        throw new System.Exception("Attempts to fill more cells with resources than there are");

                    _resourceIcons[i].SetSprite(AssetsHolder.Instance.ResourceConfigs.First(x => x.Type == resource.Type).ForgeIcon);
                    i++;
                }
            }

            if (i < _resourceIcons.Length)
                _resourceIcons[i].SetSprite(_recipeSprite);
        }

        _craftBtn.interactable = canCraft;
    }

    private void Craft()
    {
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
    }

    private void BackToMap()
    {
        Hide();
        MenuSwitcher.Instance.OpenMap();
    }

    private void OnDestroy()
    {
        foreach (ProjectileRecipeUI recipe in _recipes)
            recipe.OnClick -= ChooseRecipe;
    }
}