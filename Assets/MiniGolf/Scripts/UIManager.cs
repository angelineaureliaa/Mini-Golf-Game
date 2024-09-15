using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private Image powerImage;
    [SerializeField] private Text shotText;
    [SerializeField] private GameObject mainMenu, gameMenu, gameOverPanel, retryBtn, nextBtn;
    [SerializeField] private GameObject lvlBtnPrefab, container;

    public Image PowerImage { get { return powerImage; } }

    public Text ShotText { get { return shotText; } }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        powerImage.fillAmount = 0;
    }

    private void Start()
    {
        //reload the same level or go to main menu 
        if(GameManager.singleton.gameStatus == GameStatus.NONE)
        {
            CreateLevelButtons();
        }
        else if (GameManager.singleton.gameStatus == GameStatus.FAILED || 
                 GameManager.singleton.gameStatus == GameStatus.COMPLETED)
        {
            mainMenu.SetActive(false);
            gameMenu.SetActive(true);
            LevelManager.instance.SpawnLevel(GameManager.singleton.currentLevelIndex);
        }
    }

    private void CreateLevelButtons()
    {
        //for loop 
        for (int i = 0; i < LevelManager.instance.LevelDatas.Length; i++)
        {
            GameObject lvlBtn = Instantiate(lvlBtnPrefab, container.transform);
            lvlBtn.transform.GetChild(0).GetComponent<Text>().text = "" + (i + 1);
            Button btn = lvlBtn.GetComponent<Button>();

            btn.onClick.AddListener(() => OnClick(btn));
        }
    }

    private void OnClick(Button btn)
    {
        mainMenu.SetActive(false);
        gameMenu.SetActive(true);
        GameManager.singleton.currentLevelIndex = btn.transform.GetSiblingIndex();
        LevelManager.instance.SpawnLevel(GameManager.singleton.currentLevelIndex);
    }

    public void GameResult()
    {
        gameOverPanel.SetActive(true);
        switch (GameManager.singleton.gameStatus)
        {
            case GameStatus.FAILED:
                retryBtn.SetActive(true);
                break;
            case GameStatus.COMPLETED:
                nextBtn.SetActive(true);
                break;
        }
    }

    public void HomeBtn()
    {
        GameManager.singleton.gameStatus = GameStatus.NONE;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RetryNextBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}