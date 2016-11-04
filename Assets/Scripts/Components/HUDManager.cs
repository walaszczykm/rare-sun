using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private Text points, hp, weaponModel, ap;

    private static HUDManager instance;
    public static HUDManager Instance
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
