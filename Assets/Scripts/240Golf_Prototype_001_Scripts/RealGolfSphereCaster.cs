using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealGolfSphereCaster : MonoBehaviour
{
    [SerializeField] private GameObject GolfingManny;
    [SerializeField] private GameObject NormalManny;
    private Vector3 regularScale;
    private Vector3 currentPosition;
    private float regularX;
    private float regularZ;
    private float x;
    private float y;
    private float z;
    private int count = 0;

    private List<float> golfableObjects = new List<float>();
    private Dictionary<float,GameObject> golfableDict = new Dictionary<float, GameObject>();
    private Dictionary<GameObject, float> golfableDict2 = new Dictionary<GameObject, float>();
    private GameObject currentTargetGolfable;
    private Vector3 currentTargetGolfable2;

    private Color originalColor;

    void Start()
    {
        regularScale = transform.localScale;
        x = transform.localScale.x;
        regularX = x;
        y = transform.localScale.y;
        z = transform.localScale.z;
        regularZ = z;
        currentTargetGolfable = null;

        originalColor = GetComponent<Renderer>().material.color;
    }

	void Update ()
    {
        if (x < 300.0f && z < 300.0f)
        {
            x = transform.localScale.x + Time.deltaTime * 300.0f;
            z = transform.localScale.z + Time.deltaTime * 300.0f;
            Vector3 newLocalScale = new Vector3(x, y, z);
            transform.localScale = newLocalScale;
        }

        if (Input.GetKeyDown("joystick button 0"))
        {
            if (golfableObjects.Count != 0)
            {
                if (currentPosition != GolfingManny.transform.position)
                {
                    UpdateGolfableStack();
                }

                if (count == 0) golfableDict[golfableObjects[golfableObjects.Count - 1]].GetComponent<IGolfable>().ChosenWithinGolfBubble(false);
                else golfableDict[golfableObjects[count - 1]].GetComponent<IGolfable>().ChosenWithinGolfBubble(false);

                golfableDict[golfableObjects[count]].GetComponent<IGolfable>().ChosenWithinGolfBubble(true);
                currentTargetGolfable = golfableDict[golfableObjects[count]];
                GolfingManny.transform.parent.GetComponent<Player_Controller_Manager_PP_001>().currentTargetGolfable = currentTargetGolfable;//

                Camera camera = GolfingManny.transform.parent.GetComponent<Player_Manager_PP_001>().playerCam;
                Vector3 cameraPos = camera.transform.position;
                Vector3 cameraForward = camera.transform.forward;
                Vector3 vectorToTarget = currentTargetGolfable.transform.position - cameraPos;
                Vector3 vectorToPlayer = GolfingManny.transform.position - cameraPos;
                float angleToTarget = Vector3.Angle(cameraForward, vectorToTarget);
                float angleToPlayer = Vector3.Angle(cameraForward, vectorToPlayer);

                Vector3 leftOrRight = new Vector3(0, 0, 0);
                if (angleToPlayer <= angleToTarget) leftOrRight = camera.transform.right.normalized;//
                else leftOrRight = -camera.transform.right.normalized;//

                float distanceToTarget = Vector3.Distance(cameraPos, currentTargetGolfable.transform.position);
                float angleBetweenTarget = Vector3.Angle(leftOrRight, vectorToTarget);
                float adjacentTriLeg = Mathf.Abs(distanceToTarget * Mathf.Cos(angleBetweenTarget));
                float oppositeTriLeg = Mathf.Abs(distanceToTarget * Mathf.Sin(angleBetweenTarget));

                Vector3 straightLineBase = adjacentTriLeg * leftOrRight;
                straightLineBase = straightLineBase + cameraPos;

                Vector3 straightLineTop = straightLineBase - currentTargetGolfable.transform.position;
                straightLineTop.Normalize();
                straightLineTop = straightLineTop * 1.749813f;
                currentTargetGolfable2 = currentTargetGolfable.transform.position + straightLineTop;

                if (count != golfableObjects.Count - 1) count++;
                else count = 0;
            }
        }

        if (Input.GetKey("joystick button 0"))
        {
            if (currentTargetGolfable != null)
            { 
                currentTargetGolfable2.y = GolfingManny.transform.position.y;
                GolfingManny.transform.position = Vector3.MoveTowards(GolfingManny.transform.position, currentTargetGolfable2, 5.0f * Time.deltaTime);

                Color random = new Color(Random.value, Random.value, Random.value, 1.0f);
                GolfingManny.GetComponentInChildren<Renderer>().material.color = random;
                random.a = 0.5f;
                GetComponent<Renderer>().material.color = random;

                Vector3 targetFinal = currentTargetGolfable.transform.position;
                //targetFinal.z = GolfingManny.transform.position.x;
                targetFinal.y = GolfingManny.transform.position.y;
                //GolfingManny.transform.LookAt(targetFinal);

                if (GolfingManny.transform.position == currentTargetGolfable2)
                {
                    GolfingManny.GetComponentInChildren<Renderer>().material.color = Color.white;
                    GetComponent<Renderer>().material.color = originalColor;
                    GolfingManny.transform.parent.GetComponent<Player_Input_Manager_PP_001>().ChangeFromFreeGolfToCircle(false);
                    
                    GolfingManny.GetComponentInChildren<Renderer>().material.color = Color.white;
                    GetComponent<Renderer>().material.color = originalColor;
                 }
            }
        }

        Debug.DrawRay(transform.position, -transform.right, Color.green);//
        currentPosition = GolfingManny.transform.position;
    }


    void OnTriggerEnter(Collider col)
    {
        if ((col.gameObject.layer == LayerMask.NameToLayer("GOLFABLE")) || (col.gameObject.layer == LayerMask.NameToLayer("GolfBall_BVS")))
        {
            Vector3 target = col.gameObject.transform.position - transform.position;
            float angle = Vector3.Angle(-transform.right, target);
            golfableObjects.Add(angle);
            golfableDict.Add(angle, col.gameObject);
            golfableDict2.Add(col.gameObject, angle);
            golfableObjects.Sort();
            col.gameObject.GetComponent<IGolfable>().WithinGolfBubble(true);
            col.gameObject.GetComponent<IGolfable>().ChosenWithinGolfBubble(false);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if ((col.gameObject.layer == LayerMask.NameToLayer("GOLFABLE")) || (col.gameObject.layer == LayerMask.NameToLayer("GolfBall_BVS")))
        {
            col.gameObject.GetComponent<IGolfable>().ChosenWithinGolfBubble(false);
            col.gameObject.GetComponent<IGolfable>().WithinGolfBubble(false);
            golfableDict.Remove(golfableDict2[col.gameObject]);
            golfableObjects.Remove(golfableDict2[col.gameObject]);
            golfableDict2.Remove(col.gameObject);
        }
    }

    public void ReturnToScale()
    {
        count = 0;
        currentTargetGolfable = null;
        foreach (var item in golfableDict2.Keys)
        {
            item.GetComponent<IGolfable>().WithinGolfBubble(false);
        }
        golfableObjects.Clear();
        golfableDict.Clear();
        golfableDict2.Clear();
        transform.localScale = regularScale;
        x = regularX;
        z = regularZ;
    }

    private void UpdateGolfableStack()
    {
        golfableObjects.Clear();
        golfableDict.Clear();
        foreach (var item in golfableDict2.Keys)
        {
            Vector3 target = item.transform.position - transform.position;
            float angle = Vector3.Angle(-transform.right, target);
            golfableObjects.Add(angle);
            golfableDict.Add(angle, item);
        }

        golfableDict2.Clear();
        foreach (KeyValuePair<float, GameObject> entry in golfableDict)
        {
            golfableDict2.Add(entry.Value, entry.Key);
        }

        golfableObjects.Sort();
    }

    public bool CheckGolfableStacks(GameObject go)
    {
        if (golfableDict2.ContainsKey(go))
        {
            return true;
        }
        else return false;
    }
}




/*Vector3 temp = currentTargetGolfable2 - GolfingManny.transform.position;
                    temp.Normalize();
                    float dist = Vector3.Distance(currentTargetGolfable2, GolfingManny.transform.position);
                    dist = dist / 10.0f;
                    temp = temp * dist;
                    temp = temp + GolfingManny.transform.position;
                    temp.y = GolfingManny.transform.position.y;*/
// GolfingManny.transform.position = Vector3.MoveTowards(GolfingManny.transform.position, temp, 25 * Time.deltaTime);

/*float finalDistance = Vector3.Distance(GolfingManny.transform.position, currentTargetGolfable2);
          if (finalDistance < .85f) finalApproach = true;

          if (finalApproach)
          { 
              GolfingManny.GetComponentInChildren<Renderer>().material.color = Color.white;
              GetComponent<Renderer>().material.color = originalColor;
              currentTargetGolfable2.y = GolfingManny.transform.position.y;
              GolfingManny.transform.position = Vector3.MoveTowards(GolfingManny.transform.position, 
                  currentTargetGolfable2 , 5 * Time.deltaTime);
              if (GolfingManny.transform.position == currentTargetGolfable2)
              {
                  //GO TO REAL GOLF MODE
                  GolfingManny.transform.parent.GetComponent<Player_Input_Manager_PP_001>().ChangeFromFreeGolfToCircle(false);
                  Vector3 targetFinal = currentTargetGolfable.transform.position;
                  targetFinal.y = GolfingManny.transform.position.y;
                  GolfingManny.transform.LookAt(targetFinal);
              }
          }*/
