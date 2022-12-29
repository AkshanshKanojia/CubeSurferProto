using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreTxt;
    [SerializeField] GameObject goUI, clearUI;

    GameManager mang;

    public bool IsClear = false;

    private void Start()
    {
        mang = GameManager.instance;
        scoreTxt.text = "Coins: " + mang.Coins;
    }
    public void ClearLevel()
    {
        var _temp = FindObjectOfType<PlayerController>();
        _temp.IsActive = false;
        _temp.GetComponent<EnvironmentController>().enabled = false;
        if(IsClear)
        {
            clearUI.SetActive(true);
        }
        else
        {
            goUI.SetActive(true);
        }
    }

    public void SetLevel(int _index)
    {
        SceneManager.LoadScene(_index);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void AddCoin(int _ammount)
    {
        mang.Coins += _ammount;
        scoreTxt.text = "Coins: " + mang.Coins;
    }
}
