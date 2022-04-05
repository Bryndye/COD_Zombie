using UnityEngine;

public enum InteractionType { Press, Hold}
public class ItemInteraction : MonoBehaviour
{
    public InteractionType MyInt;
    public string MessageForPlayer = "Press E to interact";
    public bool MessageCanAppear = true;
    public virtual void Interact(PlayerActions _playerActions)
    {
        //Debug.Log(gameObject.name);
        //something
    }
}
