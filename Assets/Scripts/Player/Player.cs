using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{

    [SerializeField]
    private int health = 100;
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
    private Weapon.Model currentWeaponModel;
    private Weapon CurrentWeapon
    {
        get
        {
            Weapon weapon;
            weapons.TryGetValue(currentWeaponModel, out weapon);
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
    }

    private void Start()
    {
        ResetPlayer();
    }

    public void ResetPlayer()
    {
        transform.position = Vector3.zero;
        foreach(Weapon weapon in weapons.Values)
        {
            Destroy(weapon.gameObject);
        }
        weapons = new Dictionary<Weapon.Model, Weapon>();
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
            RaycastHit hit;

            if(Physics.Raycast(cam.transform.position, cam.transform.forward,
                out hit, CurrentWeapon.Range))
            {
                Debug.DrawLine(cam.transform.position, hit.point, Color.red);

                CurrentWeapon.OnShoot();
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
        if (hp > 0)
        {
            health += hp;
            if (health > 100)
            {
                health = 100;
            }
        }
    }

    public void AddPoints(int points)
    {
        this.points += points;
    }

    public void SwitchWeapon(int direction)
    {
        int current = (int)currentWeaponModel;
        int next = (current + direction);
        if(next < 0)
        {
            current = Weapon.ModelsNumber + next;
        }
        else
        {
            current = next % Weapon.ModelsNumber;
        }
        currentWeaponModel = (Weapon.Model)current;
        if(!weapons.ContainsKey(currentWeaponModel))
        {
            SwitchWeapon(direction);
        }
        
        SetCurrentWeaponGameObject();
    }

    public void AddWeapon(Weapon.Model model)
    {
        if(!weapons.ContainsKey(model))
        {
            GameObject weaponGO = Instantiate(Resources.Load<GameObject>(
                Paths.Prefabs.WEAPONS + model.ToString()));
            weaponGO.transform.SetParent(weaponPoint);
            weaponGO.transform.localPosition = Vector3.zero;
            weaponGO.transform.localEulerAngles = Vector3.zero;
            Weapon weapon = weaponGO.GetComponent<Weapon>();

            weapons.Add(model, weapon);

            currentWeaponModel = model;
            SetCurrentWeaponGameObject();
        }
    }

    private void SetCurrentWeaponGameObject()
    {
        foreach(Weapon weapon in weapons.Values)
        {
            weapon.gameObject.SetActive(weapon.ModelType == currentWeaponModel);
        }
    }
}