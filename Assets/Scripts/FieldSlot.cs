using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSlot : MonoBehaviour {

    private SpriteRenderer fieldObjectRenderer;
    private TextMesh textMesh;

    private int color = 0;
    private int shape = 0;

	void Awake() {
        GameObject fieldObject = this.gameObject.transform.Find("FieldObject").gameObject;
        this.fieldObjectRenderer = fieldObject.GetComponent<SpriteRenderer>();
        GameObject nameObject = fieldObject.transform.Find("Name").gameObject;
        nameObject.GetComponent<MeshRenderer>().sortingLayerName = "Texts";
        print(nameObject.GetComponent<TextMesh>());
        this.textMesh = nameObject.GetComponent<TextMesh>();
	}

    public void Reset()
    {
        this.color = 0;
        this.UpdateColor();
        this.shape = -1;
        this.UpdateShape();
    }
	
    private void UpdateColor()
    {
        color++;
        color %= 3;
        Color newColor = new Color(0,0,0);
        switch(color)
        {
            case 0:
                newColor = new Color(255, 0, 0);
                break;
            case 1:
                newColor = new Color(0, 255, 0);
                break;
            case 2:
                newColor = new Color(0, 255, 255);
                break;
        }
        this.fieldObjectRenderer.color = newColor;
    }
    
    private void UpdateShape()
    {
        shape++;
        shape %= 4;
        Sprite newShape = null;
        switch(shape)
        {
            case 1:
                newShape = Field.instance.circle;
                break;
            case 2:
                newShape = Field.instance.square;
                break;
            case 3:
                newShape = Field.instance.triangle;
                break;
        }

        this.fieldObjectRenderer.sprite = newShape;
    }

    internal void SetText(char a)
    {
        this.textMesh.text = "" + a;
    }

    internal void ClearText()
    {
        this.textMesh.text = "";
    }

    public string GetText()
    {
        return this.textMesh.text;
    }

    public string GetShape()
    {
        switch(shape)
        {
            case 1:
                return "Circle";
            case 2:
                return "Square";
            case 3:
                return "Triangle";
            default:
                return "";
        }
    }

    public string GetColor()
    {
        switch(color)
        {
            case 0:
                return "Red";
            case 1:
                return "Green";
            case 2:
                return "Blue";
            default:
                return "";
        }
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0))
        {
            UpdateShape();
        }
        if(Input.GetMouseButtonDown(1)){
            UpdateColor();
        }
    }

}
