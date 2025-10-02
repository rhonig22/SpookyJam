using UnityEngine;

public class CameraTriggerZone : MonoBehaviour, ILevelEntity
{
    [SerializeField] private BoxCollider2D _collider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<PlayerController>() != null)
        {
            CameraController.Instance.SwitchToSecondaryCam();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<PlayerController>() != null)
        {
            CameraController.Instance.SwitchToMainCam();
        }
    }

    public LevelEntity GetLevelEntity()
    {
        LevelEntity levelEntity = new LevelEntity();
        return GetLevelEntity(levelEntity);
    }

    public LevelEntity GetLevelEntity(LevelEntity levelEntity)
    {
        levelEntity.Size = _collider.size;
        return levelEntity;
    }

    public void SetLevelEntity(LevelEntity levelEntity)
    {
        _collider.size = levelEntity.Size;
    }
}
