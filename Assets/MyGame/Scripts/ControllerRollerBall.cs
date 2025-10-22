using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ControllerRollerBall : MonoBehaviour
{

	public float speed;
	public float jumpHeight;

	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		//SetCountText();
		//winText.text = "";
	}

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

		rb.AddForce(movement * speed);

		if (Input.GetKeyDown("space"))
		{
			Vector3 jump = new Vector3(0.0f, jumpHeight, 0.0f);
			rb.AddForce(jump);
		}


	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("PickUp"))
		{
			other.gameObject.SetActive(false);
			//count++;
			//SetCountText();

		}

		if (other.gameObject.CompareTag("Death"))
		{
			SceneManager.LoadScene("RollerBall");
		}
	}

	/*void SetCountText()
	{
		countText.text = "Count: " + count.ToString();
		if (count >= numPickups)
		{
			winText.text = "You win!";
		}
	}*/
}