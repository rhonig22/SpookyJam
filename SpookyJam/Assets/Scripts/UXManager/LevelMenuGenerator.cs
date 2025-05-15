using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelMenuGenerator : MonoBehaviour
{
    [SerializeField] GameObject _levelButton;
    private int _widthOffset = 220;

    // Start is called before the first frame update
    void Start()
    {
        var levelCount = GameManager.Instance.GetCurrentWorldLevelCount();
        var currentOffset = 0;
        for (int i = 0; i < levelCount; i++)
        {
            var pos = new Vector3(transform.position.x + currentOffset, transform.position.y, 0);
            var level = Instantiate(_levelButton, pos, Quaternion.identity);
            LevelButtonController controller = level.GetComponent<LevelButtonController>();
            controller.SetLevel(i);
            level.transform.parent = transform;
            if (i == 0)
            {
                EventSystem.current.SetSelectedGameObject(level);
            }
            currentOffset += _widthOffset;
        }
    }
}
