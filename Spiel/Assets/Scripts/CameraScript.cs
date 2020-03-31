using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private float cameraMovespeed = 1.6f;

    [SerializeField]
    private Vector3 offset;
    private float normalY;

    private GameEventSystem gameEventSystem;
    // Start is called before the first frame update
    void Start()
    {
        gameEventSystem = GameObject.FindGameObjectWithTag("GameEventSystem").GetComponent<GameEventSystem>(); 
        gameEventSystem.onGridStart += GameStart;
        gameEventSystem.onResetOrigin += SnapToPlayer;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 target = player.position + offset;
        Vector3 target = new Vector3(player.position.x, normalY, player.position.z) + offset;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * cameraMovespeed);
        
    }

    void GameStart()
    {
        transform.position = player.position + offset;
        normalY = player.position.y;
    }

    void SnapToPlayer()
    {
        transform.position = player.position + offset;
        transform.position = new Vector3(transform.position.x, normalY, transform.position.z);
    }
}
