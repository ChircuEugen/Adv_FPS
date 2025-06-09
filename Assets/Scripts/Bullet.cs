using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            CreateBulletImactEffect(collision);
            Debug.Log("!!!");
        }
        Destroy(gameObject);
    }

    private void CreateBulletImactEffect(Collision collision)
    {
        ContactPoint contactPoint = collision.GetContact(0);

        GameObject hole = Instantiate(GlobalReferences.Instance.bulletImpactEffectPrefab, 
            contactPoint.point, Quaternion.LookRotation(contactPoint.normal));

        hole.transform.SetParent(collision.transform);
    }

}
