using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLoader : MonoBehaviour
{
    [SerializeField] private StageConfig _config = null;
    [SerializeField] private StageFactory _factory = null;
    [SerializeField] private LineRenderer _line = null;

    private void Start()
    {
        Level[] levels = _factory.GetStageLevels(_config);

        for(int i = 0; i < levels.Length; i++)
        {
            levels[i].transform.position = _line.GetPosition(i);
            levels[i].transform.SetParent(transform, false);
        }
    }
}
