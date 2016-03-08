using UnityEngine;
using System.Collections;

public class Carema : MonoBehaviour {

   // private playermovement playermove;
    //观察目标
    private Transform Target;
    //观察距离
    public float Distance = 5F;
    //旋转速度
    private float SpeedX = 10F;
    private float SpeedY = 10F;
    //角度限制
    private float MinLimitY = 1;
    private float MaxLimitY = 75;


    //旋转角度
    private float mX = 0.0F;
    private float mY = 0.0F;

    //鼠标缩放距离最值
    private float MaxDistance = 10;
    private float MinDistance = 1.5F;
    //鼠标缩放速率
    private float ZoomSpeed = 2F;

    //是否启用差值
    public bool isNeedDamping = true;
    //速度
    public float Damping = 2.5F;
    public Vector3 offset;

    private bool CamButtonDown = false;

    void Start()
    {
        //初始化旋转角度
        //		mX=transform.eulerAngles.x;
        //		mY=transform.eulerAngles.y;
    

       
       // playermove = new playermovement();
        Target = GameObject.FindGameObjectWithTag("PlayerN").transform;
        if (Target == null)
        {
            Debug.LogWarning("Don't have a target for Camare");
        }
        else {
            transform.rotation = Quaternion.identity;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CamButtonDown = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            CamButtonDown = false;
        }

    }


    void LateUpdate()
    {


        //鼠标右键旋转
        if (Target != null && Input.GetMouseButton(1))
        {


            //获取鼠标输入
            mX += Input.GetAxis("Mouse X") * SpeedX;
            mY -= Input.GetAxis("Mouse Y") * SpeedY;
            //范围限制
            mY = Mathf.Clamp(mY, MinLimitY, MaxLimitY);
            mX = ClampAngle(mX);


        }

        //鼠标滚轮缩放

        Distance -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        Distance = Mathf.Clamp(Distance, MinDistance, MaxDistance);

        //重新计算位置和角度
        Quaternion mRotation = Quaternion.Euler(mY, mX, 0);
        Vector3 mPosition = mRotation * new Vector3(0.0F, 0.0F, -Distance) + Target.position + offset;

        //设置相机的角度和位置
        if (isNeedDamping)
        {
            //球形插值
            transform.rotation = Quaternion.Lerp(transform.rotation, mRotation, Time.deltaTime * Damping);
            //线性插值
            transform.position = Vector3.Lerp(transform.position, mPosition, Time.deltaTime * Damping);
        }
        else
        {
            transform.rotation = mRotation;
            transform.position = mPosition;
        }
        //将玩家转到和相机对应的位置上
        if (Target.GetComponent<CharacterController>().velocity.magnitude>0.5f)
        {
            Target.eulerAngles = new Vector3(0, mX, 0);
        }
    }

    private float ClampAngle(float angle)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return angle;
    }
	
}
