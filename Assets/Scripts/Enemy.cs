using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Enemy : MonoBehaviour
{
    private EnemyStats _stats;
    public EnemyStats Stats => _stats;
    private Rewards _rewards;
    private SkeletonDataAsset _skeletonData;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime;
    }

    public void GetHit(int damage)
    {

    }

    public void Init(EnemyConfig config)
    {
        _stats = config.Stats;
        _rewards = config.Rewards;
        _skeletonData = config.SkeletonData;
        gameObject.name = config.Name;
    }
}
