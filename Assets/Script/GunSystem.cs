using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class GunSystem : MonoBehaviour
{
    // Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    private int bulletsLeft, bulletsShot;

    // Bools
    private bool shooting, readyToShoot, reloading;

    // References
    public Camera fpsCam;
    public Transform attackPoint;
    public LayerMask whatIsEnemy;

    // Graphics
    public GameObject muzzleFlashPrefab, bulletHoleGraphic;
    public CamShake camShake;
    public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;
    public Animator gunAnimator;

    private Queue<GameObject> muzzleFlashPool = new Queue<GameObject>();
    public int poolSize = 5;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        InitializeMuzzleFlashPool();
    }

    private void Start()
    {
        ValidateEnemyTag();
    }

    private void Update()
    {
        HandleInput();
        UpdateAmmoText();
    }

    private void InitializeMuzzleFlashPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject muzzleFlash = Instantiate(muzzleFlashPrefab);
            muzzleFlash.SetActive(false);
            muzzleFlashPool.Enqueue(muzzleFlash);
        }
    }

    private GameObject GetMuzzleFlash()
    {
        if (muzzleFlashPool.Count > 0)
        {
            GameObject muzzleFlash = muzzleFlashPool.Dequeue();
            muzzleFlash.SetActive(true);
            return muzzleFlash;
        }
        else
        {
            GameObject newMuzzleFlash = Instantiate(muzzleFlashPrefab);
            return newMuzzleFlash;
        }
    }

    private void ReturnMuzzleFlash(GameObject muzzleFlash)
    {
        muzzleFlash.SetActive(false);
        muzzleFlashPool.Enqueue(muzzleFlash);
    }

    private void HandleInput()
    {
        if (allowButtonHold)
            shooting = Input.GetMouseButton(0);
        else
            shooting = Input.GetMouseButtonDown(0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
            Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!readyToShoot || bulletsLeft <= 0) return;

        Debug.Log("Shooting!");
        readyToShoot = false;

        if (gunAnimator != null)
            gunAnimator.SetTrigger("Shoot");

        for (int i = 0; i < bulletsPerTap; i++)
        {
            Vector3 direction = CalculateSpread();
            if (Physics.Raycast(fpsCam.transform.position, direction, out RaycastHit rayHit, range))
            {
                Debug.Log("Hit: " + rayHit.collider.name);

                if (bulletHoleGraphic != null)
                    Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.LookRotation(rayHit.normal));

                if (rayHit.collider.CompareTag("Enemy"))
                {
                    Enemy enemy = rayHit.collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage);
                    }
                    else
                    {
                        Debug.LogWarning("Hit an enemy, but no Enemy script found!");
                    }
                }
            }

            if (muzzleFlashPrefab != null)
            {
                GameObject muzzleFlash = GetMuzzleFlash();
                muzzleFlash.transform.position = attackPoint.position;
                muzzleFlash.transform.rotation = Quaternion.identity;
                StartCoroutine(DisableMuzzleFlash(muzzleFlash, 1f));
            }
        }

        if (camShake != null)
            camShake.Shake(camShakeDuration, camShakeMagnitude);

        bulletsLeft -= bulletsPerTap;
        bulletsShot -= bulletsPerTap;

        Debug.Log("Bullets left: " + bulletsLeft);
        Invoke(nameof(ResetShot), timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShots);
    }
    private IEnumerator DisableMuzzleFlash(GameObject muzzleFlash, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnMuzzleFlash(muzzleFlash);
    }

    private Vector3 CalculateSpread()
    {
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        return fpsCam.transform.forward + new Vector3(x, y, 0);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke(nameof(ReloadFinished), reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private void UpdateAmmoText()
    {
        if (text != null)
            text.SetText(bulletsLeft + " / " + magazineSize);
    }

    private void ValidateEnemyTag()
    {
        if (GameObject.FindWithTag("Enemy") == null)
        {
            Debug.LogWarning("No objects with tag 'Enemy' found in the scene. Make sure to set the correct tag on enemies.");
        }
    }
}
