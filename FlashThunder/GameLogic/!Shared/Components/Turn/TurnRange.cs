namespace FlashThunder.GameLogic.Components.Turn
{
    /// <summary>
    /// Component tracking available actions for a unit during the current turn
    /// </summary>
    internal struct TurnRange
    {
        public int MaxMoves { get; set; }
        public int MovesRemaining { get; set; }
        public int MaxActions { get; set; }
        public int ActionsRemaining { get; set; }
        public bool HasMoved { get; set; }
        public bool HasAttacked { get; set; }
        
        public readonly bool CanMove => MovesRemaining > 0 && !HasMoved;
        public readonly bool CanAttack => ActionsRemaining > 0 && !HasAttacked;
        public readonly bool CanAct => CanMove || CanAttack;
        
        public TurnRange(int maxMoves = 1, int maxActions = 1)
        {
            MaxMoves = maxMoves;
            MovesRemaining = maxMoves;
            MaxActions = maxActions;
            ActionsRemaining = maxActions;
            HasMoved = false;
            HasAttacked = false;
        }
        
        public void ResetForNewTurn()
        {
            MovesRemaining = MaxMoves;
            ActionsRemaining = MaxActions;
            HasMoved = false;
            HasAttacked = false;
        }
        
        public void UseMove()
        {
            if (MovesRemaining > 0)
            {
                MovesRemaining--;
                HasMoved = true;
            }
        }
        
        public void UseAction()
        {
            if (ActionsRemaining > 0)
            {
                ActionsRemaining--;
                HasAttacked = true;
            }
        }
    }
}
