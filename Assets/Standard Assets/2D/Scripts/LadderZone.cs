using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySampleAssets._2D;

public class LadderZone : MonoBehaviour
{
    private PlatformerCharacter2D thePlayer;
    public GameObject GroundAbove;
    // Use this for initialization
    void Start()
    {
        thePlayer = FindObjectOfType<PlatformerCharacter2D>();
        thePlayer.climbing = false;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Debug.Log("found a ladder");
            thePlayer.climbing = true;
            GroundAbove.layer = 8;
        }

    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Debug.Log("Left a ladder");
            thePlayer.climbing = false;
            GroundAbove.layer = 0;
        }

    }
}