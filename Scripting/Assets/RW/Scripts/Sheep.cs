using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public float dropDestroyDelay; // 1
    public float runSpeed;
    public float gotHayDestroyDelay;
    public float heartOffset; // 1
    public GameObject heartPrefab; // 2

    private Collider myCollider; // 2
    private Rigidbody myRigidbody;
    private bool hitByHay;
    private SheepSpawner sheepSpawner;


    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other) // 1
    {
        if (other.CompareTag("Hay") && !hitByHay) // 2
        {
            Destroy(other.gameObject); // 3
            HitByHay(); // 4
        }
        else if (other.CompareTag("DropSheep"))
        {
            Drop();
        }

    }

    private void HitByHay()
    {
        GameStateManager.Instance.SavedSheep();
        sheepSpawner.RemoveSheepFromList(gameObject);
        hitByHay = true; // 1
        runSpeed = 0; // 2
        SoundManager.Instance.PlaySheepHitClip();
        Destroy(gameObject, gotHayDestroyDelay); // 3
        TweenScale tweenScale = gameObject.AddComponent<TweenScale>(); ; // 1
        tweenScale.targetScale = 0; // 2
        tweenScale.timeToReachTarget = gotHayDestroyDelay; // 3

        Instantiate(heartPrefab, transform.position + new Vector3(0, heartOffset, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
    }

    private void Drop()
    {

        GameStateManager.Instance.DroppedSheep();
        SoundManager.Instance.PlaySheepDroppedClip();
        sheepSpawner.RemoveSheepFromList(gameObject);
        myRigidbody.isKinematic = false; // 1
        myCollider.isTrigger = false; // 2
        Destroy(gameObject, dropDestroyDelay); // 3
    }

    public void SetSpawner(SheepSpawner spawner)
    {
        sheepSpawner = spawner;
    }

}
