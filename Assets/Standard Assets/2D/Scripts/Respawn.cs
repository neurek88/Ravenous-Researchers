using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;


//This script acts as a dead zone, so when the Player hits it, the level is reloaded.
public class Respawn : MonoBehaviour
{
	//This method is called when an object (with RigidBody2D and Collider2D) collides with this
	public void reloadScene()
	{
        //Reload the level (Unity scene)
        //string activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Opening");
    }
    public void loadTitleScene()
    {
        SceneManager.LoadScene("Title");
    }

}