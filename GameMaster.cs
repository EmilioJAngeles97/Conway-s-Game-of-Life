using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class GameMaster : MonoBehaviour {

    public GameOfLifeBoard myBoard;   // reference to board data structure
    private int boardSize = 30;       // Nothing magic about 50, could be 30, 40, etc.
    public Transform masterCell;      // Holds the prefab to create. In Inspector, drag and drop 
                                      // prefab onto Master Controller game object to set this 
                                      // Use this for initialization
    public static bool gamePlaying;

    public float timeForNextBoard;
    
    void Start()
    {
        Transform newCell;
        CellScript myCellScript;

        myBoard = new GameOfLifeBoard(boardSize);

        timeForNextBoard = 0.25F;
        gamePlaying = true;

        for (int i = 0; i < boardSize; i++) // starts in bottom left corner goes right to left
        {
            for (int j = 0; j < boardSize; j++) // starts in bottom left corner goes bottom to top
            {
                // Create a new instance of the prefab. This creates a Transform, not a GameObject
                newCell = Instantiate(masterCell, new Vector3(i * 1.5F, j * 1.5F, 0.0F), Quaternion.identity) as Transform;
                

                // You get to a GameObject by accessing the Transform's GameObject property.
                // Once we have a GameObject, can call GetComponent to retrieve the CellScript component on it.
                // CellScript is the name of the class defined on the prefab.
                // This is how we get access to the script defined on the prefab.
                myCellScript = newCell.gameObject.GetComponent<CellScript>();

                // Now that we have a reference to the script on the prefab (an instance of type of CellScript)
                // it is possible to call methods or directly set public variables. This is how we pass data 
                // from the master initialization script into each grid cell's GameObject.
                myCellScript.SetRowCol(i, j);
                myCellScript.SetGameOfLifeBoard(myBoard);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gamePlaying == true)
        {
            Debug.Log("Space Pressed so Game Paused");
            gamePlaying = false;
        }else if (Input.GetKeyDown(KeyCode.Space) && gamePlaying == false)
        {
            Debug.Log("Space Pressed so Game Resumed");
            gamePlaying = true;
        }

        if (Input.GetKeyDown(KeyCode.Comma) && Time.fixedDeltaTime <= 1.0F)
        {
            timeForNextBoard += 0.05F;
            Debug.Log("Fixed Time: " + timeForNextBoard);
        }

        if (Input.GetKeyDown(KeyCode.Period) && Time.fixedDeltaTime >= 0.05F)
        {
            timeForNextBoard += -0.05F;
            Debug.Log("Fixed Time: " + timeForNextBoard);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gamePlaying == true)
        {
            Time.fixedDeltaTime = timeForNextBoard;
            myBoard.WalkThrough();
            myBoard.SetNewBoard();
        }
    }
}
