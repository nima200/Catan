/*
 * ENUM for representing the turn phases of the game.
 * Phase1: first turn where player places settlement and an adjacent road at any position   ]
 *                                                                                          ] <-- SANDBOX MODES
 * Phase2: second turn where player places city and an adjacent road at any position        ]
 * Phase3: every other build phase in the game where the specific rules are applied [Post sandbox mode]
 */

public enum TurnPhase
{
    Phase1,
    Phase2,
    Phase3
}