using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
public class PlayerMovementController : NetworkBehaviour
{
    public GameObject PlayerModel;
    public GameObject[] playerModels;
    public float speed = 6f;
    public float mouseSensivity = 100f;
    public CharacterController controller;
    public bool isDead, notMove;
    void Start()
    {
        PlayerModel.SetActive(false);
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (PlayerModel.activeSelf == false)
            {
                SetPosition();
                PlayerModel.SetActive(true);
                PlayerCosmeticsSetup();
                GetComponent<PlayerSetup>().enabled = true;
            }
            if (hasAuthority)
            {
                if (isDead)
                {
                    Movement();
                }
            }
        }
    }
    public void SetPosition()
    {
        var rnd = Random.Range(0, GameManager.singleton.HousePosition.Count);
        Transform sitPosition = GameManager.singleton.HousePosition[rnd].transform.GetChild(0).transform;
        transform.position = new Vector3(sitPosition.position.x, 1f, sitPosition.position.z);
        transform.rotation = sitPosition.rotation;
        GameManager.singleton.playerHouse.Add(GameManager.singleton.HousePosition[rnd], transform.GetComponent<PlayerObjectController>());
        GameManager.singleton.HousePosition.RemoveAt(rnd);
    }
    public void PlayerCosmeticsSetup()
    {
        GameObject playerModeGO = Instantiate(playerModels[GetComponent<PlayerObjectController>().PlayerCharacther], PlayerModel.transform);
    }
    public void Movement()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.right * xDirection + transform.forward * zDirection;
        controller.Move(moveDirection * speed * Time.deltaTime);
    }
}
