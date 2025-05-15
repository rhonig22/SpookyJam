using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] GameObject _worldButton;
    private int _widthOffset = 220;

    // Start is called before the first frame update
    void Start()
    {
        var worldCount = GameManager.Instance.GetWorldCount();
        var currentOffset = 0;
        for (int i = 0; i < worldCount; i++)
        {
            var pos = new Vector3(transform.position.x + currentOffset, transform.position.y, 0);
            var world = Instantiate(_worldButton, pos, Quaternion.identity);
            WorldButtonController controller = world.GetComponent<WorldButtonController>();
            controller.SetWorld(i+1);
            world.transform.parent = transform;
            if (i == 0)
            {
                EventSystem.current.SetSelectedGameObject(world);
            }
            currentOffset += _widthOffset;
        }
    }
}
