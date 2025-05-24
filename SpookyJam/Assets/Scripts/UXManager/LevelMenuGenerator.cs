using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelMenuGenerator : MonoBehaviour
{
    [SerializeField] GameObject _levelButton;
    private int _offset = 220;
    private int _xMax = 660;

    // Start is called before the first frame update
    void Start()
    {
        var levelCount = GameManager.Instance.GetCurrentWorldLevelCount();
        var currentOffsetX = 0;
        var currentOffsetY = 0;
        var initialX = transform.position.x;
        var initialY = transform.position.y;
        for (int i = 0; i < levelCount; i++)
        {
            var pos = new Vector3(initialX + currentOffsetX, initialY + currentOffsetY, 0);
            var level = Instantiate(_levelButton, pos, Quaternion.identity);
            LevelButtonController controller = level.GetComponent<LevelButtonController>();
            controller.SetLevel(i);
            level.transform.parent = transform;
            if (i == 0)
            {
                EventSystem.current.SetSelectedGameObject(level);
            }

            currentOffsetX += _offset;
            if (currentOffsetX > _xMax)
            {
                currentOffsetX = 0;
                currentOffsetY -= _offset;
            }
        }
    }
}
