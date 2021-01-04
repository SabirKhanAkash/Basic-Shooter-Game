using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D myBody;
    public float moveForce = 0.5f;
    [SerializeField] private FireButtonScript fireButton;
    [SerializeField] public GameObject herobullet;
    [SerializeField] int health = 200;
    Vector2 bulletPos;
    [SerializeField] public float fireRate = 0.8f;
    float nextFire = 0.0f;

    private FixedJoystick joystick;
    // Start is called before the first frame update
    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        joystick = GameObject.FindWithTag("Joystick").GetComponent<FixedJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        
        myBody.velocity = new Vector2(joystick.Horizontal * 20 * moveForce, joystick.Vertical * 15 * moveForce);
        
        if (Input.GetKey(KeyCode.Space) || fireButton.isPressing)
        {
            if (nextFire < Time.time)
            {
                nextFire = Time.time + fireRate;
                bulletPos = transform.position;
                bulletPos += new Vector2(+1f, 0f);
                Instantiate(herobullet, bulletPos, Quaternion.identity);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer)
            return;
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.hit();
        if (health <= 0)
        {
            Destroy(gameObject);
            FindObjectOfType<LoadScene>().LoadGameOverScene();
        }
            
    }

}
