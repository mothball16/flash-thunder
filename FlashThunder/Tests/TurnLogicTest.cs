using System;
using FlashThunder.GameLogic.Components.Turn;

namespace FlashThunder.Tests;

/// <summary>
/// Simple test to verify turn-based logic works correctly
/// </summary>
public static class TurnLogicTest
{
    public static void RunTests()
    {
        Console.WriteLine("Running Turn Logic Tests...");
        
        TestTurnRangeComponent();
        TestActionTracking();
        
        Console.WriteLine("All turn logic tests passed!");
    }
    
    private static void TestTurnRangeComponent()
    {
        Console.WriteLine("Testing TurnRange component...");
        
        var turnRange = new TurnRange(maxMoves: 2, maxActions: 1);
        
        // Test initial state
        if (!turnRange.CanMove) throw new Exception("Should be able to move initially");
        if (!turnRange.CanAttack) throw new Exception("Should be able to attack initially");
        if (!turnRange.CanAct) throw new Exception("Should be able to act initially");
        
        // Test using a move
        turnRange.UseMove();
        if (turnRange.MovesRemaining != 1) throw new Exception("Should have 1 move remaining");
        if (!turnRange.HasMoved) throw new Exception("Should be marked as having moved");
        
        // Test using an action
        turnRange.UseAction();
        if (turnRange.ActionsRemaining != 0) throw new Exception("Should have 0 actions remaining");
        if (!turnRange.HasAttacked) throw new Exception("Should be marked as having attacked");
        if (turnRange.CanAttack) throw new Exception("Should not be able to attack anymore");
        
        // Test reset for new turn
        turnRange.ResetForNewTurn();
        if (!turnRange.CanMove) throw new Exception("Should be able to move after reset");
        if (!turnRange.CanAttack) throw new Exception("Should be able to attack after reset");
        if (turnRange.HasMoved) throw new Exception("Should not be marked as moved after reset");
        if (turnRange.HasAttacked) throw new Exception("Should not be marked as attacked after reset");
        
        Console.WriteLine("  ✓ TurnRange component tests passed");
    }
    
    private static void TestActionTracking()
    {
        Console.WriteLine("Testing action tracking logic...");
        
        var turnRange = new TurnRange(maxMoves: 1, maxActions: 1);
        
        // Test that unit can't move twice
        turnRange.UseMove();
        if (turnRange.CanMove) throw new Exception("Should not be able to move twice");
        
        // Test that unit can still attack after moving
        if (!turnRange.CanAttack) throw new Exception("Should still be able to attack after moving");
        
        turnRange.UseAction();
        if (turnRange.CanAttack) throw new Exception("Should not be able to attack twice");
        if (turnRange.CanAct) throw new Exception("Should not be able to act after all actions used");
        
        Console.WriteLine("  ✓ Action tracking tests passed");
    }
}