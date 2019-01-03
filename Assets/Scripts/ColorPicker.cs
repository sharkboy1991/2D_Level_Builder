using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public int paletteID = 0;

    Image[] colImages;

    int colorPicked = 0;

    public Color[] _colors;

    public TileManager tileMng;

    public Color GetColor()
    {
        colImages = new Image[transform.childCount];

        //get image components from the childs 
        for (int i = 0; i < colImages.Length; i++)
        {
            colImages[i] = transform.GetChild(i).GetComponent<Image>();
            colImages[i].color = _colors[i];
        }

        return _colors[colorPicked];
    }

    public void ChangeColor(int col)
    {
        colorPicked = col;

        if (paletteID == 0)
        {
            tileMng.SetColorA(_colors[colorPicked]);
        }
        else if (paletteID == 1)
        {
            tileMng.SetColorB(_colors[colorPicked]);
        }

        gameObject.SetActive(false);
    }
}
