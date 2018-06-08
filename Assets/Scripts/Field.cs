using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour {

    public Sprite square;
    public Sprite triangle;
    public Sprite circle;
    public GameObject fieldSlot;
    public GameObject winner;

    public static Field instance;

    private FieldSlot[,] fieldSlots;
    private List<Vector2> letters;
    private Dictionary<string, Constant> constants;
    private List<PLS> predicates;

    public List<Text> predicateTexts;

    private int boardSize = 5;
    private int numLetters = 4;

	// Use this for initialization
	void Start () {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
            return;
        }

        CreateFieldSlots();
        AddLetters(numLetters);
        CreateExpression();
        DisplayPredicates();
	}


    private void AddLetters(int numLetters)
    {
        this.letters = new List<Vector2>();
        this.constants = new Dictionary<string, Constant>();
        List<Vector2> list = new List<Vector2>();
        for (int x = 0; x < boardSize; x++)
            for (int y = 0; y < boardSize; y++)
                list.Add(new Vector2(x, y));

        for (char a = 'a'; a < 'a' + numLetters; a++)
        {
            int index = Random.Range(0, list.Count);
            Vector2 chosen = list[index];
            list.RemoveAt(index);
            fieldSlots[(int)chosen.x,(int)chosen.y].SetText(a);
            letters.Add(chosen);
            constants.Add(""+a,new Constant("" + a));
        }
    }

    private void CreateFieldSlots()
    {
        fieldSlots = new FieldSlot[boardSize,boardSize];
       for (int x = 0; x < boardSize; x++)
        {
            for (int y  = 0; y < boardSize; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);
                fieldSlots[x, y] = Instantiate(fieldSlot, pos, Quaternion.identity, this.transform).GetComponent<FieldSlot>();
            }
        }
    }

    private void CreateExpression()
    {
        this.predicates = new List<PLS>();
        PLS predicate = new Neg(new UnaryPred(constants["a"], "Red"));
        this.predicates.Add(predicate);

        predicate = new And(new UnaryPred(constants["b"], "Square"), new UnaryPred(constants["c"], "Green"));
        this.predicates.Add(predicate);

        predicate = new To(new UnaryPred(constants["d"], "Red"), new UnaryPred(constants["a"], "Square"));
        this.predicates.Add(predicate);
    }

    private void DisplayPredicates()
    {
        for (int i = 0; i < this.predicates.Count; i++)
        {
            this.predicateTexts[i].text = this.predicates[i].ToString();
        }
    }

    public void Evaluate()
    {
        UpdateConstantProperties();
        bool allTrue = true;
        foreach(PLS p in this.predicates)
        {
            Debug.Log(p.ToString() + "\t" + p.Evaluate());
            allTrue = allTrue && p.Evaluate();
        }

        Debug.Log(allTrue);
        this.winner.SetActive(allTrue);
    }

    private void UpdateConstantProperties()
    {
        foreach (Vector2 letterPos in this.letters)
        {
            FieldSlot fSlot = fieldSlots[(int)letterPos.x, (int)letterPos.y];
            string name = fSlot.GetText();
            string col = fSlot.GetColor();
            string shape = fSlot.GetShape();

            Constant c = this.constants[name];
            c.properties.Clear();
            if (shape.Length > 0)
            {
                c.properties.Add(shape);
                if (col.Length > 0)
                {
                    c.properties.Add(col);
                }
            }
        }
    }
}
