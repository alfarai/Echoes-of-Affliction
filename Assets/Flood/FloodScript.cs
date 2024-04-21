using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodScript : MonoBehaviour
{
    public float targetY = 10; // Target Y pos
    public float duration = 180f; // Duration for the scaling animation
    //public FloodLevel floodLevel = FloodLevel.Level0;
    public enum FloodLevel
    {
        Level0 = 0,
        Level1 = 3,
        Level2 = 5,
        Level3 = 8,
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ScaleObject());
    }

    public void setFloodLevel(Vector3 scale)
    {
   
        /*if(scale.y >= 3 && scale.y <5)
        {
            floodLevel = FloodLevel.Level1;
        }
        else if(scale.y>=5 && scale.y < 8 )
        {
            floodLevel = FloodLevel.Level2;
        }
        else if (scale.y >= 8)
        {
            floodLevel = FloodLevel.Level3;
        }*/
  
    }
    IEnumerator ScaleObject()
    {
        Vector3 initialPos = transform.position;
        Vector3 targetPos = new Vector3(initialPos.x, targetY, initialPos.z);
       
    
            float currentTime = 0f;

        while (currentTime < duration)
        {
            float scaleFactor = Mathf.Lerp(initialPos.y, targetPos.y, currentTime / duration);
            transform.position = new Vector3(initialPos.x, scaleFactor, initialPos.z);
            //setFloodLevel(transform.localScale);
            currentTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final scale is set correctly to avoid precision errors
        transform.position = targetPos;
    }
}
