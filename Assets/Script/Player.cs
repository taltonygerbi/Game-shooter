using System.Collections;
using UnityEngine;




public class Player : MonoBehaviour
{
    public float speed = 2;
    public float jumpForce = 10;
    public int grounded = 0;
    public float mouseSensitivity = 100f;



    [Header("HealthBar")]
    private float CurHealth;
    public float MaxHealth;
    [SerializeField] private HealthBarUI healthBar;
    public float DaleySpeedHealth;
    private float targetHealth;

    [Header("Attack")]
    public float attackRange = 2.0f;
    public float attackDamage = 20f;
    public GameObject AttackOfPlayer;

    private float yaw = 0f;
    [SerializeField]private Rigidbody rb;

    void Start()
    {
        
        if(rb == null)rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        if (MaxHealth <= 0) MaxHealth = 100;
        CurHealth = MaxHealth;
        targetHealth = CurHealth;

        healthBar.SetMaxHealth(MaxHealth);
        healthBar.SetHealth(CurHealth);

        StartCoroutine(MakeMeTest());
    }

    public IEnumerator MakeMeTest()
    {
        int myCountr = 1;
        Debug.Log("test start here");
        yield return new WaitForSeconds(3);
        Debug.Log("wow it passed 3 sec");
        yield return new WaitForSeconds(1);
        Debug.Log("this is amazing !!!");
        yield return new WaitForSeconds(1);
        while (myCountr <100) {
            Debug.Log(myCountr);
            myCountr++;
            yield return null; 
        }
    }

    void Update()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, yaw, 0);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = transform.forward * z + transform.right * x;
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        if (Input.GetKeyDown(KeyCode.Space) && grounded < 2)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            grounded++;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)|| Input.GetKeyDown(KeyCode.RightShift))
        {
            transform.Translate(movement * speed*10 * Time.deltaTime, Space.World);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeHealth(20f);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeHealth(-20f);
        }
        if (transform.position.y < 1)
        {
            ChangeHealth(-1f * Time.deltaTime);
        }
        CurHealth = Mathf.Lerp(CurHealth, targetHealth, Time.deltaTime * DaleySpeedHealth);
        healthBar.SetHealth(CurHealth);

 
    }

    public void ChangeHealth(float healthChange)
    {
        targetHealth = Mathf.Clamp(targetHealth + healthChange, 0, MaxHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
        grounded = 0;
    }

    private void Attack()
    {
        Debug.Log("Player Attacked!");

        if (AttackOfPlayer != null)
        {
            GameObject attackInstance = Instantiate(AttackOfPlayer, transform.position + transform.forward * 1.5f, Quaternion.identity);
            StartCoroutine(FadeOutAndDestroy(attackInstance, 2f)); 
        }

        Debug.DrawRay(transform.position, transform.forward * attackRange, Color.red, 0.5f);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log("Hit Enemy! Dealt " + attackDamage + " damage.");
            }
        }
    }

    private IEnumerator FadeOutAndDestroy(GameObject attackObject, float duration)
    {
        Renderer renderer = attackObject.GetComponent<Renderer>();
        if (renderer == null)
        {
            Destroy(attackObject, duration); // אם אין רנדרר, פשוט הורסים
            yield break;
        }

        Material material = renderer.material;
        Color startColor = material.color;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            material.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        Destroy(attackObject);
    }

}
