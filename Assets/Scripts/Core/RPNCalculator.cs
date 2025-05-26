using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class RPNCalculator
{
    //int baseVal = -1;
    public static float Evaluate(string expression, Dictionary<string, float> variables)
    {
        var stack = new Stack<float>();
        var tokens = expression.Split(' ');
        
        foreach (var token in tokens)
        {
            if (float.TryParse(token, out float number))
            {
                stack.Push(number);
            }
            else if (variables.ContainsKey(token))
            {
                stack.Push(variables[token]);
            }
            else
            {
                float b = stack.Pop();
                float a = stack.Pop();

                switch (token)
                {
                    case "+": stack.Push(a + b); break;
                    case "-": stack.Push(a - b); break;
                    case "*": stack.Push(a * b); break;
                    case "/": stack.Push(a / b); break;
                    case "%": stack.Push(a % b); break;
                    default: throw new System.ArgumentException($"Unknown operator: {token}");
                }
            }
        }

        return stack.Pop();
    }
    public static int CalculateEnemyCount(string rpnExpression, int wave)
    {
        try
        {
            var variables = new Dictionary<string, float>
            {
                { "wave", wave }     
            };

            float result = Evaluate(rpnExpression, variables);
            return Mathf.Max(1, Mathf.FloorToInt(result)); 
        }
        catch (System.Exception e)
        {
            Debug.LogError($"HPcalculate fail: {e.Message} ( {rpnExpression})");
            return -1; // return base
        }
    }

    public static float CalculateBaseCount(string rpnExpression, int wave, float baseVal)
    {

        try
        {
            var variables = new Dictionary<string, float>
            {
                { "base", baseVal },  
                { "wave", wave }     
            };

            float result = Evaluate(rpnExpression, variables);
            //Debug.Log(result);
            return result;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"HP calculate fail: {e.Message} ( {rpnExpression}) ({baseVal})({wave})");
            return -1; // return base
        }
    }
    // New method: evaluate float results with power and wave variables
    public static float EvaluateFloat(string rpnExpression, int wave, float power)
    {
        //Debug.Log($"rpnExpression:{rpnExpression} wave: {wave} power: {power})");
        try
        {
            var variables = new Dictionary<string, float>
            {
                { "wave", wave },
                { "power", power }
            };

            return Evaluate(rpnExpression, variables);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Float RPN evaluate failed: {e.Message} ({rpnExpression})");
            return -1; // return base
        }
    }

    public static float EvaluateFloat(string rpnExpression, int wave)
    {
        try
        {
            //Debug.Log(rpnExpression +";");
            var variables = new Dictionary<string, float>
            {
                { "wave", wave }
            };
            return Evaluate(rpnExpression, variables);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Float RPN evaluate failed: {e.Message} ({rpnExpression})");
            return -1; // return base
        }
    }
}