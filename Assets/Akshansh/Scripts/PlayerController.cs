using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f,minXlimit,maxXlimit,cubeHeight = 1;
    [SerializeField] GameObject cubPref;
    [SerializeField] int startingCubeCount = 1;

    private void Start()
    {
        AddCubes(startingCubeCount - 1);//1 cube is added by default
    }

    //Moves player on x axis in givn direction (not normalized due to random touch intensity)
    public void MovePlayer(float _dir)
    {
        transform.position += moveSpeed * Time.deltaTime * new Vector3(-_dir,0,0);//- to avoid inverted movement based on input manager calculations
        Vector3 _tempPos = transform.position;
        _tempPos.x = Mathf.Clamp(_tempPos.x, minXlimit, maxXlimit);
        transform.position = _tempPos;
    }

    //generate cubes below already existing cubes based on ammount
    public void AddCubes(int _ammount)
    {
        int _tempChildCount = transform.childCount;
        List<Rigidbody> _tempChilds = new List<Rigidbody>();
        //disable rb for avoiding physics error if any
        for(int i = 0;i<_tempChildCount;i++)
        {
            _tempChilds.Add(transform.GetChild(i).GetComponent<Rigidbody>());
            _tempChilds[i].isKinematic = true;
        }

        //increment existing cubes and add new one below it
        for(int i=0; i < _ammount; i++)
        {
            //update existing cubes
            foreach(Rigidbody _tempChild in _tempChilds)
            {
                _tempChild.transform.position += new Vector3(0, cubeHeight, 0);
            }
            Rigidbody _tempRb = Instantiate(cubPref, transform).GetComponent<Rigidbody>();
            _tempRb.isKinematic = true;
            _tempChilds.Add(_tempRb);
        }
        //enable physics again once added
        foreach(var v in _tempChilds)
        {
            v.isKinematic = false;
        }

    }
}
