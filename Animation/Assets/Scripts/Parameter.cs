using System.Collections.Generic;
using UnityEngine;

public static class ParameterDriver
{
    public enum Parameters { Speed, VelocityX, VelocityZ, Jump, Fall, Crouch, Punch, Kick }
    private static List<Parameter> _parameters;

    public static void Initialise()
    {
        _parameters = new List<Parameter>
        {
            new Parameter("Speed"),
            new Parameter("VelocityX"),
            new Parameter("VelocityZ"),
            new Parameter("isJump"),
            new Parameter("isFall"),
            new Parameter("isCrouch"),
            new Parameter("isPunch"),
            new Parameter("isKick"),
        };
    }

    public static int GetParameterHash(Parameters parameter)
    {
        return _parameters[(int) parameter].ParameterHash;
    }
        
    public static string GetParameterString(Parameters parameter)
    {
        return _parameters[(int) parameter].ParameterString;
    }
}

public class Parameter
{
    public Parameter(string name)
    {
        ParameterHash = Animator.StringToHash(name);
        ParameterString = name;
    }

    public string ParameterString { get; }

    public int ParameterHash { get; }
}