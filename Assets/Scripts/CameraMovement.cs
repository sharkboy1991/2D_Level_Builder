using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public TileManager _tileMng;

    public float xSpeed = 2;
    public float ySpeed = 2;

    float xMax = 0;
    float xMin = 0;

    float yMax = 0;
    float yMin = 0;

    private void Start()
    {
        //calculate x boundarys
        xMax = _tileMng.xTile * 1.28f / 2f;
        xMin = -(_tileMng.xTile * 1.28f / 2f);

        //calculate y boundarys
        yMax = _tileMng.yTile * 1.28f / 2f;
        yMin = -(_tileMng.yTile * 1.28f / 2f);
    }


    void Update()
    {
        //get axis inputs
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        //clamp x input
        if (transform.position.x < xMin)
            xInput = Mathf.Clamp(xInput, 0, 1);
        else if (transform.position.x > xMax)
            xInput = Mathf.Clamp(xInput, -1, 0);
        else
            xInput = Mathf.Clamp(xInput, -1, 1);

        //clamp y input
        if (transform.position.y < yMin)
            yInput = Mathf.Clamp(yInput, 0, 1);
        else if (transform.position.y > yMax)
            yInput = Mathf.Clamp(yInput, -1, 0);
        else
            yInput = Mathf.Clamp(yInput, -1, 1);


        float x = xInput * Time.deltaTime * xSpeed;
        float y = yInput * Time.deltaTime * ySpeed;

        //move camera
        transform.position += new Vector3(x, y, 0);
    }
}
