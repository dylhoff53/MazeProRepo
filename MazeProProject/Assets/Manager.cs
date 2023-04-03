using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject mazeMaker;
    public MazeMaker secondMaze;
    public MazeMaker firstMaze;
    public GameObject thirdMazeMaker;
    public MazeMaker thirdMaze;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            MakeNewMaze();
        }else if (Input.GetKeyDown(KeyCode.R))
        {
            MakeThirdMaze();
        } else if (Input.GetKeyDown(KeyCode.T))
        {
            MakeSecondMaze();
        }
    }
    public void MakeNewMaze()
    {
        mazeMaker.transform.position = new Vector3(0, 0, firstMaze.mazeHeight);
        secondMaze.MakeMaze();
    }

    public void MakeThirdMaze()
    {
        thirdMazeMaker.transform.position = new Vector3(0, 0, secondMaze.mazeHeight);
        thirdMaze.MakeMaze();
    }
    public void MakeSecondMaze()
    {
        mazeMaker.transform.position = new Vector3(0, 0, thirdMaze.mazeHeight);
        secondMaze.otherMazeMaker = thirdMaze;
        secondMaze.MakeMaze();
    }
}
