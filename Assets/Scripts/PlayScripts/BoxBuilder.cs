using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBuilder : MonoBehaviour {

    [Tooltip("This determines the size of your test area.")]
    public Vector2 sizeOfBox;

    [Tooltip("Offset the starting area of the Box, shifting it by this amount")]
    public Vector2 offset;

    public GameObject BoxPrefab;

    private void Start()
    {
        GameObject TestBox = new GameObject();

        for (int y = 0; y < sizeOfBox.y; y++)
        {
            for (int x = 0; x < sizeOfBox.x; x++)
            {
                if (x == 0 || y == 0 || x == sizeOfBox.x - 1 || y == sizeOfBox.y - 1)
                {
                    GameObject cell = Instantiate(BoxPrefab, new Vector3(x + offset.x, y + offset.y, 0), Quaternion.identity);
                    cell.name = "BOX X:" + (x + offset.x).ToString() + "Y:" + (y + offset.y).ToString();
                    cell.transform.SetParent(TestBox.transform);
                }
            }
        }

        TestBox.name = "Box Holder";
    }
}
