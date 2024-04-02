using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHub 
{
    //https://www.youtube.com/watch?v=0G6LAyINwcw&ab_channel=FumetsuHito
    //used for passing parameters during events
    //before invoking an event, DataHub values are updated first
    //game listeners read values from the datahub, game events write to data hub first before invoking an event
    //essentially acts as middle man
    public static ObjectInteracted ObjectInteracted = new ObjectInteracted();
    public static PlayerStatus PlayerStatus = new PlayerStatus();
    public static ObjectiveConditionHelper ObjectiveHelper = new ObjectiveConditionHelper();
    public static TriggerInteracted TriggerInteracted = new TriggerInteracted();
}
