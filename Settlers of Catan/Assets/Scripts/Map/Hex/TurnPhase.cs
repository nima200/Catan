/*
 * ENUM for representing the turn phases of the game.
 * SANDBOX1: first turn where player places settlement and an adjacent road at any position   ]
 *                                                                                          ] <-- SANDBOX MODES
 * Sandbox2: second turn where player places city and an adjacent road at any position        ]
 * Build: every other build phase in the game where the specific rules are applied [Post sandbox mode]
 */

public enum TurnPhase
{
    Sandbox1,
    Sandbox2,
    Build
}