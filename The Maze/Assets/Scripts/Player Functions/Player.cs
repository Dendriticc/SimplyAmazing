using UnityEngine;
public class Player : MonoBehaviour
{
    [SerializeField]
    public int health, speed;
    int baseSpeed = 5;

    private Animator animator;

    void Awake()
    {
        health = GameManager.startingHealth;
        FindObjectOfType<HealthText>().UpdateHealth(health);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GameManager.startingHealth = health;
        if (health <= 0 || GameManager.remainingTime <= 0)
        {
            FindObjectOfType<GameUIManager>().LoseGame();
            // died particles
            Destroy(gameObject);
        }
        
        if (Input.GetKey(KeyCode.Q) && Stamina.stamina > 0)
        {
            animator.SetBool("IsRunning", true);
            speed = baseSpeed * 2;
            Stamina.stamina -= Time.deltaTime * 10;
        }
        else 
        {
            animator.SetBool("IsRunning", false);
            speed = baseSpeed;
        }
    }
    public void Warp(Vector3 destination)
    {
        transform.position = destination;
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            health--;
            animator.Play("Ouch");
            GetComponent<AudioController>().EnemySound.Play();
            FindObjectOfType<CameraShake>().ShakeCamera(1f);
            FindObjectOfType<HealthText>().UpdateHealth(health);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Good Item")) 
        {
            GameManager.remainingTime += 30f;
            Stamina.stamina += 20f;
            GetComponent<AudioController>().GoodItemSound.Play();
            if (Random.Range(0, 1f) < 0.1)
            {
                health++;
                FindObjectOfType<HealthText>().UpdateHealth(health);
            }
            else if (Random.Range(0, 1f) < 0.1)
            {
                speed++;
            }
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Bad Item"))
        {
            GameManager.remainingTime -= 15f;
            GetComponent<AudioController>().BadItemSound.Play();
            FindObjectOfType<CameraShake>().ShakeCamera(0.5f);
            foreach (EnemyStateManager enemy in FindObjectsOfType<EnemyStateManager>()) enemy.SetTriggerLocation(transform.position);
            if (Random.Range(0, 1f) < 0.1)
            {
                speed--;
            }
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Finish"))
        {
            FindObjectOfType<GameUIManager>().WinGame();
            //winner particles
            Destroy(gameObject);
        }
    }
}
