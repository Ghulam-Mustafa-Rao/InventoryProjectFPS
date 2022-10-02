using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : Character
{
    float verticalInput;
    float horizontalInput;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.gameManager.chracters.Add(this);
        GameManager.gameManager.charactersNameKills.Add(cName, kills);
        //Debug.LogError(GameManager.gameManager.chracters.Count);
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if(verticalInput !=0)
        {
            rigidbody.AddForce(transform.forward * speed * verticalInput, ForceMode.Impulse);

            if (rigidbody.velocity.magnitude > maxSpeed)
            {
                rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);
            }
        }
        else
        {
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, 0);
        }

        transform.Rotate(Vector3.up * rotateSpeed * horizontalInput);
    }


}
