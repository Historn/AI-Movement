using UnityEngine;
using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction
using UnityEngine.AI;

[Action("MyActions/Hide")]
[Help("Get the Vector3 for hiding.")]
public class HideBB : BasePrimitiveAction
{
    [InParam("game object")]
    [Help("Game object to add the component, if no assigned the component is added to the game object of this behavior")]
    public GameObject targetGameobject;

    [OutParam("hide")]
    [Help("Vector3 for higing.")]
    public Vector3 hide;

    //[HideInInspector]
    public GameObject cop;
    private GameObject[] hidingSpots;

    public override TaskStatus OnLatentStart()
    {
        cop = GameObject.FindGameObjectWithTag("cop");
        hidingSpots = GameObject.FindGameObjectsWithTag("hide");
        return TaskStatus.COMPLETED;
    }

    public override TaskStatus OnUpdate()
    {
        hide = AI.Movement.HideValue(cop, hidingSpots);
        return TaskStatus.COMPLETED;
    }
}