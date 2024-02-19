using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodScript : MonoBehaviour
{
    public float targetScaleY = 10; // Target Y scale
    public float duration = 180f; // Duration for the scaling animation
    public FloodLevel floodLevel = FloodLevel.Level0;
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
   
        if(scale.y >= 3 && scale.y <5)
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
        }
  
    }
    IEnumerator ScaleObject()
    {
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(initialScale.x, targetScaleY, initialScale.z);
       
    
            float currentTime = 0f;

        while (currentTime < duration)
        {
            float scaleFactor = Mathf.Lerp(initialScale.y, targetScale.y, currentTime / duration);
            transform.localScale = new Vector3(initialScale.x, scaleFactor, initialScale.z);
            setFloodLevel(transform.localScale);
            currentTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final scale is set correctly to avoid precision errors
        transform.localScale = targetScale;
    }
}
