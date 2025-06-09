using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    private PlayerInput input;
    private InputAction shootAction;

    private Camera cam;

    public bool isShooting;
    public bool isReadyToShoot = true;
    private bool allowReset = true;
    public float shootingDelay = 2f; // a.k.a fire rate

    // Burst Settings
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    // Spreat Settings
    public float sprayIntensity;

    public enum ShootingMode
    {
        Single,
        Burst,
        Automatic
    }

    public ShootingMode currentMode;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;

    private void Start()
    {
        cam = Camera.main;
        input = GetComponentInParent<PlayerInput>();
        shootAction = input.actions["Shoot"];
        //shootAction.performed += _ => FireWeapon();

        burstBulletsLeft = bulletsPerBurst;
    }

    private void Update()
    {
        if (currentMode == ShootingMode.Automatic)
        {
            isShooting = shootAction.IsPressed();
        }
        else if (currentMode == ShootingMode.Single || currentMode == ShootingMode.Burst)
        {
            isShooting = shootAction.triggered;
        }

        if(isReadyToShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    public void FireWeapon()
    {
        isReadyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.transform.forward = shootingDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        if(allowReset)
        {
            Invoke(nameof(ResetShot), shootingDelay);
            allowReset = false;
        }

        if(currentMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke(nameof(FireWeapon), shootingDelay);
        }

        Destroy(bullet, 3f);
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;

        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(50);
        }

        Vector3 direction = targetPoint - bulletSpawnPoint.position;
        float x = UnityEngine.Random.Range(-sprayIntensity, sprayIntensity);
        float y = UnityEngine.Random.Range(-sprayIntensity, sprayIntensity);

        return direction + new Vector3(x, y, 0);
    }

    private void ResetShot()
    {
        isReadyToShoot = true;
        allowReset = true;
    }

}
