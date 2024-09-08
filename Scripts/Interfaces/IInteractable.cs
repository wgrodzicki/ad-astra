using UnityEngine;

/// <summary>
/// Interface for all objects that can be interacted with by the player.
/// </summary>
public interface IInteractable
{
    bool IsUsable { get; set; }
    bool IsBeingUsed { get; set; }
    void Use();
    GameObject GenerateSound(GameObject sound);
}
