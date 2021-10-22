using UnityEngine;

public class Skull : MonoBehaviour
{
    [SerializeField] private SkullHand _hand;
    [SerializeField] private Animator _skullAnimator;
    [SerializeField] private Animator _handAnimator;

    private int _spawnParamID;
    private int _victoryParamID;

    private void Awake()
    {
        _spawnParamID = Animator.StringToHash("spawn");
        _victoryParamID = Animator.StringToHash("victory");
    }

    private void OnEnable()
    {
        _hand.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if(_hand != null)
            _hand.gameObject.SetActive(false);
    }

    private void Start()
    {
        Game.Instance.OnLevelEnd += OnLevelEnd;
    }

    public void SetEnemy(Enemy enemy)
    {
        enemy.Grab();
        enemy.transform.position = Vector3.one * 100f;
        _hand.SetEnemy(enemy);
        _handAnimator.SetTrigger(_spawnParamID);
        _skullAnimator.SetTrigger(_spawnParamID);
    }

    private void OnLevelEnd(bool victory)
    {
        if (victory == true)
            _skullAnimator.SetTrigger(_victoryParamID);
    }

    private void OnDestroy()
    {
        Game.Instance.OnLevelEnd -= OnLevelEnd;
    }
}