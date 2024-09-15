using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    [HideInInspector]
    public int currentLevelIndex = 0;
    [HideInInspector] public GameStatus gameStatus = GameStatus.NONE;
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}

[System.Serializable]
public enum GameStatus
{
    NONE,
    PLAYING, 
    FAILED,
    COMPLETED
}
