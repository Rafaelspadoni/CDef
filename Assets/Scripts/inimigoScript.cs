using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigoScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //fun�ao que detecta colisao com a arma
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch( collision.gameObject.tag)
        {
            case "Arma":
                print("tomei dano");
                break;
        }
    }
}
