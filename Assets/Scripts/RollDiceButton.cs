using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDiceButton : MonoBehaviour {

    public Animator animator;
    public bool isClickable;
    public PlayerType playerType;
    public DiceCube diceCube;
    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    private void Update()
    {
        isClickable = gm.currentPlayer.playerType == playerType && gm.waitingForRoll == true && !diceCube.isRolling;
        animator.SetBool("isInteractive", isClickable);
    }

    private void OnMouseDown()
    {
        if(isClickable == true)
        {
            transform.position = transform.position - Vector3.up * 0.3f;
        }
    }

    private void OnMouseUp()
    {
        if(isClickable == true)
        {
            transform.position = transform.position + Vector3.up * 0.3f;
            diceCube.StartCoroutine(diceCube.Roll());
        }
    }

}
