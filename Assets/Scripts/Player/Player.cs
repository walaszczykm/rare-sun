﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{
    #region Consts
    public const string HIGHSCORE_PREFS_KEY = "HIGHSCORE";
    #endregion

    #region SerializedFields
    [SerializeField]
    private int health = 100;
    [SerializeField]
    private Transform weaponPoint;
    [SerializeField]
    private Camera cam;
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
            health = value;
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
    private void Start()
    {
        Health = 100;
        Points = 0; 
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
        transform.position = Vector3.zero;
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
    }

    

    public void AddHealth(int hp)
    {
        if(hp > 0)
        {
            Health += hp;
            if (Health > 100)
            {
                Health = 100;
            }
        }
    }

    public void Hit()
    {
        Health -= 20;
        if(Health <= 0)
        {
            GameOver();
        }
    }

    public void AddPoints(int points)
    {
        Points += points;
    }

    public void SwitchWeapon(int direction)
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
                SwitchWeapon(direction);
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

            if (CurrentWeapon.ShootingModeType == Weapon.ShootingMode.SINGLE)
            {
                break;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
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
    #endregion
}