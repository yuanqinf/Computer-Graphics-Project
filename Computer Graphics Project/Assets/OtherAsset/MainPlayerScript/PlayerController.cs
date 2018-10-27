using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour {
    
    
    
    public GameObject bulletPrefab;
    public GameObject cube;
    public GameObject arm;
    public GameObject thisCamera;
    public Transform bulletSpawn;
    public Transform itemSpawn;
    private Rigidbody rb;
    public float jump_force;
    
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationY = 0F;
    
    private GameObject obj;
    private Vector3 arm_rotation;
    private bool restore = false;
    private int count = 0;
    private float time = 15.0f;
    private GameObject real_obj;
    bool hold = false;
    bool trigger = false;
    bool drop = false;
    bool tp = false;
    
    // Use this for initialization
    void Start () {
        obj = cube;
        rb = GetComponent<Rigidbody> ();
        
        // Determine if is local player
        
        if (GetComponent<Rigidbody>())
        GetComponent<Rigidbody>().freezeRotation = true;
        
    }
    
    // Update is called once per frame
    void Update () {
        
        
        // Detect if player hold an item
        if (real_obj && hold) {
            //real_obj.transform.localScale= new Vector3(0.5f,0.5f,0.5f);
            real_obj.transform.position = itemSpawn.transform.position;
            real_obj.transform.rotation = itemSpawn.transform.rotation;
        }
        // Detect if player drop an item
        else if (real_obj && !hold && drop) {
            real_obj.transform.eulerAngles = new Vector3 (0, itemSpawn.transform.eulerAngles.y, 0);
            real_obj.transform.position = new Vector3 (itemSpawn.transform.position.x,
            0, itemSpawn.transform.position.z);
            drop = false;
        }
        
        // player click primary button on mouse to act
        if (Input.GetMouseButtonDown (0)) {
            if (!restore) {
                arm.transform.Rotate (20, 0, 0);
                restore = true;
            }
            // If player click primary button on mouse when he or she is holding something, the flag will change to drop
            if (hold) {
                hold = false;
                trigger = false;
                drop = true;
                // If player does not use teleport, then remove his or her authority directly
                if (!tp) {
                    Debug.Log ("Remove");
                    
                }
                // Else, his or her authority will remove after they teleport the item and exit teleport's trigger
                else {
                    Debug.Log ("Not Remove");
                }
            }
            // If player active a holdable item's tigger, he or she can hold this item on hands
            if (trigger) {
                hold = true;
                // and give this player authority directly to change this object's position
                
            }
        }
        
        // player jump action
        if(Input.GetKeyDown(KeyCode.Space)){
            // Check if player is on the ground
            if (Mathf.Abs (rb.velocity.y) < 0.00001) {
                rb.AddForce (new Vector3 (0.0f, 1.0f, 0.0f) * jump_force);
            }
        }
        
        
        
        obj = bulletPrefab;
        time = 2.0f;
        
        
        // arm rotation restore
        count += 1;
        if (restore) {
            if (count % 30 == 0) {
                arm.transform.Rotate (-20, 0, 0);
                restore = false;
            }
        }
        
        // movement action
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 5.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 5.0f;
        
        transform.Translate(x, 0, 0);
        transform.Translate(0, 0, z);
        
        // shoot and create
        if (Input.GetKeyDown("c"))
        {
            CmdFire();
        }
        
        // mouse look action
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
            
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
            
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
            
            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }
    
    
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate (
        obj,
        bulletSpawn.position,
        bulletSpawn.rotation);
        
        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 25;
        
        // Spawn the bullet on the Clients
        
        
        // Destroy the bullet after 2 seconds
        Destroy(bullet, time);
    }
    
    void OnTriggerEnter(Collider other)
    {
        // If player collide with objects with "Item" tag, they can hold them
        if (other.gameObject.CompareTag ("Item")) {
            if (!hold) {
                real_obj = other.gameObject;  // get the reference of this object
                trigger = true;               // set "trigger" flag to true
            }
        }
        // If player collide the teleport, change the "tp" flag
        if (other.gameObject.CompareTag ("Teleport")) {
            tp = true;                        // set "tp" flag to true
        }
    }
    
    void OnTriggerExit(Collider other){
        // If player exit the teleport, change the "tp" flag back to default
        if (other.gameObject.CompareTag ("Teleport")) {
            tp = false;                       // set "tp" flag to false
        }
    }
    
    
    
    
}
