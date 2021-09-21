using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;

public class Enemy : MonoBehaviour
{
    private EnemyStats _stats;
    public EnemyStats Stats => _stats;
    private Rewards _rewards;
    private SkeletonDataAsset _skeletonData;

    public Action OnDead;

    private void Update()
    {
        transform.position += Vector3.left * Time.deltaTime;
    }

    public void GetHit(int damage)
    {
        _stats.Health -= damage;

        if (_stats.Health <= 0)
            Dead();
    }

    private void Dead()
    {
        Destroy(gameObject);
        OnDead?.Invoke();
    }

    public void Init(EnemyConfig config)
    {
        _stats = config.Stats;
        _rewards = config.Rewards;
        _skeletonData = config.SkeletonData;
        gameObject.name = config.Name;
    }
}
