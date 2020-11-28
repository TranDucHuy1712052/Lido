using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Node that those 'horses' respawn when being kicked.
/// Also the place when horses first stand, at the begining of game.
/// </summary>
public class SpawnNode : MonoBehaviour {

    public Transform nextNode;
    public Token token;
    public bool interactable = false;

    public float smoothness = 10f;

    private void Update()
    {
        if (token == null)
            return;

        if(token.tokenStatus == TokenStatus.LOCKED)
        {
            token.tokenTransform.position = Vector3.Slerp(token.tokenTransform.position, GetPosition(), smoothness * Time.deltaTime);
        }
    }

    public Vector3 offset = new Vector3(0, 3, 0);

    public Vector3 GetPosition()
    {
        return transform.position + offset;
    }

    private void OnMouseDown()
    {
        Interact();
    }

    public void Interact()
    {
        if (interactable == true)
        {
            if (this.nextNode.GetComponent<Node>().IsOccupied())
                return;
            GameManager.instance.StartCoroutine(GameManager.instance.PlayToken(token));
            interactable = false;
        }
    }

}
