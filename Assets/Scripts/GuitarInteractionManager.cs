using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarInteractionManager : MonoBehaviour
{
    public GameObject rightController;
    public GameObject leftController;


    private float previousDistanceBetweenControllers = 0f;
    [SerializeField]
    private float sensitivity = 0.05f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGuitarTransform();
    }


    /// <summary>
    /// VR controller로 Virtual Guitar를 컨트롤하는 함수
    /// 기타 바디의 위치는 오른쪽 controller에 고정
    /// 기타 바디의 x축 회전은 오른쪽 controller로 조절
    /// 기타 넥의 방향은 오른쪽 controller와 왼쪽 controller 사이 각도로 조절
    /// </summary>
    public void UpdateGuitarTransform()
    {
        // 기타 바디의 위치를 오른쪽 controller 위치에 고정
        gameObject.transform.position = rightController.transform.position;

        // 오른쪽 controller와 왼쪽 controller 사이의 방향 벡터를 구함
        Vector3 guitarDirectionVector = leftController.transform.position - rightController.transform.position;
        if (guitarDirectionVector.magnitude == 0) return;

        Quaternion guitarNeckDirection = Quaternion.LookRotation(guitarDirectionVector);


        // 오른쪽 controller의 x축 회전값을 기타 바디의 x축 회전값으로 사용
        float guitarBodyRotationX = -rightController.transform.rotation.eulerAngles.z - 60f;

        // 위에서 구한 guitarNeckDirection 방향으로  guitar가 바라보게 만듦
        gameObject.transform.rotation = guitarNeckDirection;
        // 기타의 바디가 guitarNeckDirection 방향을 바라보게 되므로 x축으로 90도 회전시켜서 기타 넥이 guitaNeckDirection을 바라보게 조정
        // 기타 바디의 x축 회전 값을 오른쪽 controller의 x축 회전값으로 조정
        gameObject.transform.Rotate(new Vector3(guitarBodyRotationX, 90f, 0f));
    }


    /// <summary>
    /// 플레이어가 코드를 변화시켰는지 확인하는 함수
    /// 플레이어가 hit key를 누를 때(기타 줄을 튕길 때)마다 실행되며 hit key를 직전에 눌렀을 때와 비교하여
    /// 왼쪽, 오른쪽 controller 사이의 거리가 sensitivity 보다 크게 변하면 코드를 변화시킨것으로 인식하고 그렇지 않으면 코드를 변화시키지 않은 것으로 인식한다.
    /// </summary>
    /// <returns> true: 코드가 변함 / false: 코드가 변하지 않음 </returns>
    public bool CheckChordChange()
    {
        // 두 controller 사이의 거리를 계산
        float currentDistanceBetweenControllers = (rightController.transform.position - leftController.transform.position).magnitude;
        // 전에 hit key가 눌렸을 때 controller 사이의 거리와 현재 controller 사이의 거리의 차를 계산
        float distanceDifference = Mathf.Abs(currentDistanceBetweenControllers - previousDistanceBetweenControllers);
        // 다음번 검사를 위해 previousDistanceBetweenControllers를 currentDistanceBetweenControllers로 변경
        previousDistanceBetweenControllers = currentDistanceBetweenControllers;

        //Debug.Log("distance: " + distanceDifference);

        // 거리 차이가 sensitivity보다 크면 true를 return 그렇지 않으면 false를 return
        if (distanceDifference > sensitivity) return true;
        else return false;
    }
}
