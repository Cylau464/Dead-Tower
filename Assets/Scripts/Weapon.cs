using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float _minCharge = 4f;
    [SerializeField] private float _maxCharge = 16f;
    [SerializeField] private float _chargeSpeed = 1f;
    private float _charge;

    [SerializeField] private TrajectoryLine _line = null;
    [SerializeField] private Transform _shootPoint = null;
    [SerializeField] private GameObject _projectile = null;

    private void Start()
    {
        _line.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _charge = _minCharge;
            _line.gameObject.SetActive(true);
            _line.UpdateLine(_charge * _shootPoint.right, _shootPoint.position);
        }
        else if(Input.GetMouseButton(0))
        {
            _charge += _chargeSpeed * Time.deltaTime;
            _line.UpdateLine(_charge * _shootPoint.right, _shootPoint.position);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            GameObject projectile = Instantiate(_projectile, _shootPoint.position, _shootPoint.rotation);
            projectile.GetComponent<Projectile>().SetVelocity(_shootPoint.right * _charge); 
            _line.gameObject.SetActive(false);
        }
    }
}