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
    /// VR controller�� Virtual Guitar�� ��Ʈ���ϴ� �Լ�
    /// ��Ÿ �ٵ��� ��ġ�� ������ controller�� ����
    /// ��Ÿ �ٵ��� x�� ȸ���� ������ controller�� ����
    /// ��Ÿ ���� ������ ������ controller�� ���� controller ���� ������ ����
    /// </summary>
    public void UpdateGuitarTransform()
    {
        // ��Ÿ �ٵ��� ��ġ�� ������ controller ��ġ�� ����
        gameObject.transform.position = rightController.transform.position;

        // ������ controller�� ���� controller ������ ���� ���͸� ����
        Vector3 guitarDirectionVector = leftController.transform.position - rightController.transform.position;
        if (guitarDirectionVector.magnitude == 0) return;

        Quaternion guitarNeckDirection = Quaternion.LookRotation(guitarDirectionVector);


        // ������ controller�� x�� ȸ������ ��Ÿ �ٵ��� x�� ȸ�������� ���
        float guitarBodyRotationX = -rightController.transform.rotation.eulerAngles.z - 60f;

        // ������ ���� guitarNeckDirection ��������  guitar�� �ٶ󺸰� ����
        gameObject.transform.rotation = guitarNeckDirection;
        // ��Ÿ�� �ٵ� guitarNeckDirection ������ �ٶ󺸰� �ǹǷ� x������ 90�� ȸ�����Ѽ� ��Ÿ ���� guitaNeckDirection�� �ٶ󺸰� ����
        // ��Ÿ �ٵ��� x�� ȸ�� ���� ������ controller�� x�� ȸ�������� ����
        gameObject.transform.Rotate(new Vector3(guitarBodyRotationX, 90f, 0f));
    }


    /// <summary>
    /// �÷��̾ �ڵ带 ��ȭ���״��� Ȯ���ϴ� �Լ�
    /// �÷��̾ hit key�� ���� ��(��Ÿ ���� ƨ�� ��)���� ����Ǹ� hit key�� ������ ������ ���� ���Ͽ�
    /// ����, ������ controller ������ �Ÿ��� sensitivity ���� ũ�� ���ϸ� �ڵ带 ��ȭ��Ų������ �ν��ϰ� �׷��� ������ �ڵ带 ��ȭ��Ű�� ���� ������ �ν��Ѵ�.
    /// </summary>
    /// <returns> true: �ڵ尡 ���� / false: �ڵ尡 ������ ���� </returns>
    public bool CheckChordChange()
    {
        // �� controller ������ �Ÿ��� ���
        float currentDistanceBetweenControllers = (rightController.transform.position - leftController.transform.position).magnitude;
        // ���� hit key�� ������ �� controller ������ �Ÿ��� ���� controller ������ �Ÿ��� ���� ���
        float distanceDifference = Mathf.Abs(currentDistanceBetweenControllers - previousDistanceBetweenControllers);
        // ������ �˻縦 ���� previousDistanceBetweenControllers�� currentDistanceBetweenControllers�� ����
        previousDistanceBetweenControllers = currentDistanceBetweenControllers;

        //Debug.Log("distance: " + distanceDifference);

        // �Ÿ� ���̰� sensitivity���� ũ�� true�� return �׷��� ������ false�� return
        if (distanceDifference > sensitivity) return true;
        else return false;
    }
}
