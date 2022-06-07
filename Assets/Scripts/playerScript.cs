using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{

    // variaveis de componentes
    private     Animator        playerAnimator;
    private     Rigidbody2D     playerRb;

    // variaveis de terreno
    public      Transform   groundCheck; //objeto responsavel por detectar se o personagem ta em uma superficie ou não
    public      LayerMask   whatIsGround; // indica o que é superficie para teste grounded

    //variaveis de movimento
    public      float       speed; // velocidade do personagem
    public      float       jumpForce; //força aplicada para gerar o pulo do personagem 

    //variaveis de animacao
    public      bool        Grounded; //indica que o personagem ta em cima de alguma superficie
    public      bool        lookLeft; // indica se o personagem está olhando para o lado esquerdo
    public      bool        attacking; //indica se o personagem esta atacando
    public      int         idAnimation; // indentificador de animação
    private     float       h, v; //variavel horizontal e vertical
    public      Collider2D  standing, crounching;// colisores de em pé ou agachado

    //interaçao com item e objetos
    private     Vector3     dir = Vector3.right;// define direcao do raycast(a direira)
    public      Transform   hand;// variavel de interacao por raycast
    public      LayerMask   interacao; // indica com o que pode interagir
    public      GameObject objetoInteracao;
    


    //variavel para sistema de armas
    public GameObject[] armas; 

    // Start is called before the first frame update
    void Start()
    {
        //associa componente a variaveis
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
    
        //laço foreach para desativar sprite das armas ao iniciar
        foreach(GameObject o in armas)
        {
            o.SetActive(false);
        } 

    }

    void FixedUpdate() // taxa de atualização fixa
    {
        //verifica colisão com o chão
        Grounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f, whatIsGround);

        // função que da movimento para o personagem
        playerRb.velocity = new Vector2(h * speed, playerRb.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        //atribuição de eixo
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // faz o personagem virar chamando a função flip
        if (h > 0 && lookLeft == true && attacking == false)
        {
            flip();
        }
        else if (h < 0 && lookLeft == false && attacking == false) { flip(); }

        // atribui animaçoes de andar e agachar as variaveis verticais e horizontais
        if (v < 0)
        {
            idAnimation = 2;
            h = 0; // impede o personagem de andar abaixado
        }

        else if (h != 0)
        {
            idAnimation = 1;

        }
        else idAnimation = 0;

        //comando de ataque
        if (Input.GetButtonDown("Fire1") && v >= 0 && attacking == false && objetoInteracao == null)
        {
            playerAnimator.SetTrigger("attack");
        }

        //comando de interacao
        if (Input.GetButtonDown("Fire1") && v >= 0 && attacking == false && objetoInteracao != null)
        {
            objetoInteracao.SendMessage("interacao", SendMessageOptions.DontRequireReceiver);
        }

        //comando de pulo
        if (Input.GetButtonDown("Jump") && Grounded == true && attacking == false)
        {
            playerRb.AddForce(new Vector2(0, jumpForce));

        }

        //função que para o personagem de atacar no chao
        if (attacking == true && Grounded == true)
        {
            h = 0;
        }

        // função que altera o hitbox para caso esteja abaixado ou em pé
        if (v < 0 && Grounded == true)// hitbox abaixado
        {
            crounching.enabled = true;
            standing.enabled = false;
        }
        else if (v >= 0 && Grounded == true)//hitbox em pé
        {
            crounching.enabled = false;
            standing.enabled = true;
        }
        else if (v != 0 && Grounded == false)//hitbox em pé ao pular
        {
            crounching.enabled = false;
            standing.enabled = true;
        }

        playerAnimator.SetBool("grounded", Grounded);
        playerAnimator.SetInteger("idAnimation", idAnimation);
        playerAnimator.SetFloat("speedY", playerRb.velocity.y);

        interagir();
    }


    //função que vira o personagem para a esquerda(flip)
    void flip()
    {
        lookLeft = !lookLeft;// inverte o valor da variavel booleana
        float x = transform.localScale.x;
        x *= -1; //inverte o sinal de scale X
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);

        // muda a direcao do raycast
        dir.x = x;
        dir = dir;
    }

    //estrutura de decisão para animação de ataque
    public void atack(int atk)
    {
        switch (atk)
        {
            case 0:
                attacking = false;
                armas[2].SetActive(false);// apos finalizar o ataque esse comando desativa os sprite das armas
                break;
            case 1:
                attacking = true;          
                break;
        }
    }

    //Funcoes para tratar colisão
    private void OnCollisionEnter2D(Collision2D collision) // usada ao iniciar uma colisão
    {
        if(collision.gameObject.tag == "espinhos")
        {
            playerAnimator.SetTrigger("hit");
        }
    }

    private void OnCollisionExit2D(Collision2D collision) // usado para quando se encerra uma colisão
    {
        if (collision.gameObject.tag == "Caixa")
        {
            print("deixou de colidir");
        }
    }

    private void OnCollisionStay2D(Collision2D collision) // usado enquanto houver colisão
    {
        if (collision.gameObject.tag == "Caixa")
        {
            playerAnimator.SetTrigger("hit");
        }
    }

    // Função para tratar Triggers
    private void OnTriggerEnter2D(Collider2D collision) // Funçao para entrada de um gatilho
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision) // função para saida de um gatilho
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)// funçao para enquanto um gatilho estiver ativo
    {
        
    }

    //funçao de interacao por raycast
    void interagir()
    {
        RaycastHit2D hit = Physics2D.Raycast(hand.position, dir, 0.2f, interacao);
        Debug.DrawRay(hand.position, dir*0.2f, Color.red);

        if(hit == true)
        {
            objetoInteracao = hit.collider.gameObject;
        }
        else
        {
            objetoInteracao = null;
        }
    }

    //função que habilita e desabilita os sprite(por evento no animation)
    void controleArma(int id)
    {
        //laço for each para desativar armas apos uso
        foreach (GameObject o in armas)
        {
            o.SetActive(false);
        }

        //habilita a arma pelo ID indexado por eventos
        armas[id].SetActive(true);
    }
 
}
