using UnityEngine;

public class OverworldEntranceManager : MonoBehaviour
{
    void Start()
    {
        int entrance = GameManager.Instance.CurrentEntrance;
        if (entrance == -1)
            return;

        var doors = GameObject.FindObjectsByType<Door>(FindObjectsSortMode.None);
        var player = GameObject.FindGameObjectWithTag("Ghost");
        foreach (var door in doors)
        {
            if (door.GetEntranceNumber() == entrance)
            {
                player.transform.position = door.GetEntrancePosition();
                break;
            }
        }
    }
}
