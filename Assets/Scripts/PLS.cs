using System.Collections;
using System.Collections.Generic;

public interface PLS {

    bool Evaluate();
}

public abstract class BinaryOP: PLS
{
    protected PLS op1;
    protected PLS op2;

    public BinaryOP(PLS o1, PLS o2)
    {
        this.op1 = o1;
        this.op2 = o2;
    }

    public abstract bool Evaluate();
    public abstract string GetOperand();

    
    public override string ToString()
    {
        return "(" + this.op1.ToString() + this.GetOperand() + this.op2.ToString()+")";
    }
}

public abstract class UnaryOP: PLS
{
    protected PLS op1;

    public UnaryOP(PLS o1)
    {
        this.op1 = o1;
    }

    public abstract bool Evaluate();
}

public class Neg: UnaryOP
{
    public Neg(PLS o1) : base(o1) { }
    

    public override bool Evaluate()
    {
        return !this.op1.Evaluate();
    }

    public override string ToString()
    {
        return "¬("+this.op1.ToString()+")";
    }
}

public class And: BinaryOP
{
    public And(PLS o1, PLS o2) : base(o1, o2) { }
    public override string GetOperand()
    {
        return "∧";
    }


    public override bool Evaluate()
    {
        return this.op1.Evaluate() && this.op2.Evaluate();
    }

}

public class Or: BinaryOP
{
    public Or(PLS o1, PLS o2) : base(o1, o2) { }
    
    public override string GetOperand()
    {
        return "∨";
    }

    public override bool Evaluate()
    {
        return this.op1.Evaluate() || this.op2.Evaluate();
    }

}

public class To: BinaryOP
{
    public To(PLS o1, PLS o2) : base(o1, o2) { }
    public override string GetOperand()
    {
        return "→";
    }
    

    public override bool Evaluate()
    {
        return !this.op1.Evaluate() || this.op2.Evaluate();
    }
}

public class Equiv: BinaryOP
{
    public Equiv(PLS o1, PLS o2) : base(o1, o2) { }
    public override string GetOperand()
    {
        return "↔";
    }
    

    public override bool Evaluate()
    {
        return this.op1.Evaluate() == this.op2.Evaluate();
    }
}

public class UnaryPred : PLS
{
    private Constant c;
    private string name;

    public UnaryPred(Constant c, string name)
    {
        this.c = c;
        this.name = name;
    }

    public bool Evaluate()
    {
        return c.properties.Contains(name);
    }

    public override string ToString()
    {
        return name + "(" + c.name + ")";
    }
}

public class Constant
{
    public string name;
    public HashSet<string> properties;

    public Constant(string name)
    {
        this.name = name;
        this.properties = new HashSet<string>();
    }
}