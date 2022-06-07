using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestScript : MonoBehaviour
{
    private _GameController _GameController;
    
    private SpriteRenderer spriteRenderer;
    public Sprite[] imagemObjeto;// array para poder trocar o sprite
    public bool open;

    // Start is called before the first frame update
    void Start()
    {

        // comando para chamar/achar outro script
        _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //função de interaçao entre scripts
    public void interacao()
    {
        //comando de abrir e fechar bau
        open = !open;
        
        //Funçao que abre e fecha 
        switch(open)
        {
            case true:
                spriteRenderer.sprite = imagemObjeto[1];
                //_GameController.teste += 1;
                //funçao para caso o objeto nao seja carregado(verificação)
                /*if (_GameController == null)
                {
                    _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
                }*/
                break;

            case false:
                spriteRenderer.sprite = imagemObjeto[0];
                break;
        }

        //comando de abertura unica
        /*if (open == false)
        {
            open = true;
            spriteRenderer.sprite = imagemObjeto[1];
        }
        */
    }
}
