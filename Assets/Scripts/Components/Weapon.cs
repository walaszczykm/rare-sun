using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class Weapon : Pickup
{
    public enum Model
    {
        PISTOL,
        SMG
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

    private const float SHOW_ANIM_DURATION = 0.3f;

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
    private int maxAmmo;
    public int MaxAmmo
    {
        get
        {
            return maxAmmo;
        }
    }
    private int ammo;
    public int Ammo
    {
        get
        {
            return ammo;
        }
        private set
        {
            ammo = value;
        }
    }

    private void Awake()
    {
        Reload();
    }

    public void Reload()
    {
        Ammo = MaxAmmo;
    }

    public void Show()
    {
        StartCoroutine(ShowAnim());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void OnShoot()
    {
        --Ammo;
        //TODO: shooting effect

    }

    protected override void OnPickedup(Player player)
    {
        player.AddWeapon(model);
    }

    private IEnumerator ShowAnim()
    {
        float currentTime = 0.0f;
        Vector3 startPos = new Vector3(0.0f, -0.25f, 0.0f);
        transform.localPosition = startPos;
        while (currentTime <= SHOW_ANIM_DURATION)
        {
            transform.localPosition = Vector3.Lerp(startPos, Vector3.zero, currentTime / SHOW_ANIM_DURATION);

            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;
        }
    }
}
