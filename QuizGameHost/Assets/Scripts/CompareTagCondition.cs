using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CompareTag", story: "[Agent] has [TagName] tag", category: "Conditions", id: "8deb71979217a97543b3e8e22e98be56")]
public partial class CompareTagCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<string> TagName;

    public override bool IsTrue()
    {
        return Agent.Value.tag == TagName;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
