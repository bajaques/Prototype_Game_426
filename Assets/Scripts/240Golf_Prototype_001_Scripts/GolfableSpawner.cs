using UnityEngine;
using UnityEngine.Networking;

public class GolfableSpawner : NetworkBehaviour {

	public GameObject golfPrefab;
	public int numGolfBalls;

	public override void OnStartServer() {
		var spawnPosition = new Vector3(50, 5, 50);
		var spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

		var ball = (GameObject)Instantiate(golfPrefab, spawnPosition, spawnRotation);
		print(ball.transform.position);
		NetworkServer.Spawn(ball);
	}
}
