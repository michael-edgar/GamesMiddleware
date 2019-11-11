using System;
using System.Collections.Generic;

public static class StateDriver
{
    public enum States { BaseIdle, Walk, Run, Jump, MidAir, Fall, FightIdle, Punch, Kick, PositionIdle, Crouch}
    private static List<State> _states;

    public static void Initialise()
    {
        _states = new List<State>
        {
            new State("Base Layer", "Idle"),
            new State("Base Layer.Movement", "Walk"),
            new State("Base Layer.Movement", "Run"),
            new State("Base Layer.Airborne", "Jump"),
            new State("Base Layer.Airborne", "Mid Air"),
            new State("Base Layer.Airborne", "Fall"),
            new State("Fight Layer", "Idle"),
            new State("Punch Layer", "Punch"),
            new State("Kick Layer", "Kick"),
            new State("Position Layer", "Idle"),
            new State("Position Layer", "Crouch")
        };
    }
    public static string GetState(States state)
    {
        return _states[(int) state].StateAsString;
    }
}

public class State
{
    private string Layer;
    private string Name;

    public State(string layer, string name)
    {
        Layer = layer;
        Name = name;
    }

    public string StateAsString => Layer + "." + Name;
}