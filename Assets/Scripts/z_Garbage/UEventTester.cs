using UnityEngine;

namespace StormRend.Tests
{
	public class UEventTester : MonoBehaviour
	{
        public string testMessage = "This is a test!";

        public void Call()
        {
            Debug.Log(testMessage);
        }
	}
}
