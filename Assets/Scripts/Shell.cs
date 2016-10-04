using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{
    //The force applied to the shell when spawned out of the gun
    public float forceMinimum = 100.0f;
    public float forceMaximum = 200.0f;
    public float fadeTime = 2.0f;
    public float lifetime = 4.0f;

    //Privates
    Rigidbody body;

    //Apply a force to the body
    void Start ()
    {
        //Find
        body = GetComponent<Rigidbody>();
	    
        if(body != null)
        {
            //Find the force
            float force = Random.Range(forceMinimum, forceMaximum);

            //Apply
            body.AddForce(transform.right * force);
            body.AddTorque(Random.insideUnitSphere * force);

            //Begin
            StartCoroutine(Fade());
        }
	}
	
	//The shell should fade out and delete after a certain amount of time
    IEnumerator Fade()
    {
        //Needs to exist for certain amount of time
        yield return new WaitForSeconds(lifetime);

        //Fade our the material
        float percent = 0.0f;
        Material attached = GetComponent<Renderer>().material;
        Color inital = attached.color;

        while(percent < 1)
        {
            percent += Time.deltaTime * (1 / fadeTime);
            attached.color = Color.Lerp(inital, Color.clear, percent);
            yield return null;
        }

        //Now remove
        Destroy(gameObject);
    }
}
