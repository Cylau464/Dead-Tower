using UnityEngine;

public class AxeZombie : Enemy
{
    [Space]
    [SerializeField] private float _takeAxeDelay = 1f;

    private int _takeAxeParamID;
    private int _axeTakenParamID;

    private new void Start()
    {
        _takeAxeParamID = Animator.StringToHash("takeAxe");
        _axeTakenParamID = Animator.StringToHash("axeTaken");

        base.Start();
    }

    public override void Spawned()
    {
        Invoke(nameof(TakeAxe), _takeAxeDelay);

        base.Spawned();
    }

    private void TakeAxe()
    {
        _animator.SetTrigger(_takeAxeParamID);
        StopMove();
    }

    public void AxeTaken()
    {
        _animator.SetBool(_axeTakenParamID, true);
        Move();
    }
}