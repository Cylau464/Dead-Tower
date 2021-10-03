using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ProjectileRecipeUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Button _button;
    private ProjectileConfig _config;

    public Action<ProjectileConfig> OnClick;

    public void Init(ProjectileConfig config)
    {
        _config = config;
        _icon.sprite = config.Icon;
        _title.text = config.Title;
        _button.onClick.AddListener(() => OnClick?.Invoke(_config));
    }
}
