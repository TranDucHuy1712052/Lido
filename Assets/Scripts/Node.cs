using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    #region var
    public bool allowsKilling = true;
    public bool interactable = false;

    public Node nextNode;

    // Used to keep track of any player on top of the node
    //public List<Token> bluePlayers = new List<Token>();
    //public List<Token> greenPlayers = new List<Token>();
    //public List<Token> redPlayers = new List<Token>();
    //public List<Token> yellowPlayers = new List<Token>();
    //public List<Token> players = new List<Token>();

    Token currentPlayer;                //ONLY 1 TOKEN PER 1 NODE

    // Used to determine the player position on top of the node
    public Vector3 offset = new Vector3(0, 2, 0);
    
    // Used only if the node is an entrance for a specific player type
    public Node nextToEntranceNode;
    public bool isEntrance = false;
    public PlayerType affectedPlayerType;

    public GameObject posGOPrefab;
    public GameObject posGO;
    public float smoothness = .2f;
    public float[] scaleMultipliers = 
        { 0.5f, 0.5f, 0.5f, 0.4f, 0.4f, 0.3f, 0.3f, 0.3f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f };

    public Material defaultMaterial;
    public Material highLightMaterial;

    private GameManager gm;

    #endregion

    #region mono
    private void Start()
    {
        gm = GameManager.instance;
        defaultMaterial = GetComponent<MeshRenderer>().material;
    }
    public void Update()
    {
        if (this.currentPlayer == null) return;

        Vector3 direction;
        //if (count == 1)
        {
            currentPlayer.tokenTransform.localScale = Vector3.Slerp(currentPlayer.tokenTransform.localScale, currentPlayer.originalScale, smoothness);
            direction = Vector3.Slerp(Vector3.zero, GetPosition() - currentPlayer.tokenTransform.position, smoothness);
            currentPlayer.tokenTransform.Translate(direction);
            return;
        }
        //Transform aux = posGO.transform.GetChild(count - 2);
        //for (int i = 0; i < count; i++)
        //{
        //    players[i].tokenTransform.localScale = Vector3.Slerp(players[i].tokenTransform.localScale, players[i].originalScale * scaleMultipliers[count - 2], smoothness);
        //    direction = Vector3.Slerp(Vector3.zero, aux.GetChild(i).position - players[i].tokenTransform.position, smoothness);
        //    players[i].tokenTransform.Translate(direction);
        //}
    }

    #endregion

    #region collider
    private void OnMouseOver()
    {
        Highlight();
    }

    private void OnMouseExit()
    {
        Unhighlight();
    }
    private void OnMouseDown()
    {
        Interact();
    }

    #endregion


    public void Interact()
    {
        if (interactable == true)
        {
            Unhighlight();
            //Token chosenToken = null;
            //switch (gm.currentPlayer.playerType)
            //{
            //    case PlayerType.BLUE:
            //        chosenToken = bluePlayers[0];
            //        break;
            //    case PlayerType.GREEN:
            //        chosenToken = greenPlayers[0];
            //        break;
            //    case PlayerType.RED:
            //        chosenToken = redPlayers[0];
            //        break;
            //    case PlayerType.YELLOW:
            //        chosenToken = yellowPlayers[0];
            //        break;
            //}

            gm.StartCoroutine(gm.PlayToken(this.currentPlayer));
        }
    }

    public Node GetNextNode(PlayerType playerType)
    {
        if(isEntrance)
        {
            if (playerType == affectedPlayerType)
                return nextToEntranceNode;
        }
        return nextNode;
    }
    public bool IsOccupied() { return (this.currentPlayer != null); }

    public bool IsBlocked(Token token, int step)
    {
        for (int i = 0; i < step - 1; i++)
        {
            Node nextNode = GetNextNode(token.tokenType);
            if (nextNode != null && !nextNode.IsEmpty())
            {
                PlayerType player2 = nextNode.currentPlayer.tokenType;
                if (player2 != token.tokenType)         //another enemy!
                {

                }
            }
        }
        return false;           //dummy
    }

    #region player
    public void AddPlayer(Token _token)
    {
        _token.SetParentNode(transform);
        this.currentPlayer = _token;
        //players.Add(_token);
        if (currentPlayer != null)
            posGO = Instantiate(posGOPrefab, GetPosition(), transform.rotation);
        //switch (_token.tokenType)
        //{
        //    case PlayerType.BLUE:
        //        bluePlayers.Add(_token);
        //        break;
        //    case PlayerType.GREEN:
        //        greenPlayers.Add(_token);
        //        break;
        //    case PlayerType.RED:
        //        redPlayers.Add(_token);
        //        break;
        //    case PlayerType.YELLOW:
        //        yellowPlayers.Add(_token);
        //        break;
        //}
    }

    public void RemovePlayer(Token _token)
    {
        //_token.SetParentNode(null);                 //this token will no longer stand in this node.
        this.currentPlayer = null;
        Destroy(posGO);
        
        //players.Remove(_token);
        //if (players.Count == 0)
        //    Destroy(posGO);
        //switch (_token.tokenType)
        //{
        //    case PlayerType.BLUE:
        //        bluePlayers.Remove(_token);
        //        break;
        //    case PlayerType.GREEN:
        //        greenPlayers.Remove(_token);
        //        break;
        //    case PlayerType.RED:
        //        redPlayers.Remove(_token);
        //        break;
        //    case PlayerType.YELLOW:
        //        yellowPlayers.Remove(_token);
        //        break;
        //}
    }

    public Token GetPlayerToKill(Token killer)
    {
        //if (killer.tokenType != PlayerType.BLUE && bluePlayers.Count > 0)
        //    return bluePlayers[0];
        //if (killer.tokenType != PlayerType.GREEN && greenPlayers.Count > 0)
        //    return greenPlayers[0];
        //if (killer.tokenType != PlayerType.RED && redPlayers.Count > 0)
        //    return redPlayers[0];
        //if (killer.tokenType != PlayerType.YELLOW && yellowPlayers.Count > 0)
        //    return yellowPlayers[0];
        if (this.currentPlayer.tokenType != killer.tokenType)
        {
            return this.currentPlayer;
        }
        return null;
    }

    #endregion


    #region position
    public Vector3 GetPosition()
    {
        return transform.position + offset;
    }

    public Vector3 GetUpPosition()
    {
        return transform.position + 2 * offset;
    }
    #endregion

    public bool IsEmpty()
    {
        //return (bluePlayers.Count + greenPlayers.Count + redPlayers.Count + yellowPlayers.Count) == 0;
        return (currentPlayer == null);
    }


    #region highlight
    /// <summary>
    /// Change color to green of all the nodes ahead, if possible. To show da way...
    /// </summary>
    public void Highlight()
    {
        if(interactable)
        {
            Node auxNode = this;
            for (int i = 0; i <= gm.dice.value; i++)
            {
                auxNode.GetComponent<MeshRenderer>().material = auxNode.highLightMaterial;
                if (auxNode.isEntrance && gm.currentPlayer.playerType == auxNode.affectedPlayerType)
                    auxNode = auxNode.nextToEntranceNode;
                else
                    auxNode = auxNode.nextNode;
            }
        }
    }

    /// <summary>
    /// Revert color of all the nodes ahead, if possible. Because they dont know da way...
    /// </summary>
    public void Unhighlight()
    {
        if (interactable)
        {
            Node auxNode = this;
            for (int i = 0; i <= gm.dice.value; i++)
            {
                auxNode.GetComponent<MeshRenderer>().material = auxNode.defaultMaterial;
                if (auxNode.isEntrance && gm.currentPlayer.playerType == auxNode.affectedPlayerType)
                    auxNode = auxNode.nextToEntranceNode;
                else
                    auxNode = auxNode.nextNode;
            }
        }
    }

    #endregion

}
