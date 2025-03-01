using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cube;
    private static int seed;
    [SerializeField] private int roomX, roomZ;
    public static int Seed { get => seed; set => seed = value; }

    int[,] room;// X Z Axes
    Queue<Vector3> roomsToCreate = new Queue<Vector3>();

    private void Start()
    {
        //UnityEngine.Random.InitState(Seed);
        StartCoroutine(CreateRoom());
    }

    private IEnumerator CreateRoom()
    {
        int roomsCreated = 0;
        roomsToCreate.Enqueue(Vector3.zero);
        while (roomsToCreate.Count > 0)
        {
            Vector3 actualVector = roomsToCreate.Dequeue();
            if (Physics.OverlapSphere(actualVector, 0.1f).Length == 0)
            {
                Instantiate(cube, actualVector, Quaternion.identity);
                roomsCreated++;
                Debug.Log("Rooms created: " + roomsCreated);
            }
            for (int i = 0; i < 4; i++)
            {
                Vector3 tempVector = Vector3.zero;
                if (Convert.ToBoolean(UnityEngine.Random.Range(0, 2)))
                {
                    switch (i)
                    {
                        case 0:
                            tempVector = new Vector3(0, 0, 1) + actualVector;
                            break;
                        case 1:
                            tempVector = new Vector3(1, 0, 0) + actualVector;
                            break;
                        case 2:
                            tempVector = new Vector3(0, 0, -1) + actualVector;
                            break;
                        case 3:
                            tempVector = new Vector3(-1, 0, 0) + actualVector;
                            break;
                    }
                    Debug.Log(actualVector + " wants a new room on: " + tempVector);
                }
                if (Physics.OverlapSphere(tempVector, 0.1f).Length == 0)
                {
                    Debug.Log("Enqueued: " + tempVector);
                    if (roomsCreated <= 10)
                    {
                        roomsToCreate.Enqueue(tempVector);
                    }
                }
            }
            Debug.Log("Rooms left in queue: " + roomsToCreate.Count);
            yield return new WaitForSecondsRealtime(0.5f);
        }
        Debug.Log("Ended creating the rooms");
    }

}
