using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarePlayer : MonoBehaviour
{
    public RealisticEyeMovements.LookTargetController LookTargetController;
    public GameObject NoteLane;
    public GameManager GameManager;

    private GameManager.GameLevel gameLevel;

    [SerializeField]
    //private float animationStep = 10;


    /// <summary>
    /// ����� ��Ÿ����Ʈ�� �÷��̾ �Ĵٺ��� ����� �Լ�
    /// </summary>
    public void Stare()
    {
        LookTargetController.lookAtPlayerRatio = 1;

        NoteLane.transform.localPosition = NoteLane.transform.localPosition - new Vector3(0f, -10f, 15f);
        gameLevel = GameManager.CurrentGameLevel;
        GameManager.CurrentGameLevel = GameManager.GameLevel.None;
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

        NoteLane.transform.localPosition = new Vector3(0f, -1f, 15f);
        GameManager.CurrentGameLevel = gameLevel;
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
