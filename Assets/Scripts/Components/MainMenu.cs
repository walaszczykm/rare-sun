using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    #region SerializedFields
    [SerializeField]
    private GameObject mainMenu, settingsMenu;
    [SerializeField]
    private InputField mazeRows, mazeColumns;
    [SerializeField]
    private Dropdown mazeAlgorithm;
    [SerializeField]
    private Slider loadingBar;
    #endregion

    #region MonoBehaviour methods
    private void Awake()
    {
        SetAlgorithmDropdownTexts();
        SetSettingsValuesFromPrefs();
    }

    private void Start()
    {
        ShowMainMenu();
    }
    #endregion

    #region Public methods
    public void OnNewGameButtonClicked()
    {
        StartCoroutine(LoadGameSceneAsyncCoroutine());
    }

    public void OnSettingsButtonClicked()
    {
        ShowSettingsMenu();
    }

    public void OnSaveSettingsButtonClicked()
    {
        int rows, columns, value;
        if(int.TryParse(mazeRows.text, out rows))
        {
            PlayerPrefs.SetInt(LevelGenerator.MAZE_ROWS_PREFS_KEY, rows);
        }
        if(int.TryParse(mazeColumns.text, out columns))
        {
            PlayerPrefs.SetInt(LevelGenerator.MAZE_COLUMNS_PREFS_KEY, columns);
        }

        value = mazeAlgorithm.value;
        string algorithmName = mazeAlgorithm.options[value].text;
        PlayerPrefs.SetString(LevelGenerator.MAZE_ALGORITHM_PREFS_KEY, algorithmName);

        PlayerPrefs.Save();
        ShowMainMenu();
    }

    public void OnBackButtonClicked()
    {
        ShowMainMenu();
    }
    #endregion

    #region Private methods
    private void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        loadingBar.gameObject.SetActive(false);
    }

    private void ShowSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        loadingBar.gameObject.SetActive(false);
    }

    private void ShowLoadingBar()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        loadingBar.gameObject.SetActive(true);
    }

    private void SetAlgorithmDropdownTexts()
    {
        mazeAlgorithm.ClearOptions();

        List<Dropdown.OptionData> data = new List<Dropdown.OptionData>();
        foreach(string algorithmName in MazeAlgorithms.MazeAlgorithm.Algorithms.Keys)
        {
            data.Add(new Dropdown.OptionData(algorithmName));
        }

        mazeAlgorithm.AddOptions(data);
    }

    private void SetSettingsValuesFromPrefs()
    {
        if(PlayerPrefs.HasKey(LevelGenerator.MAZE_ROWS_PREFS_KEY))
        {
            mazeRows.text = PlayerPrefs.GetInt(LevelGenerator.MAZE_ROWS_PREFS_KEY).ToString();
        }
        if(PlayerPrefs.HasKey(LevelGenerator.MAZE_COLUMNS_PREFS_KEY))
        {
            mazeColumns.text = PlayerPrefs.GetInt(LevelGenerator.MAZE_COLUMNS_PREFS_KEY).ToString();
        }
        if(PlayerPrefs.HasKey(LevelGenerator.MAZE_ALGORITHM_PREFS_KEY))
        {
            string algorithmName = PlayerPrefs.GetString(LevelGenerator.MAZE_ALGORITHM_PREFS_KEY);
            foreach(Dropdown.OptionData item in mazeAlgorithm.options)
            {
                if(item.text == algorithmName)
                {
                    mazeAlgorithm.value = mazeAlgorithm.options.IndexOf(item);
                    break;
                }
            }
        }
    }

    private IEnumerator LoadGameSceneAsyncCoroutine()
    {
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("Game");
        sceneLoading.allowSceneActivation = true;

        ShowLoadingBar();

        while(!sceneLoading.isDone)
        {
            loadingBar.value = sceneLoading.progress;
            yield return new WaitForEndOfFrame();
        }

    }
    #endregion
}
