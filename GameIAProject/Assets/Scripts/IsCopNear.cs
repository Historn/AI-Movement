using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Framework;

[Condition("MyConditions/Is Cop Near?")]
[Help("Checks whether Cop is near the Treasure.")]
public class IsCopNear : ConditionBase
{
    public override bool Check()
    {
        GameObject cop = GameObject.FindGameObjectWithTag("cop");
        GameObject treasure = GameObject.FindGameObjectWithTag("treasure");
        return Vector3.Distance(cop.transform.position, treasure.transform.position) < 3f;
    }
}