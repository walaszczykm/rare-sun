using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public enum SFX
    {
        Hit,
        Dash,
        Coin,
        NextLevel,
        SwitchWeapon,
        Shoot
    }

    #region Consts
    public const string HIGHSCORE_PREFS_KEY = "HIGHSCORE";
    private const float SHOOTING_RATE = 0.05f;
    #endregion

    #region SerializedFields
    [SerializeField]
    private int health = 100;
    [SerializeField]
    private Transform weaponPoint;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Light shootingLight;
    [SerializeField]
    private AudioSource audioSource;

    private Dictionary<SFX, AudioClip> sfxClips;
    #endregion

    #region PrivateFields
    private Dictionary<Weapon.Model, Weapon> weapons = new Dictionary<Weapon.Model, Weapon>();
    private Weapon.Model currentWeaponModel;
    private int points = 0;
    #endregion

    #region Properties
    public int Health
    {
        get
        {
            return health;
        }
        private set
        {
            health = Mathf.Clamp(value, 0, 100);
            UIManager.Instance.SetHP(health);
        }
    }
    public Camera Cam
    {
        get
        {
            return cam;
        }
    }

    private Dictionary<Weapon.Model, Weapon> Weapons
    {
        get
        {
            return weapons;
        }
        set
        {
            weapons = value;
            UIManager.Instance.SetWeaponModel(string.Empty);
            UIManager.Instance.SetAP(0, 0);
        }
    }
    public Weapon.Model CurrentWeaponModel
    {
        get
        {
            return currentWeaponModel;
        }
        private set
        {
            PlaySfx(SFX.SwitchWeapon);
            currentWeaponModel = value;
            UIManager.Instance.SetWeaponModel(currentWeaponModel.ToString());
            UIManager.Instance.SetAP(CurrentWeapon.Ammo, CurrentWeapon.MaxAmmo);
        }
    }
    private Weapon CurrentWeapon
    {
        get
        {
            Weapon weapon;
            Weapons.TryGetValue(CurrentWeaponModel, out weapon);
            return weapon;
        }
    }
    public int Points
    {
        get
        {
            return points;
        }
        private set
        {
            points = value;
            UIManager.Instance.SetPoints(points);
        }
    }
    #endregion

    #region MonoBehaviour methods
    private void Awake()
    {
        Health = 100;
        Points = 0;
        shootingLight.enabled = false;
    }

    private void Start()
    {
        InitSfxsDict();
        ResetPlayer();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
    #endregion

    #region Public methods
    public void ResetPlayer()
    {
        transform.position = Vector3.up;
        foreach(Weapon weapon in Weapons.Values)
        {
            Destroy(weapon.gameObject);
        }
        Weapons = new Dictionary<Weapon.Model, Weapon>();
    }

    public void Shoot()
    {
        if(CurrentWeapon != null)
        {
            StartCoroutine("ShootCoroutine");
        }
    }

    public void StopShooting()
    {
        StopCoroutine("ShootCoroutine");
        shootingLight.enabled = false;
    }

    

    public void RestoreHealth(int hp)
    {
        Health += hp;
        Debug.Log("Player.RestoreHealth(" + hp + ")");
    }

    public void Hit()
    {
        PlaySfx(SFX.Hit);
        Health -= 20;
        if(Health <= 0)
        {
            GameOver();
        }
    }

    public void AddPoints(int points)
    {
        PlaySfx(SFX.Coin);
        Points += points;
    }

    public void SwitchWeapon(int number)
    {
        if (number < Weapon.ModelsNumber)
        {
            Weapon.Model model = (Weapon.Model)number;
            Weapon weapon;
            if(Weapons.TryGetValue(model, out weapon))
            {
                CurrentWeaponModel = weapon.ModelType;
                SetCurrentWeaponGameObject();
            }
        }
    }

    public void SwitchWeaponWithDirection(int direction)
    {
        if(Weapons.Count > 0)
        {
            int current = (int)currentWeaponModel;
            int next = (current + direction);
            if (next < 0)
            {
                current = Weapon.ModelsNumber + next;
            }
            else
            {
                current = next % Weapon.ModelsNumber;
            }
            
            currentWeaponModel = (Weapon.Model)current;
            if(!Weapons.ContainsKey(currentWeaponModel))
            {
                SwitchWeaponWithDirection(direction);
            }
            CurrentWeaponModel = currentWeaponModel;
            SetCurrentWeaponGameObject();
        }
    }

    public void AddWeapon(Weapon.Model model)
    {
        if(!Weapons.ContainsKey(model))
        {
            GameObject weaponGO = Instantiate(Resources.Load<GameObject>(
                Paths.Prefabs.WEAPONS + model.ToString()));
            weaponGO.transform.SetParent(weaponPoint);
            weaponGO.transform.localPosition = Vector3.zero;
            weaponGO.transform.localEulerAngles = Vector3.zero;
            Weapon weapon = weaponGO.GetComponent<Weapon>();

            Weapons.Add(model, weapon);

            CurrentWeaponModel = model;
            SetCurrentWeaponGameObject();
        }
        else
        {
            Weapons[model].Reload();
            if(model == CurrentWeaponModel)
            {
                CurrentWeaponModel = model;
            }
        }
    }

    public void PlaySfx(SFX sfx)
    {
        audioSource.Stop();
        audioSource.clip = sfxClips[sfx];
        audioSource.Play();
    }
    #endregion

    #region Private methods
    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            if (CurrentWeapon.Ammo <= 0)
            {
                break;
            }

            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                CurrentWeapon.OnShoot();
                UIManager.Instance.SetAP(CurrentWeapon.Ammo, CurrentWeapon.MaxAmmo);

                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Hit(CurrentWeapon.Damage);
                    //TODO: emit blood particles
                }
                else
                {
                    //TODO: emit sparks particles
                }
            }

            PlaySfx(SFX.Shoot);
            shootingLight.enabled = !shootingLight.enabled;
            yield return new WaitForSeconds(SHOOTING_RATE);

            if (CurrentWeapon.ShootingModeType == Weapon.ShootingMode.Single)
            {
                break;
            }
        }

        shootingLight.enabled = false;
        yield return null;
    }

    private void SetCurrentWeaponGameObject()
    {
        foreach(Weapon weapon in Weapons.Values)
        {
            weapon.gameObject.SetActive(weapon.ModelType == CurrentWeaponModel);
        }
        CurrentWeapon.Show();
    }

    private void GameOver()
    {
        ManageSavedHighScore();

        cam.transform.SetParent(null);
        LevelGenerator.Instance.DestroyLevel();
        UIManager.Instance.SetEndGameLayout();
        
        if (CurrentWeapon != null)
        {
            Destroy(CurrentWeapon.gameObject);
        }
        Destroy(gameObject);
    }

    private void ManageSavedHighScore()
    {
        if(PlayerPrefs.HasKey(HIGHSCORE_PREFS_KEY))
        {
            int highscore = PlayerPrefs.GetInt(HIGHSCORE_PREFS_KEY);
            if(points > highscore)
            {
                PlayerPrefs.SetInt(HIGHSCORE_PREFS_KEY, points);
                PlayerPrefs.Save();
            }
        }
        else
        {
            PlayerPrefs.SetInt(HIGHSCORE_PREFS_KEY, points);
            PlayerPrefs.Save();
        }
    }

    private void InitSfxsDict()
    {
        sfxClips = new Dictionary<SFX, AudioClip>();
        foreach (string sfxName in Enum.GetNames(typeof(SFX)))
        {
            sfxClips.Add((SFX)Enum.Parse(typeof(SFX), sfxName), 
                Resources.Load<AudioClip>(Paths.Audio.SFXS + sfxName));
        }
    }
    #endregion
}