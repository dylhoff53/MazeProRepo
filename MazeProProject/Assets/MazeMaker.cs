﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMaker : MonoBehaviour
{
    public int mazeWidth;
    public int mazeHeight;
    public Location mazeStart = new Location(0,0);
    public Location LastMazeEnd = new Location(0, 0);
    public Location exit = new Location(0, 0);
    bool hasExit = true;
    public bool first;
    public MazeMaker otherMazeMaker;
    //GridLevel levelOne;
    GridLevelWithRooms levelOne;
    GameObject wallPrefab;
    GameObject wall1Prefab;
    GameObject wall2Prefab;
    GameObject blockerPrefab;
    public Manager mang;
    public int MazeCounter;

    // Start is called before the first frame update
    void Start()
    {
        wallPrefab = Resources.Load<GameObject>("Wall");
        wall1Prefab = Resources.Load<GameObject>("Wall 1");
        wall2Prefab = Resources.Load<GameObject>("Wall 2");
        blockerPrefab = Resources.Load<GameObject>("Blocker");
        if (first == true)
        {
            MakeMaze();
        }


        //levelOne = new GridLevel(mazeWidth, mazeHeight);
        //generateMaze(levelOne, mazeStart);

        // levelOne.cells is now a two-dimensional array representing each cell in the grid
        // each cell know if it's in the maze and has an array of booleans indicating whether or not
        // it's connected to each of its four neighbors: +x, +y, -y, -x
        // e.g. assuming a traditional 2D coordinate space, true true false false means 
        // this cell connects to its neighbors to the right and down,
        // but not to the cells to the left and up

        //for (int i = 0; i < mazeWidth; i++)
        //{
        //    for (int j = 0; j < mazeHeight; j++)
        //    {
        //        Debug.Log("Cell " + i + "," + j + ": " + levelOne.cells[i, j]);
        //    }
        //}

        // add an exit (per Millington bottom of page 705)
        //int exitX = mazeWidth - 1 - mazeStart.x;
        //int exitY = mazeHeight - 1 - mazeStart.y;
        //Location exit = new Location(exitX, exitY);
        //MakeDoorway(exit);

        //MakeDoorway(mazeStart);
        //BuildMaze();
    }

    public void MakeMaze()
    {
        // destroy old walls
        if(first == true)
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            foreach (GameObject wall in walls)
            {
                Destroy(wall);
            }
        } else if (MazeCounter == 2)
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall1");
            foreach (GameObject wall in walls)
            {
                Destroy(wall);
            }
        } else if(MazeCounter == 3)
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall2");
            foreach (GameObject wall in walls)
            {
                Destroy(wall);
            }
        }

        // generate a new maze
        if (first == true)
        {
            mazeWidth = (int)Random.Range(10f, 20f);
            mazeHeight = (int)Random.Range(10f, 20f);
        } else
        {
            mazeWidth = otherMazeMaker.mazeWidth;
            mazeHeight = (int)Random.Range(10f, 20f) + otherMazeMaker.mazeHeight;
        }
        if(first == true)
        {
            mazeStart = new Location((int)Random.Range(1f, mazeWidth - 1), 0);
        } else
        {
            LastMazeEnd = otherMazeMaker.exit;
            mazeStart = LastMazeEnd;
        }
        //mazeEnd = new Location((int)Random.Range(1f, mazeWidth - 1), mazeHeight);
        //levelOne = new GridLevel(mazeWidth, mazeHeight);
        levelOne = new GridLevelWithRooms(mazeWidth, mazeHeight);
        generateMaze(levelOne, mazeStart);
        MakeDoorway(mazeStart);
        // MakeDoorway(mazeEnd);
        if (hasExit)
        {
            if(first == true)
            {
                exit.x = mazeWidth - 1 - mazeStart.x;
                exit.y = mazeHeight - 1 - mazeStart.y;
            } else
            {
                exit.x = (int)Random.Range(1f, mazeWidth - 1);
                exit.y = mazeHeight - 1;
            }
            MakeDoorway(exit);
        }
        if(first == true)
        {
            BuildMaze(0,0, wallPrefab);
 //           mang.MakeNewMaze();
        }
        else if(MazeCounter == 2)
        {
            BuildMaze(0, otherMazeMaker.mazeHeight, wall1Prefab);
        } else if(MazeCounter == 3)
        {
            BuildMaze(0, otherMazeMaker.mazeHeight, wall2Prefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && first == true)
        {
            MakeMaze();
        }

        // debug draw the maze
        if (levelOne != null)
        {
            for (int x = 0; x < mazeWidth; x++)
            {
                for (int y = 0; y < mazeHeight; y++)
                {
                    Connections currentCell = levelOne.cells[x, y];
                    if (currentCell.inMaze)
                    {
                        Vector3 cellPos = new Vector3(x, 0, y);
                        float lineLength = 1f;
                        if (currentCell.directions[0])
                        {
                            // positive x
                            Vector3 neighborPos = new Vector3(x + lineLength, 0, y);
                            Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                        }
                        if (currentCell.directions[1])
                        {
                            // positive y
                            Vector3 neighborPos = new Vector3(x, 0, y + lineLength);
                            Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                        }
                        if (currentCell.directions[2])
                        {
                            // negative y
                            Vector3 neighborPos = new Vector3(x, 0, y - lineLength);
                            Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                        }
                        if (currentCell.directions[3])
                        {
                            // negative x
                            Vector3 neighborPos = new Vector3(x - lineLength, 0, y);
                            Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                        }
                    }
                }
            }
        }
    }

    void BuildMaze(int width, int height, GameObject wallPrefab)
    {
        for (int x = width; x < mazeWidth; x++)
        {
            for (int y = height; y < mazeHeight; y++)
            {
                Connections currentCell = levelOne.cells[x, y];
                if (levelOne.cells[x, y].inMaze)
                {
                    Vector3 cellPos = new Vector3(x, 0, y);
                    float lineLength = 1f;
                    if (!currentCell.directions[0])
                    {
                        Vector3 wallPos = new Vector3(x + lineLength / 2, 0, y);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.identity) as GameObject;
                    }
                    if (!currentCell.directions[1])
                    {
                        Vector3 wallPos = new Vector3(x, 0, y + lineLength / 2);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.Euler(0f, 90f, 0f)) as GameObject;
                    }
                    if (y == 0 && !currentCell.directions[2])
                    {
                        // negative y
                        Vector3 wallPos = new Vector3(x, 0, y - lineLength / 2);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.Euler(0f, 90f, 0f)) as GameObject;
                    }
                    if (x == 0 && !currentCell.directions[3])
                    {
                        // negative x
                        Vector3 wallPos = new Vector3(x - lineLength / 2, 0, y);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.identity) as GameObject;
                    }
                } else
                {
                        MakeMaze();
                }
                if (!currentCell.directions[0] && !currentCell.directions[1] && !currentCell.directions[2] && !currentCell.directions[3])
                {
                    GameObject blocker = Instantiate(blockerPrefab, new Vector3(x, 0, y), Quaternion.identity) as GameObject;
                }
            }
        }
    }

    void MakeDoorway(Location location)
    {
        Connections cell = levelOne.cells[location.x, location.y];
        // which connection to set to true?
        // directions are listed in this order: +x, +y, -y, -x
        if (location.x == 0)
        {
            cell.directions[3] = true;
        }
        else if (location.x == mazeWidth - 1)
        {
            cell.directions[0] = true;
        }
        else if (location.y == 0)
        {
            cell.directions[2] = true;
        }
        else if (location.y == mazeHeight - 1)
        {
            cell.directions[1] = true;
        }
    }

    // From Millington pg. 706. He calls this function just "maze"
    void generateMaze(Level level, Location start)
    {
        // a stack of locations we can branch from
        Stack<Location> locations = new Stack<Location>();
        locations.Push(start);
        level.startAt(start);

        while (locations.Count > 0)
        {
            Location current = locations.Peek();

            // try to connect to a neighboring location
            Location next = level.makeConnection(current);
            if (next != null)
            {
                // if successful, it will be our next iteration
                locations.Push(next);
            }
            else
            {
                locations.Pop();
            }
        }
    }
}
