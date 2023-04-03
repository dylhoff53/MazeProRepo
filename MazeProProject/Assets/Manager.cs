using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject mazeMaker;
    public MazeMaker secondMaze;
    public MazeMaker firstMaze;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            MakeNewMaze();
        }
    }
    public void MakeNewMaze()
    {
        mazeMaker.transform.position = new Vector3(0, 0, firstMaze.mazeHeight);
        secondMaze.MakeMaze();
    }
}
