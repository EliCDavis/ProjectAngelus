using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {
    
    [SerializeField]
    private float fireRate = 0;
    [SerializeField]
    private float damage = 10;
    [SerializeField]
    private LayerMask whatToHit;
    [SerializeField]
    private Transform BulletTrailPrefab;

    private float timeToFire = 0;
    private Transform firePoint;
    private bool m_DidHit;
    // Use this for initialization
    void Awake()
    {
        firePoint = transform.FindChild("Fire");
        if (firePoint == null)
        {
            Debug.LogError("No FirePoint");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ShootF();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                ShootF();
            }
        }
    }

    void ShootF()
    {
        //Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector3 firePointPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        m_DidHit = Physics.Raycast(firePointPosition, firePointPosition, 100, whatToHit);
        //Effect();
        Debug.DrawLine(firePointPosition, (firePointPosition) * 100, Color.cyan);
        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Debug.Log("We hit" + hit.collider.name + " and did " + damage + " some damage");
        }
    }
    void Effect()
    {
        Instantiate(BulletTrailPrefab, firePoint.position, Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, 90 + firePoint.eulerAngles.z));
    }
}
