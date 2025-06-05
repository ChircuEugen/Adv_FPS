using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    private PlayerInput input;
    private InputAction shootAction;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;

    private void Start()
    {
        input = GetComponentInParent<PlayerInput>();
        shootAction = input.actions["Shoot"];
        //shootAction.performed += _ => FireWeapon();
    }

    private void Update()
    {
        if(shootAction.triggered)
        {
            FireWeapon();
        }
    }

    public void FireWeapon()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnPoint.forward.normalized * bulletVelocity, ForceMode.Impulse);
        Destroy(bullet, 3f);
    }


    private void OnDisable()
    {
        shootAction.performed -= _ => FireWeapon();
    }
}
