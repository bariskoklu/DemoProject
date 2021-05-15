using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float horizontalSpeed;

    private float horizontalInput;

    void Update()
    {
        if (GameManagerScript.Instance.currentGameState != GameState.Playing)
        {
            return;
        }

#if UNITY_EDITOR
        horizontalInput = Input.GetAxis("Horizontal");
#endif

#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            var touch = Input.touches[0];
            if (touch.position.x < Screen.width / 2)
            {
                horizontalInput = -1;
            }
            else if (touch.position.x > Screen.width / 2)
            {
                horizontalInput = 1;
            }
        }
#endif
        transform.position = new Vector3(transform.position.x + (horizontalInput * Time.deltaTime * horizontalSpeed), transform.position.y, transform.position.z + (speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("FinishLine") && GameObject.ReferenceEquals(GameManagerScript.Instance.currentLevel, collision.transform.parent.gameObject))
        {
            GameManagerScript.Instance.CheckForLevelEnd();
        }
    }
}
