using UnityEngine;
using UnityEngine.EventSystems;

public class TapEffect : MonoBehaviour
{
    [SerializeField] private GameObject _particlePrefab;
    [SerializeField] private RectTransform _targetTransform;

    private void Update()
    {
        bool mouseButtonUp, isPointerOverGO;
#if UNITY_STANDALONE || UNITY_EDITOR
        mouseButtonUp = Input.GetMouseButtonUp(0);
         isPointerOverGO = EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID || UNITY_IOS
        if(Input.touchCount > 0)
            mouseButtonUp = Input.GetTouch(0).phase == TouchPhase.Canceled || Input.touches[0].phase == TouchPhase.Ended;
        else
            mouseButtonUp = false;
        
        if (Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Canceled || Input.touches[0].phase == TouchPhase.Ended))
            isPointerOverGO = EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId);
        else
            isPointerOverGO = false;
#endif

        if (mouseButtonUp == true/* && isPointerOverGO == false*/)
        {
            Instantiate(_particlePrefab, Input.mousePosition, Quaternion.identity, _targetTransform);
        }
    }
}
