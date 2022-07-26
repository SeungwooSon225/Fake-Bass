using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarePlayer : MonoBehaviour
{
    public RealisticEyeMovements.LookTargetController LookTargetController;

    [SerializeField]
    private float animationStep = 10;


    /// <summary>
    /// ����� ��Ÿ����Ʈ�� �÷��̾ �Ĵٺ��� ����� �Լ�
    /// </summary>
    public void Stare()
    {
        LookTargetController.lookAtPlayerRatio = 1;
    }

    //IEnumerator Staring()
    //{
    //    LookTargetController.lookAtPlayerRatio = 1;

    //    for (int i = 0; i < animationStep; i++)
    //    {
    //        EyeAndHeadAnimator.mainWeight += 1.0f / animationStep;

    //        yield return new WaitForSeconds(1.0f / animationStep);
    //    }

    //    yield return null;
    //}


    /// <summary>
    /// ����� ��Ÿ����Ʈ�� ���� �ڼ��� ���ƿ��� ����� �Լ�
    /// </summary>
    public void Return()
    {
        LookTargetController.lookAtPlayerRatio = 0;
    }

    //IEnumerator Returning()
    //{
    //    LookTargetController.lookAtPlayerRatio = 0;

    //    for (int i = 0; i < animationStep; i++)
    //    {
    //        EyeAndHeadAnimator.mainWeight -= 1 / animationStep;

    //        yield return new WaitForSeconds(1 / animationStep);
    //    }

    //    yield return null;
    //}


}
