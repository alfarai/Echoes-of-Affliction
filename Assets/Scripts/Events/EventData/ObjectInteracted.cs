using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteracted {
    public GameObject interactedObj; //object interacted on: can be Objectlaceable, ObjectBreakable, or holdObject
    public string interactable; //object used : can be hammer, plank, key
    public string interaction; //type of interaction: can be hold,place,break, breakloot
}
