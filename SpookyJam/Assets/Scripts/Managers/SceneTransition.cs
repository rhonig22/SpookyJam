using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [SerializeField] private Animator animator;
    private Action _finishTransitionAction;

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

    public void EndLevelTransition(Action finishTransitionAction)
    {
        _finishTransitionAction = finishTransitionAction;
        animator.SetTrigger("EndLevel");
    }

    public void StartLevelTransition()
    {
        animator.SetTrigger("StartLevel");
    }

    public void LoadNextLevel()
    {
        _finishTransitionAction();
    }

    public void RestartLevel()
    {
        GameManager.Instance.ReloadLevel();
    }
}
