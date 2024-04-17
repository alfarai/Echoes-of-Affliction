using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseBuilding : MonoBehaviour
{
    //time it takes to rotate on an axis
    public float xRotateTime = 2, yRotateTime = 2, zRotateTime = 2;

    //time it takes to collapse building
    public float collapseTime = 1;

    //time interval on when to rotate / smoothness of rotation
    public float rotateTimeInterval = 0.3f;

    //rotation amount to be applied
    public float tiltAmount = 1;
 
    //magnitude of change in position when collapsing downwards
    public float collapseAmount = 1;




    private float elapsed,totalElapsed;
    private bool isFullyRotatedOnX, isFullyRotatedOnY, isFullyRotatedOnZ, isCollapsed, isFullyCollapsed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf && !isFullyCollapsed)
        {
            elapsed += Time.deltaTime;
            if(elapsed >= rotateTimeInterval)
            {
                if(xRotateTime != 0 && !isFullyRotatedOnX)
                {
                    //apply rotation
                    //Vector3 newRotation = new Vector3(gameObject.transform.rotation.x + tiltSpeed*Mathf.Sign(xTiltBy), gameObject.transform.rotation.y, gameObject.transform.rotation.z);
                    gameObject.transform.Rotate(
                        tiltAmount,
                        0f,
                        0f,
                        Space.World
                        );
                    
                }
                if (yRotateTime != 0 && !isFullyRotatedOnY)
                {
                    //apply rotation
                    gameObject.transform.Rotate(
                        0f,
                        tiltAmount,
                        0f,
                        Space.World
                        );
                   
                }
                if (zRotateTime != 0 && !isFullyRotatedOnZ)
                {
                    //apply rotation
                    gameObject.transform.Rotate(
                        0f,
                        0f,
                        tiltAmount,
                        Space.World
                        );
                    
                }
                if(collapseTime != 0 && !isCollapsed)
                {
                    gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - collapseAmount, transform.position.z);
                }

                totalElapsed += elapsed;
                elapsed = 0;

                if (totalElapsed >= xRotateTime)
                {
                    isFullyRotatedOnX = true;
                }
                if (totalElapsed >= yRotateTime)
                {
                    isFullyRotatedOnY = true;
                }
                if (totalElapsed >= zRotateTime)
                {
                    isFullyRotatedOnZ = true;
                }
                if(totalElapsed >= collapseTime)
                {
                    isCollapsed = true;
                }
                
            }
            if (isFullyRotatedOnX && isFullyRotatedOnY && isFullyRotatedOnZ && isCollapsed)
            {
                Debug.Log("gameObject stopped collapsing");
                isFullyCollapsed = true;
                gameObject.GetComponent<CollapseBuilding>().enabled = false;
                
            }
        }
    }
}
