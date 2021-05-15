using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalColllider : MonoBehaviour
{
    private bool didItHitGround = false;
    private bool didItHitPlatformForFirstTime = false;

    private Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Finish") && !didItHitGround)
        {
            didItHitGround = true;
            GameManagerScript.Instance.numberOfBallsHitGround++;
            GameManagerScript.Instance.UpdateBallInfoText();
        }
        else if (collision.transform.CompareTag("Platform") && !didItHitPlatformForFirstTime)
        {
            rb.velocity = Vector3.zero;
            didItHitPlatformForFirstTime = true;
        }
    }
}
