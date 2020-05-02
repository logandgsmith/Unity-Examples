using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Barebones Example Implementation of a Finite State Machine 
 *  Written by Logan D.G. Smith
 *  If you see any issues or would like to see more like this, 
 *  please let me know on GitHub!
 *  https://github.com/logandgsmith/Unity-Examples
 */

// The base class from which all States must inherit
public abstract class State
{
    // Each state should have a copy of the actor to control
    public Actor self;
    public State(Actor self) { this.self = self; }

    // Functions that are called from above
    public abstract void Enter();   // Actions to perform upon entering the state
    public abstract void Execute(); // Actions to perform on each update
    public abstract void Exit();    // Actions to perform when exiting the state
}

// An example implementation of what a state would look like
public class ExampleState : State
{
    // You can declare any variables specific to this state here
    private int spawners;
    private int[] actions;

    // Constructor that takes an additional param for this state
    public ExampleState(Actor self, int spawners) : base(self) 
    {
        this.spawners = spawners;
    }

    // Any actions such as intializations that must occur before the first update
    public override void Enter()
    {
        // Randomly generates a wave of enemies 
        actions = new int[spawners];
        for (int i = 0; i < spawners; i++)
            actions[i] = Random.Range(0, addlParam);
    }

    // Actions that should occur every update
    public override void Execute()
    {
        // Spawn a new entity from each spawner (You should not do this unless you have a cooldown)
        for (int i = 0; i < spawners; i++)
            self.ExampleAction(i, actions[i]);
    }

    // Actions such as clean ups that must occur before you move to another state
    public override void Exit()
    {
        Debug.Log("Exiting the example state");
    }
}

// The "Brain" of your actor, should handle the "thinking" while actor simply acts out the thoughts
public class FiniteStateMachine
{
    State currentState;
    List<State> states;

    public FiniteStateMachine()
    {
        // Add all states in the constructor as shown
        states.Add(new ExampleState());
    }

    // Transition from one state to the next
    public void TransitionState()
    {
        // Cannot exit a nonexistant state
        if (currentState != null)
            currentState.Exit();

        //TODO: Decide a transition table structure
    }

    // Execute the current state and handle any other FSM related matters
    public void UpdateState()
    {
        currentState.Execute();
    }
}

// The "Body" of your actor, should carry out the actions from FSM
public class Actor : MonoBehavior
{
    // Perhaps this actor spawns other entities defined elsewhere
    public List<GameObject> spawns;
    public List<GameObject> spawners; 

    // The brain of the actor
    private FiniteStateMachine fsm;

    // Any initializations for the actor
    private void Start()
    {
        fsm = new FiniteStateMachine();
    }

    // Handles updates and other actions the actor must perform each update
    private void Update()
    {
        fsm.UpdateState();
    }

    // You can add any actions that you want the actor to perform. This one spawns new entities
    public void ExampleAction(int spawner, int spawn)
    {
        Instantiate(spawns[spawn], spawners[spawner].transform.position, spawners[spawner].transform.rotation);
    }
}