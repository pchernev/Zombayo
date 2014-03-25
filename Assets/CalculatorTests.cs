using System;
using SharpUnit;

public class CalculatorTests : TestCase
{
    private Calculator calculator = null;


    public override void SetUp()
    {
        calculator = new Calculator();
    }

    public override void TearDown()
    {
        calculator = null;
    }

    [UnitTest]
    public void BasicSumTest() 
    {
        this.calculator.PressAC();
        this.calculator.Enter(3m);
        this.calculator.PressPlus();
        this.calculator.Enter(2m);
        this.calculator.PressEquals();

        Assert.Equal(5m, this.calculator.Display, "Has to be 5");
    }

    [UnitTest]
    public void SumOfNegativeNumbers()
    {
        this.calculator.PressAC();
        this.calculator.Enter(-3m);
        this.calculator.PressPlus();
        this.calculator.Enter(-2m);
        this.calculator.PressEquals();

        Assert.Equal(-5m, this.calculator.Display, "Has to be -5");
    }

    [UnitTest]
    public void SumOfZeros()
    {
        this.calculator.PressAC();
        this.calculator.Enter(0m);
        this.calculator.PressPlus();
        this.calculator.Enter(0m);
        this.calculator.PressEquals();

        Assert.Equal(0m, this.calculator.Display, "Has to be 0");
    }

    [UnitTest]
    public void MoreThanOneSum()
    {
        this.calculator.PressAC();
        this.calculator.Enter(2m);
        this.calculator.PressPlus();
        this.calculator.Enter(0m);
        this.calculator.PressPlus();
        this.calculator.Enter(3m);
        this.calculator.PressPlus();
        this.calculator.Enter(11m);
        this.calculator.PressEquals();

        Assert.Equal(16m, this.calculator.Display, "Has to be 16");
    }

    [UnitTest]    
    public void ExpectException()
    {
        try
        {
            this.calculator.PressAC();
            string invalidNumber = "asdasd";
            this.calculator.Enter(Decimal.Parse(invalidNumber));
            this.calculator.PressPlus();
            this.calculator.Enter(Decimal.Parse(invalidNumber));
            this.calculator.PressEquals();
            Assert.ExpectException(new FormatException());
        }
        catch(FormatException ex)
        {
            
        }        
    }
    
    [UnitTest] 
    public void SumOfSmallNumbers() 
    {
        decimal dec = 0.0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001m;
        decimal dec1 = 0.0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001m;
        this.calculator.PressAC();
        this.calculator.Enter(dec);
        this.calculator.PressPlus();
        this.calculator.Enter(dec1);
        this.calculator.PressEquals();
        decimal expectedResult = 0.0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002m;
        Assert.Equal(expectedResult, this.calculator.Display, "Has to be 0.00....2");
    }
   
}