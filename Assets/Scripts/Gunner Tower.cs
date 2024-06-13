using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GunnerTower : MonoBehaviour
{
    private CircleCollider2D Range;
    public List<GameObject> PossibleTargets = new List<GameObject>();
    private GameObject Target;
    public GameObject bullet;
    public float ShootTime;
    public int Upgrade = 0;

    // Start is called before the first frame update
    void Start()
    {
        Range = GetComponent<CircleCollider2D>();
        StartCoroutine(StartShooting());
    }

    // Update is called once per frame
    void Update()
    {
        if(PossibleTargets.Count != 0)
        {
            transform.right = Target.transform.position - transform.position;
        }
    }

    IEnumerator StartShooting()
    {
        if(PossibleTargets.Count != 0)
        {
            GameObject Bullet = Instantiate(bullet,transform.position,Quaternion.identity);
            Bullet.GetComponent<Bullet>().Move(Target.transform.position - transform.position);
        }
        yield return new WaitForSeconds(ShootTime);
        StartCoroutine(StartShooting());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            PossibleTargets.Add(collision.gameObject);
            TargetStrongest();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            PossibleTargets.Remove(collision.gameObject);
            TargetStrongest();
        }
    }
    public void UpgradeTower()
    {
        switch(Upgrade)
        {
            case 0:
            {
                ShootTime = ShootTime * 0.6f;
                Range.radius = 4f;
                break;
            }
            case 1: 
            {
                ShootTime = ShootTime * 0.6f;
                Range.radius = 5f;
                bullet.GetComponent<Bullet>().damage = 10f;
                break;
            }
        }
        Upgrade++;
    }
    public void DestroyTower()
    {
        Destroy(transform.parent.gameObject);
    }
    public void TargetFirst()
    {
        if (PossibleTargets.Count == 0){ return; }
        Target = PossibleTargets[0];
        Invoke("TargetFirst", 1f);
    }
    public void TargetLast()
    {
        if (PossibleTargets.Count == 0) { return; }
        Target = PossibleTargets.Last();
        Invoke("TargetLast", 1f);
    }
    public void TargetStrongest()
    {
        if (PossibleTargets.Count == 0) { return; }
        float maxhealth = 0f;
        foreach (var target in PossibleTargets)
        {
            if(target.GetComponent<EnemyHealth>().MaxHealth > maxhealth)
            {
                maxhealth = target.GetComponent<EnemyHealth>().MaxHealth;
                Target = target;
            }
        }
        Invoke("TargetStrongest", 1f);
    }
    public void TargetWeakest() 
    {
        if (PossibleTargets.Count == 0) { return; }
        float minhealth = 9999999f;
        foreach (var target in PossibleTargets)
        {
            if(target.GetComponent<EnemyHealth>().MaxHealth < minhealth)
            {
                minhealth = target.GetComponent<EnemyHealth>().MaxHealth;
                Target = target;
            }
        }
        Invoke("TargetWeakest", 1f);
    }
}
