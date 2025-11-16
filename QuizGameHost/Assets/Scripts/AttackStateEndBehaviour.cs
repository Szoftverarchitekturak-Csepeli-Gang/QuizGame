using UnityEngine;

public class AttackStateEndBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var soldier = animator.gameObject.GetComponent<SoldierController>();
        if (soldier != null)
            soldier.OnAttackAnimationEnd();
    }
}
