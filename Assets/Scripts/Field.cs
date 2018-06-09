using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
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
        Reset();
	}

    public void Reset()
    {
        AddLetters(numLetters);
        CreateExpressions();
        DisplayPredicates();
    }


    private void AddLetters(int numLetters)
    {
        this.letters = new List<Vector2>();
        this.constants = new Dictionary<string, Constant>();
        List<Vector2> list = new List<Vector2>();
        for (int x = 0; x < boardSize; x++)
            for (int y = 0; y < boardSize; y++)
            {
                list.Add(new Vector2(x, y));
                fieldSlots[x, y].ClearText();
            }

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

    private void CreateExpressions()
    {
        this.predicates = new List<PLS>();
        PLS predicate = CreateExpression(3);
        this.predicates.Add(predicate);

        predicate = CreateExpression(2);
        this.predicates.Add(predicate);

        predicate = CreateExpression(2);
        this.predicates.Add(predicate);
    }

    private PLS CreateExpression(int size)
    {
        PLS predicate = CreateUnaryExpression();
        int sizeLeft = size;
        if (sizeLeft > 0) { 
            int j = Random.Range(0, 4);
            PLS newExp = CreateExpression(sizeLeft - 1);
            switch(j)
            {
                case 0:
                    predicate = new And(predicate, newExp);
                    break;
                case 1:
                    predicate = new Or(predicate, newExp);
                    break;
                case 2:
                    predicate = new To(predicate, newExp);
                    break;
                case 3:
                    predicate = new Equiv(predicate, newExp);
                    break;
            }
        }
        int i = Random.Range(0, 2);
        if (i > 0)
        {
            predicate = new Neg(predicate);
        }
        return predicate;
    }

    private PLS CreateUnaryExpression()
    {
        int i = Random.Range(0, this.constants.Count);
        Constant c = Utils.RandomValue(this.constants);
        string property = Utils.RandomProperty();
        return new UnaryPred(c, property);
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
        for (int i = 0; i < this.predicates.Count; i++)
        {
            PLS p = this.predicates[i];
            bool pTrue = p.Evaluate();
            Debug.Log(p.ToString() + "\t" + pTrue);
            allTrue = allTrue && pTrue;

            if (pTrue)
            {
                this.predicateTexts[i].color = new Color(0, 255, 0);
            }
            else
            {
                this.predicateTexts[i].color = new Color(255, 0, 0);
            }
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
