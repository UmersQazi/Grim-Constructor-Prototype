using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public float goodDeeds = 100f;
    public TextMeshProUGUI goodDeedsText;
    Soldier soldierToPlace;
    public CustomCursor customCursor;
    bool unDo;
    public GameObject grid;
    public Tile[] roadTiles;
    public Tile[] soldierTiles;
    Road roadToPlace;

    public GameObject player;
    public float playerSpeed = 5f;
    public float obstacleGoal = 3f;
    public float obstacleCount = 0f;
    public bool reached;

    public GameObject winText;

    public AudioSource select;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        reached = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        goodDeedsText.SetText("Good Deeds: " + goodDeeds);
        
        if(obstacleCount >= obstacleGoal)
        {
            reached = true;
            player.transform.position += Vector3.up * playerSpeed * Time.deltaTime;
            if(player.GetComponent<PlayerController>().done)
            winText.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            unDo = true;
            customCursor.gameObject.SetActive(false);
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0) && soldierToPlace != null && customCursor.gameObject.GetComponent<SpriteRenderer>().sprite == soldierToPlace.GetComponent<SpriteRenderer>().sprite)
        {
            select.Play();
            Tile nearestTile = null;
            float nearestDistance = float.MaxValue;
            foreach (Tile tile in soldierTiles)
            {
                float dist = Vector2.Distance(tile.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (dist < nearestDistance)
                {
                    nearestDistance = dist;
                    nearestTile = tile;
                }
            }
            if (!nearestTile.isOccupied && !unDo)
            {
                Instantiate(soldierToPlace, nearestTile.transform.position, Quaternion.identity);
                obstacleCount++;
                soldierToPlace = null;
                nearestTile.isOccupied = true;
                grid.SetActive(false);
                customCursor.gameObject.SetActive(false);
                Cursor.visible = true;

            }
        }
        
        else if (Input.GetMouseButtonDown(0) && roadToPlace != null && customCursor.gameObject.GetComponent<SpriteRenderer>().sprite == roadToPlace.GetComponent<SpriteRenderer>().sprite) {
            select.Play();
            Tile nearestTile = null;
            float nearestDistance = float.MaxValue;
            foreach (Tile tile in roadTiles)
            {
                float dist = Vector2.Distance(tile.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (dist < nearestDistance)
                {
                    nearestDistance = dist;
                    nearestTile = tile;
                }
            }
            if (!nearestTile.isOccupied && !unDo)
            {
                Instantiate(roadToPlace, nearestTile.transform.position, Quaternion.identity);
                obstacleCount++;
                roadToPlace = null;
                nearestTile.isOccupied = true;
                grid.SetActive(false);
                customCursor.gameObject.SetActive(false);
                Cursor.visible = true;

            }
        }

    }

    public void BuySoldier(Soldier soldier) {
        if(goodDeeds >= soldier.cost)
        { 
            customCursor.gameObject.SetActive(true);
            customCursor.GetComponent<SpriteRenderer>().sprite = soldier.GetComponent<SpriteRenderer>().sprite;
            Cursor.visible = false;
            goodDeeds -= soldier.cost;
            soldierToPlace = soldier;
            grid.SetActive(true);
        }
        
    }

    public void BuyRoad(Road road) {
        if (goodDeeds >= road.cost)
        {
            customCursor.gameObject.SetActive(true);
            customCursor.GetComponent<SpriteRenderer>().sprite = road.GetComponent<SpriteRenderer>().sprite;
            Cursor.visible = false;
            goodDeeds -= road.cost;
            roadToPlace = road;
            grid.SetActive(true);
        }
    }

    public void restart()
    {
        select.Play();
        SceneManager.LoadScene("Level 1");
    }

    public void help() {
        select.Play();
        SceneManager.LoadScene("Help");
    }

    public void title()
    {
        select.Play();
        SceneManager.LoadScene("Main Menu");
    }

}
