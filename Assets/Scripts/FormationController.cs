using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour
{
    public int playerNumber;
    public GameObject housePrefab;
    public float radius;
    public Vector3 point = Vector3.zero;
    public GameObject bakedData;
    private void Start()
    {
        if (GameManager.singleton != null)
            playerNumber = GameManager.singleton.GetAllRoleNumber();

        CreateHouseAndPlayerPositionAroundPoint();
        Destroy(bakedData);
    }

    public void CreateHouseAndPlayerPositionAroundPoint()
    {
        for (int i = 0; i < playerNumber; i++)
        {
            var radians = 2 * Mathf.PI / playerNumber * i;
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertical);
            var spawnPos = point + spawnDir * radius;
            var house = Instantiate(housePrefab, spawnPos, Quaternion.identity) as GameObject;

            house.transform.LookAt(point);
            house.transform.Translate(new Vector3(0, house.transform.localScale.y / 2, 0));

            if (GameManager.singleton != null)
                GameManager.singleton.HousePosition.Add(house);
        }
    }
}
