using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Token is a class that represent the 'horses'.
/// </summary>
public class Token {

    #region var
    public Transform tokenTransform;                    //
    public PlayerType tokenType;                        //player that hold this token
    public Transform originalSpawnNode;                 //the node that this token stand at the begining
    public SpawnNode originalSpawnNodeComponent;
    public Transform parentNode;                        //the node (or 'tile') that this 'horse' is standing.
    public Vector3 originalScale;
    public TokenComponent tokenComponent;

    public TokenStatus tokenStatus;
    #endregion

    #region func
    public void SetParentNode(Transform _parentNode)
    {
        parentNode = _parentNode;
    }

    public Node GetParentNodeComponent()
    {
        if (tokenStatus == TokenStatus.LOCKED)
            return null;
        return parentNode.GetComponent<Node>();
    }

    public Token(PlayerType playerType, Transform spawnNode, Transform _tokenTransform)
    {
        tokenType = playerType;
        originalSpawnNode = spawnNode;
        originalSpawnNodeComponent = originalSpawnNode.GetComponent<SpawnNode>();
        originalSpawnNodeComponent.token = this;

        tokenTransform = _tokenTransform;
        tokenTransform.SetPositionAndRotation(originalSpawnNodeComponent.GetPosition(), Quaternion.identity);
        tokenStatus = TokenStatus.LOCKED;

        originalScale = tokenTransform.localScale;

        tokenComponent = tokenTransform.GetComponent<TokenComponent>();
        tokenComponent.tokenInstance = this;

    }

    public void Despawn()
    {
        tokenTransform.SetPositionAndRotation(originalSpawnNodeComponent.GetPosition(), Quaternion.identity);
        SetParentNode(originalSpawnNode);
        tokenStatus = TokenStatus.LOCKED;
    }

    public void Spawn()
    {
        SetParentNode(originalSpawnNodeComponent.nextNode);
        tokenStatus = TokenStatus.MOVEABLE;
    }

    public bool IsColliding()
    {
        return tokenTransform.GetComponent<TokenComponent>().isColliding;
    }

    public void Interact()
    {
        if(tokenStatus == TokenStatus.LOCKED)
        {
            if(originalSpawnNodeComponent.interactable == true)
            {
                originalSpawnNodeComponent.Interact();
                return;
            }
        }

        if (tokenStatus == TokenStatus.MOVEABLE)
        {
            if(GetParentNodeComponent().interactable == true)
            {
                GetParentNodeComponent().Interact();
            }
        }
    }

    public void Highlight()
    {
        if (tokenStatus == TokenStatus.MOVEABLE)
        {
            if (GetParentNodeComponent().interactable == true)
            {
                GetParentNodeComponent().Highlight();
            }
        }
    }

    public void Unhighlight()
    {
        if (tokenStatus == TokenStatus.MOVEABLE)
        {
            if (GetParentNodeComponent().interactable == true)
            {
                GetParentNodeComponent().Unhighlight();
            }
        }
    }

    public void ShowArrow(bool isActive)
    {
        this.tokenTransform.GetChild(0).gameObject.SetActive(isActive);
    }

    #endregion
}

public enum PlayerType {BLUE, GREEN, RED, YELLOW}
public enum TokenStatus {LOCKED, MOVEABLE, WON}
