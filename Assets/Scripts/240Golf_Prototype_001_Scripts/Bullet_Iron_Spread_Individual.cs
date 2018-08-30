using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*NOTE - WE WILL NEED TO BUILD A BULLET MANAGER CLASS THAT WE CAN ATTACH TO EVERY BULLET, WHICH WILL SOMEHOW
 KNOW WHICH BULLET IT IS AND THEN CALL THE APPROPRIATE FUNCTION FOR THAT BULLETS BEHAVIOR.  I REALLY DO NOT WANT 
 500 DIFFERENT SCRIPTS FOR EACH INDIVIDUAL BULLET*/

     

public class Bullet_Iron_Spread_Individual : MonoBehaviour
{
    [SerializeField] private GameObject itself;

    public Collider col;
    private bool starting = true; //is this the first bullet spawned?
    private Vector3 sizes; //controls the 
    private float lifespan;
    private Vector3 direction; //this will need to be forward based on the up vector of the bullet's current rotation, probablly gonna change with new object. 
    private int count = 1; 

    // Use this for initialization
    void Start ()
    {
        if (starting)
        {
            StartCoroutine(BreakUpBullet());
        }
        else
        {
            if (count <= 5)
            {
                transform.localScale = sizes;
                GetComponent<Rigidbody>().velocity = direction * 50.0f;
                StartCoroutine(BreakUpBullet());
            }
           
        }
	}

    IEnumerator BreakUpBullet()
    {
        yield return new WaitForSeconds(0.1f);
        //GameObject GO = Instantiate(itself, transform.position, transform.rotation) as GameObject;

        Vector3 scale = transform.localScale;
        scale.x = scale.x / 2.0f;
        scale.y = scale.y / 2.0f;
        scale.z = scale.z / 2.0f;

        //Vector3 deviation = (Vector3.up + -Vector3.right) * 0.5f;
        //Vector3 deviation = (transform.forward);
        GameObject GO = Instantiate(itself, transform.position, transform.rotation) as GameObject;
        Physics.IgnoreCollision(GO.GetComponent<Collider>(), col);
        Vector3 deviation = -transform.up;
        GO.GetComponent<Bullet_Iron_Spread_Individual>().col = col;
        GO.GetComponent<Bullet_Iron_Spread_Individual>().starting = false;
        GO.GetComponent<Bullet_Iron_Spread_Individual>().sizes = scale;
        GO.GetComponent<Bullet_Iron_Spread_Individual>().direction = deviation;
        GO.GetComponent<Bullet_Iron_Spread_Individual>().count = count + 1;
        KillProjectile(GO);

        GameObject GO1 = Instantiate(itself, transform.position, transform.rotation) as GameObject;
        Physics.IgnoreCollision(GO1.GetComponent<Collider>(), col);
        Vector3 deviation1 = (transform.forward + transform.right).normalized;
        GO1.GetComponent<Bullet_Iron_Spread_Individual>().col = col;
        GO1.GetComponent<Bullet_Iron_Spread_Individual>().starting = false;
        GO1.GetComponent<Bullet_Iron_Spread_Individual>().sizes = scale;
        GO1.GetComponent<Bullet_Iron_Spread_Individual>().direction = deviation1;
        GO1.GetComponent<Bullet_Iron_Spread_Individual>().count = count + 1;
        KillProjectile(GO1);

        GameObject GO2 = Instantiate(itself, transform.position, transform.rotation) as GameObject;
        Physics.IgnoreCollision(GO2.GetComponent<Collider>(), col);
        Vector3 deviation2 = (transform.forward + -transform.right).normalized;
        GO2.GetComponent<Bullet_Iron_Spread_Individual>().col = col;
        GO2.GetComponent<Bullet_Iron_Spread_Individual>().starting = false;
        GO2.GetComponent<Bullet_Iron_Spread_Individual>().sizes = scale;
        GO2.GetComponent<Bullet_Iron_Spread_Individual>().direction = deviation2;
        GO2.GetComponent<Bullet_Iron_Spread_Individual>().count = count + 1;
        KillProjectile(GO2);
    }

    private void KillProjectile(GameObject GO)
    {
        Destroy(GO, 0.5f);
    }
}
