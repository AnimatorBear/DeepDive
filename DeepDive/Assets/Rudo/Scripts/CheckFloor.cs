using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFloor : MonoBehaviour
{

    public WheelCollider[] wheels;
    public CartTest cart;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool hadCurb = false;
        int offRoads = 0;
        for(int i = 0; i < wheels.Length; i++)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(wheels[i].transform.position, wheels[i].transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(wheels[i].transform.position, wheels[i].transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
                Debug.Log("Did Hit " + hit.collider.gameObject.tag);
                switch(hit.collider.gameObject.tag)
                {
                    case "Curb":
                        RumbleManager.instance.RumblePulse(0, rb.velocity.magnitude / 25, 0.1f);
                        hadCurb = true;
                        break;
                    case "Offroad":
                        hadCurb = true;
                        offRoads++;
                        break;
                }
            }
        }
        if (!hadCurb)
        {
            RumbleManager.instance.currentDuration = 0;
        }
        if(offRoads >= 3)
        {
            RumbleManager.instance.RumblePulse(0, rb.velocity.magnitude / 200, 0.1f);
            cart.wheelDamageMultiplier = 2f;
        }else if(offRoads >= 1)
        {
            RumbleManager.instance.RumblePulse(0, rb.velocity.magnitude / 500, 0.1f);
            cart.wheelDamageMultiplier = 1.5f;
        }
        else
        {
            cart.wheelDamageMultiplier = 1;
        }
    }
}
