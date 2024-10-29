using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [SerializeField] private Animator animator;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartLevelTransition();
    }

    public void RestartLevelTransition()
    {
        animator.SetTrigger("RestartLevel");
    }

    public void EndLevelTransition()
    {
        animator.SetTrigger("EndLevel");
    }

    public void StartLevelTransition()
    {
        animator.SetTrigger("StartLevel");
    }

    public void LoadNextLevel()
    {
        GameManager.Instance.LoadNextLevel();
    }

    public void RestartLevel()
    {
        GameManager.Instance.ReloadLevel();
    }
}
