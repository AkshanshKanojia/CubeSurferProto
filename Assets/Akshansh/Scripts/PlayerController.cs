using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f,minXlimit,maxXlimit;

    //Moves player on x axis in givn direction (not normalized due to random touch intensity)
    public void MovePlayer(float _dir)
    {
        transform.position += moveSpeed * Time.deltaTime * new Vector3(-_dir,0,0);//- to avoid inverted movement based on input manager calculations
        Vector3 _tempPos = transform.position;
        _tempPos.x = Mathf.Clamp(_tempPos.x, minXlimit, maxXlimit);
        transform.position = _tempPos;
    }

}
