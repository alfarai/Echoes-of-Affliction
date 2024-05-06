using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExplosionForce : MonoBehaviour
{
    private AudioManager audio;
    public GameObject explosion;
    private bool isThereHeatNearby,hasExploded;
    
    public float timeBeforeExplosion = 3.0f;
    // Start is called before the first frame update
    public float radius = 5.0F;
    public float power = 10.0F;

    void Start()
    {
        audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isThereHeatNearby)
        {
            timeBeforeExplosion -= Time.deltaTime;
            if(timeBeforeExplosion <= 0)
            {
                Explode();
            }
            
            return;
        }
        List<Collider> colliders = Physics.OverlapSphere(transform.position, 3).ToList();
        
        if (colliders.Find(collider => collider.gameObject.name == "Flames"))
        {
            isThereHeatNearby = true;
            
        }
        




    }
    void Explode()
    {
        //Debug.Log("Explode!");
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (Collider hit in colliders)
        {
            if(hit.name == "Large Barrier")
            {
                DataHub.ObjectiveHelper.hasExplodedBarrier = true;
                if (hit.gameObject.activeInHierarchy)
                {
                    hit.gameObject.SetActive(false);
                }
                
            }

            //explode other explosives caught in explosion
            if(hit.gameObject.TryGetComponent(out ExplosionForce explosive))
            {
                explosive.SetIsThereHeatNearby(true);
                continue;
            }
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                if (hit.gameObject.name == "Player")
                {

                    if (hit.gameObject.TryGetComponent(out Character player))
                    {
                        DataHub.PlayerStatus.damageTaken = 40;
                        player.TakeDamage(DataHub.PlayerStatus.damageTaken);
                    }
                    return; //dont add force to player since it does weird rotations that i dont have time to fix XD
                }

                if (!hasExploded)
                {
                    Object.Instantiate(explosion, transform.position, transform.rotation);

                }
                //Debug.Log(hit.gameObject.name);
                rb.AddExplosionForce(power * 1000, explosionPos, radius, 3.0F);
                audio.PlaySFX(audio.explosion);
                hasExploded = true;
                //gameObject.transform.GetChild(0).gameObject.SetActive(true); //play explode



                
                gameObject.SetActive(false);
            }

        }
    }
    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 3);
    }
    public void SetIsThereHeatNearby(bool x)
    {
        isThereHeatNearby = x;
    }
}
