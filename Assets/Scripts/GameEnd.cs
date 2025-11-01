using System;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    [SerializeField]
    private GameObject gameEndSceen;

    [SerializeField] private GameObject doFvolume;

    public void Awake()
    {
        gameEndSceen.SetActive(false);
        doFvolume.SetActive(false);
    }

    public void ShowOnGameEndSceen()
    {
        gameEndSceen.SetActive(true);
        doFvolume.SetActive(true);
    }
}
