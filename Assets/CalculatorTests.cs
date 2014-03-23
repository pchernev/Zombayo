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

}