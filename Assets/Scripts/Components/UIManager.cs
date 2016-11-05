using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text points, points2, hp, weaponModel, ap;
    [SerializeField]
    private GameObject HUD, gameOverScreen;

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    
    private void Start()
    {
        SetGameLayout();
    }

    public void SetGameLayout()
    {
        HUD.SetActive(true);
        gameOverScreen.SetActive(false);
    }

    public void SetEndGameLayout()
    {
        gameOverScreen.SetActive(true);
        points2.text = points.text;
        HUD.SetActive(false);
    }

    public void SetPoints(int value)
    {
        points.text = value.ToString();
    }

    public void SetHP(int value)
    {
        hp.text = "HP: " + value.ToString();
    }

    public void SetAP(int value, int maxValue)
    {
        if(maxValue > 0)
        {
            ap.text = value.ToString() + "/" + maxValue.ToString();
        }
        else
        {
            ap.text = string.Empty;
        }
    }

    public void SetWeaponModel(string modelName)
    {
        weaponModel.text = modelName;
    }
}
