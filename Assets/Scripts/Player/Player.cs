using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{

    [SerializeField]
    private int health = 100;
    public int Health
    {
        get
        {
            return health;
        }
        private set
        {
            health = value;
            HUDManager.Instance.SetHP(health);
        }
    }
    [SerializeField]
    private Transform weaponPoint;
    [SerializeField]
    private Camera cam;
    public Camera Cam
    {
        get
        {
            return cam;
        }
    }

    private Dictionary<Weapon.Model, Weapon> weapons = new Dictionary<Weapon.Model, Weapon>();
    private Dictionary<Weapon.Model, Weapon> Weapons
    {
        get
        {
            return weapons;
        }
        set
        {
            weapons = value;
            HUDManager.Instance.SetWeaponModel(string.Empty);
            HUDManager.Instance.SetAP(0, 0);
        }
    }
    private Weapon.Model currentWeaponModel;
    public Weapon.Model CurrentWeaponModel
    {
        get
        {
            return currentWeaponModel;
        }
        private set
        {
            currentWeaponModel = value;
            HUDManager.Instance.SetWeaponModel(currentWeaponModel.ToString());
            HUDManager.Instance.SetAP(CurrentWeapon.Ammo, CurrentWeapon.MaxAmmo);
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
    private int points = 0;
    public int Points
    {
        get
        {
            return points;
        }
        private set
        {
            points = value;
            HUDManager.Instance.SetPoints(points);
        }
    }

    private void Start()
    {
        Health = 100;
        Points = 0; 
        ResetPlayer();
    }

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

    private IEnumerator ShootCoroutine()
    {
        while(true)
        {
            if(CurrentWeapon.Ammo <= 0)
            {
                break;
            }

            RaycastHit hit;

            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                CurrentWeapon.OnShoot();
                HUDManager.Instance.SetAP(CurrentWeapon.Ammo, CurrentWeapon.MaxAmmo);

                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if(enemy != null)
                {
                    enemy.Hit(CurrentWeapon.Damage);
                    //TODO: emit blood particles
                }
                else
                {
                    //TODO: emit sparks particles
                }
            }

            if(CurrentWeapon.ShootingModeType == Weapon.ShootingMode.SINGLE)
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

    private void SetCurrentWeaponGameObject()
    {
        foreach(Weapon weapon in Weapons.Values)
        {
            weapon.gameObject.SetActive(weapon.ModelType == CurrentWeaponModel);
        }
        CurrentWeapon.Show();
    }
}