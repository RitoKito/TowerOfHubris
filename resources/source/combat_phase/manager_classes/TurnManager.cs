using Godot;
using System;

public enum TurnState
{
    SceneInitialization,
    PlayerTurn,
    InProgress,
    SceneComplete,
}

public partial class TurnManager : Node3D
{
    public static TurnManager Instance { get; private set; }

    private Messenger _messenger;
    private InputHandler _inputHandler;

    private int _turnCount = 0;
    public int TurnCount {  get { return _turnCount; } private set { } }
    private TurnState _turnState = TurnState.SceneInitialization;
    public TurnState TurnState { get { return _turnState; } private set { } }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Instance == null)
            Instance = this;
        else
            Free();

        _messenger = Messenger.Instance;
        _inputHandler = InputHandler.Instance;

        _messenger.OnResolveTurn += HandleTurnInProgress;
        _messenger.OnTurnResolved += HandleTurnResolved;
        _messenger.OnResolveTurn += HandleTurnInProgress;

        _turnState = TurnState.SceneInitialization;
        BeginNewTurn();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void BeginNewTurn()
    {
        _turnCount++;
        _turnState = TurnState.PlayerTurn;
        _messenger.EmitTurnStateChanged(TurnState);
    }

    private void HandleTurnInProgress()
    {
        _turnState = TurnState.InProgress;
        _messenger.EmitTurnStateChanged(TurnState);
    }
    private void HandleTurnResolved()
    {
        BeginNewTurn();
    }

    private void TurnStateChanged()
    {
        _messenger.EmitTurnStateChanged(TurnState);
    }


}
