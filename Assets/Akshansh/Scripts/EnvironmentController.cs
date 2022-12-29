using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField]
    enum ObjectTypes
    {
        MovingPlatform, CubePickup, Wall, Lava,
        Coin, Cube, TrackPlayer,LevelEnd
    }
    [SerializeField] ObjectTypes BehaviourType;
    public bool IsActive = true;
    [SerializeField] float moveSpeed = 3f, minFallDist = -2f, cubeHeight = 1;
    [SerializeField] int cubeBlockLimit = 1, cubesToAdd = 1;
    [SerializeField] GameObject cubePickupPref,wallPref;

    PlayerController playerCont;
    Vector3 posOffset;

    private void Start()
    {
        playerCont = FindObjectOfType<PlayerController>();
        posOffset = transform.position - playerCont.transform.position;
        if (BehaviourType == ObjectTypes.CubePickup|| BehaviourType == ObjectTypes.Wall)
            GenerateCubeLayer();
    }

    private void GenerateCubeLayer()
    {
        int _length = cubeBlockLimit;
        GameObject _tempPref =  wallPref;
        if (BehaviourType == ObjectTypes.CubePickup)
        {
            _length = cubesToAdd;
            _tempPref = cubePickupPref;
        }
        for (int i = 1; i < _length; i++)
        {
            Instantiate(_tempPref, transform.position + (new Vector3(0, cubeHeight, 0) * i),
                Quaternion.identity).transform.parent = transform;//creat cubes in incremental order based on height
        }
    }

    private void Update()
    {
        if (IsActive)
        {
            ObjectManager();
        }
    }

    //applies required actions based on behaviour type
    void ObjectManager()
    {
        switch (BehaviourType)
        {
            case ObjectTypes.MovingPlatform:
                MoveObject();
                break;
            case ObjectTypes.Cube:
                if (transform.position.y < minFallDist)
                {
                    transform.parent = null;
                    Destroy(gameObject);
                    CheckGameOver();
                }
                break;
            case ObjectTypes.TrackPlayer:
                transform.position = playerCont.transform.position + posOffset;
                break;
        }
    }

    void MoveObject()
    {
        transform.position += moveSpeed * Time.deltaTime * -Vector3.forward;
    }

    void CheckGameOver()
    {
        if(playerCont.transform.childCount==0)
        {
            FindObjectOfType<LevelManager>().ClearLevel();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cubes") && IsActive)
        {
            switch (BehaviourType)
            {
                case ObjectTypes.Wall:
                    collision.transform.parent = transform;
                    if (transform.childCount >= cubeBlockLimit)
                        IsActive = false;
                    CheckGameOver();
                    break;
                case ObjectTypes.CubePickup:
                    playerCont.AddCubes(cubesToAdd);
                    Destroy(gameObject);
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.GetComponent<Collider>().CompareTag("Cubes") && IsActive)
        {
            switch (BehaviourType)
            {
                case ObjectTypes.Lava:
                    collision.isTrigger = true;
                    break;
                case ObjectTypes.LevelEnd:
                    FindObjectOfType<LevelManager>().IsClear = true;
                    break;
                case ObjectTypes.Coin:
                    FindObjectOfType<LevelManager>().AddCoin(1);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
