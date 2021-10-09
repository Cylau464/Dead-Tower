using UnityEngine;

[CreateAssetMenu(fileName = "CS Factory", menuName = "Factories/UI/Character Settings")]
public class CharacterSettingsFactory : GameObjectFactory
{
    [SerializeField] private CSItem _itemPrefab;

    public CSItem GetItem(UnitBasicConfig config, CharacterSettingsTabUI tab)
    {
        CSItem item = CreateGameObjectInstance(_itemPrefab);
        item.Init(config, tab);

        return item;
    }
}