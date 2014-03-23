using System;

public class Calculator
{
    bool _isDirty;

    string _operation;
    decimal _state;

    public decimal Display { get; private set; }

    public void Enter(decimal number)
    {
        _state = number;
        _isDirty = true;
    }

    public void PressPlus()
    {
        _operation = "+";
        if (_isDirty) Calculate();
    }

    public void PressEquals()
    {
        if (_isDirty) Calculate();
    }

    public void PressAC()
    {
        _operation = string.Empty;
        _state = 0m;
    }

    void Calculate()
    {
        switch (_operation)
        {
            case "+":
                Display += _state;
                break;
        }

        _isDirty = false;
    }

}