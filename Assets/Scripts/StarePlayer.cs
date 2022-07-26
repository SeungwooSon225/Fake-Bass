using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarePlayer : MonoBehaviour
{
    public RealisticEyeMovements.LookTargetController LookTargetController;

    [SerializeField]
    private float animationStep = 10;


    /// <summary>
    /// 버츄얼 기타리스트가 플레이어를 쳐다보게 만드는 함수
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
    /// 버츄얼 기타리스트가 원래 자세로 돌아오게 만드는 함수
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
