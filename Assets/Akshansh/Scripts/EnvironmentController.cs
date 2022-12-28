using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] enum ObjectTypes { MovingPlatform,CubePickup,Wall,
    Coin,Cube}
    [SerializeField] ObjectTypes BehaviourType;
    public bool IsActive = true;
    [SerializeField] float moveSpeed = 3f,minFallDist = -2f;
    [SerializeField] int cubeBlockLimit = 1,cubesToAdd = 1;

    PlayerController playerCont;

    private void Start()
    {
        playerCont = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        if(IsActive)
        {
            ObjectManager();
        }
    }

    //applies reequired actions based on behaviour type
    void ObjectManager()
    {
        switch(BehaviourType)
        {
            case ObjectTypes.MovingPlatform:
                MoveObject();
                break;
            case ObjectTypes.Cube:
                if(transform.position.y<minFallDist)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    void MoveObject()
    {
        transform.position += moveSpeed * Time.deltaTime * -Vector3.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cubes")&&IsActive)
        {
            switch (BehaviourType)
            {
                case ObjectTypes.Wall:
                    collision.transform.parent = transform;
                    if(transform.childCount>=cubeBlockLimit)
                    IsActive = false;
                    break;
                case ObjectTypes.CubePickup:
                    playerCont.AddCubes(cubesToAdd);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
