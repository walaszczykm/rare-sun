using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class Weapon : MonoBehaviour
{
    public enum Model
    {
        PISTOL,
        SMG,
        GRANTE
    }

    public enum ShootingMode
    {
        SINGLE,
        AUTOMATIC
    }

    private static int modelsNumber = Enum.GetNames(typeof(Model)).Length;
    public static int ModelsNumber
    {
        get
        {
            return modelsNumber;
        }
    }


    [SerializeField]
    private Model model;
    public Model ModelType
    {
        get
        {
            return model;
        }
    }
    [SerializeField]
    private ShootingMode shootingMode;
    public ShootingMode ShootingModeType
    {
        get
        {
            return shootingMode;
        }
    }
    [SerializeField]
    private int damage;
    public int Damage
    {
        get
        {
            return damage;
        }
    }
    [SerializeField]
    private int range;
    public int Range
    {
        get
        {
            return range;
        }
    } 

    private void OnEnable()
    {
        //animation
    }

    public void OnShoot()
    {
        //TODO: shooting effect
    }
}
