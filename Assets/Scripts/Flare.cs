using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyFlare", 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroyFlare()
    {
        Destroy(gameObject);
    }
}
