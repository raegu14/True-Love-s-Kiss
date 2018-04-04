using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SwitchRoom : MonoBehaviour {

  public string nextLevelName;

  void OnTriggerEnter2D(Collider2D col)
  {
    if(col.gameObject.tag == "Player")
    {
      col.GetComponent<Player>().SetActive(false);
      GameObject.Find("Main Camera").GetComponent<Menu>().FadeOut(false, nextLevelName);
      GameObject.Find("META").GetComponent<Meta>().Updata();
    }
  }
}
