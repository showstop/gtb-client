using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public enum CameraPlayMode
{
    None,
    JoinCameraMoveComplete,         ///< 조인 카메라 이동 완료
    PlayGame
}

public class TRCamera : MonoBehaviour 
{
    private enum UpsetState
    {
        None,
        Upset,
        Recovery,
    }

    [SerializeField]
    private float m_SynchronizeCarLookAtDistance = 1.0f;

    [SerializeField]
    public float m_cameraDistance = 2.5f;
    [SerializeField]
    public float m_cameraHeight = 1.5f;
    [SerializeField]
    public float m_cameraAngle = 30.0f;
    [SerializeField]
    public float m_cameraRight = 1.0f;
    [SerializeField]
    private float m_cameraMoveSmoothLag = 1.0f;
    [SerializeField]
    private float m_cameraRotateSmoothLag = 2.0f;
    [SerializeField]
    private float m_LookAtRotationOffset = 0.217f;

    [SerializeField]
    private float m_shakeIntensity = 0.1f;
    [SerializeField]
    private float m_shakeDuration = 0f;

    public Transform            m_SynchronizeCar;                                       //< 동기화할 차
    private SplineWalkerCon     m_SynchronizeCarSW;
    private Transform m_SynchronizeCarCameraDummy;

    private Transform           m_LookAtObject;
    private Transform           m_TargetObject;
    private Transform           m_CameraObject;
    public Camera               m_MainCamera;

    private Vector3             m_cameraPos = Vector3.zero;
    private Vector3             m_lookAtPos = Vector3.zero;

    private CameraPlayMode      m_cameraPlayMode                = CameraPlayMode.None;
    public CameraPlayMode       PlayMode                        { get { return m_cameraPlayMode; } }

    private float _upsetZ = 0f;
    private float _upsetRotationSpeed = 0f;
    private UpsetState _upsetState = UpsetState.None;

    [SerializeField]
    private Slider _heightSlider;

    [SerializeField]
    private Slider _distanceSlider;

    [SerializeField]
    private Text _heightText;

    [SerializeField]
    private Text _distanceText;
    

	// Use this for initialization
	void Start () 
    {
        Transform[] objects = transform.GetComponentsInChildren<Transform>();
        for( int i = 0 ; i < objects.Length ; i++ )
        {
            if (objects[i].name == "LookAtObject")
            {
                m_LookAtObject = objects[i];
            }

            if (objects[i].name == "TargetObject")
            {
                m_TargetObject = objects[i];
            }

            if ( objects[i].name == "CameraObject")
            {
                m_CameraObject = objects[i];
            }
        }

        if (m_MainCamera == null && Camera.main)
            m_MainCamera = Camera.main;

        m_cameraPos = m_MainCamera.transform.position;
        m_lookAtPos = m_MainCamera.transform.position;

        /*
        _heightSlider.minValue = 0f;
        _heightSlider.maxValue = 20f;
        _heightSlider.value = m_cameraHeight;        

        _distanceSlider.minValue = 0f;
        _distanceSlider.maxValue = 20f;
        _distanceSlider.value = m_cameraDistance;

        ChangeHeightSlider();
        ChangeDistanceSlider();
        */
	}

    public void ChangeHeightSlider()
    {
        m_cameraHeight = _heightSlider.value;
        _heightText.text = m_cameraHeight.ToString();
    }

    public void ChangeDistanceSlider()
    {
        m_cameraDistance = _distanceSlider.value;
        _distanceText.text = m_cameraDistance.ToString();
    }

    void LateUpdate()
    {
        if (null == m_SynchronizeCarSW && null != m_SynchronizeCar)
        {
            m_SynchronizeCarSW = m_SynchronizeCar.GetComponentInChildren<SplineWalkerCon>();
            m_SynchronizeCarCameraDummy = m_SynchronizeCar.Find("CameraDummy");
        }

        if (null == m_SynchronizeCarSW)
            return;

        if (m_cameraPlayMode == CameraPlayMode.PlayGame)
        {
            //switch (TRGlobalData.instance.CameraIndex)
            int cameraIndex = 0;
            switch (cameraIndex)
            {
                case 0:
                    {
                        m_SynchronizeCarLookAtDistance = 10.0f;
                        //m_cameraDistance = 2.5f;
                        //m_cameraHeight = 1.5f;
                        m_cameraAngle = 0.0f;
                        m_cameraRight = 3.0f;
                        m_cameraMoveSmoothLag = 1.5f;
                        m_cameraRotateSmoothLag = 4.0f;
                        m_LookAtRotationOffset = 0.217f;

                        UpdatePlayGameCamera();
                        ApplyCamera();
                    }break;
                case 1:
                    {
                        //m_cameraDistance = 2.5f;
                        //m_cameraHeight = 1.5f;
                        m_cameraAngle = 30.0f;
                        m_cameraMoveSmoothLag = 10.0f;
                        m_cameraRotateSmoothLag = 10.0f;

                        UpdatePlayGameCamera2();
                    } break;
                case 2:
                    {
                        //m_cameraDistance = 4.0f;
                        //m_cameraHeight = 3.0f;
                        m_cameraAngle = 35.0f;
                        m_cameraMoveSmoothLag = 10.0f;
                        m_cameraRotateSmoothLag = 10.0f;

                        UpdatePlayGameCamera3();
                    } break;
            }
        }
    }

    private float GetUpsetZ()
    {
        switch (_upsetState)
        {
            case UpsetState.None:
                break;

            case UpsetState.Upset:
                _upsetZ += Time.deltaTime * _upsetRotationSpeed;
                if (180f < _upsetZ)
                {
                    _upsetZ = 180f;
                    _upsetState = UpsetState.None;
                }

                break;

            case UpsetState.Recovery:
                _upsetZ -= Time.deltaTime * _upsetRotationSpeed;
                if (0f > _upsetZ)
                {
                    _upsetZ = 0f;
                    _upsetState = UpsetState.None;
                }

                break;
        }
        
        return _upsetZ;        
    }

    internal void Upset(bool aStart, float aRotationSpeed)
    {
        if (aStart)
        {
            _upsetState = UpsetState.Upset;
        }
        else
        {
            _upsetState = UpsetState.Recovery;
        }

        _upsetRotationSpeed = aRotationSpeed;
    }

    /** 카메라 적용 */
    private void ApplyCamera()
    {
        m_MainCamera.transform.position = Vector3.Lerp( m_MainCamera.transform.position, m_cameraPos, Time.deltaTime * m_cameraMoveSmoothLag);
        
        Vector3 relativePos = m_lookAtPos - m_MainCamera.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        Vector3 rot = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, GetUpsetZ());

        rotation = Quaternion.Euler(rot);
        m_MainCamera.transform.rotation = Quaternion.Lerp(m_MainCamera.transform.rotation, rotation, Time.deltaTime * m_cameraRotateSmoothLag);
    }

    IEnumerator DelayJoinGameCamera(CarController aTargetCar)
    {
        yield return new WaitForSeconds(1f);

        Vector3 vPreCameraPosition = m_MainCamera.transform.position;
        Quaternion vPreCameraQuatrnion = m_MainCamera.transform.rotation;
            
        transform.position = m_SynchronizeCar.position;
        transform.rotation = m_SynchronizeCar.rotation;

        m_MainCamera.transform.position = vPreCameraPosition;
        m_MainCamera.transform.rotation = vPreCameraQuatrnion;

        //TRCarController targetCar = TRStatic.GetGameManager().GetMyCarController();
        //if (null != targetCar)
        //{
        //    Vector3 vBack = targetCar.transform.TransformDirection(Vector3.back) + new Vector3(0.0f, 1.0f, 0.0f);
        //    vBack = Vector3.Normalize(vBack);
        //    m_cameraPos = targetCar.CarTransform.position + vBack;
        //    m_lookAtPos = targetCar.CarTransform.position;
        //}
        if (null != aTargetCar)
        {
            Vector3 vBack = aTargetCar.transform.TransformDirection(Vector3.back) + new Vector3(0.0f, 1.0f, 0.0f);
            vBack = Vector3.Normalize(vBack);
            m_cameraPos = aTargetCar.CarTransform.position + vBack;
            m_lookAtPos = aTargetCar.CarTransform.position;
        }

        float currentTime = 0.0f;
        while (true)
        {
            currentTime += Time.deltaTime;

            ApplyCamera();

            float distance = Mathf.Abs( Vector3.Distance( m_cameraPos, m_MainCamera.transform.position ) );
            if ( distance < 0.05f )
                break;

            if (currentTime > 10.0f)
                break;

            yield return null;
        }

        //if (Network.NetworkClient.active)
        //{
        //    TRGameManager gm = TRStatic.GetGameManager();
        //    if (null != gm)
        //    {
        //        gm.GetComponent<NetworkView>().RPC("EndCameraMoving", RPCMode.Server);
        //    }
        //    m_cameraPlayMode = CameraPlayMode.None;
        //}
        //else if (!TRStatic.IsNetworkConnected())
        //{
        //    m_cameraPlayMode = CameraPlayMode.JoinCameraMoveComplete;
        //}

        if (NetworkClient.active)
        {
            aTargetCar.CmdEndJoinCameraMoving();
            m_cameraPlayMode = CameraPlayMode.None;
        }
        else if (!TRStatic.IsNetworkConnected())
        {
            m_cameraPlayMode = CameraPlayMode.JoinCameraMoveComplete;
        }
    }

    
    void UpdatePlayGameCamera()
    {
        transform.position = m_SynchronizeCar.position;
        transform.rotation = m_SynchronizeCar.rotation;
        //Vector3 carLocalPosition = m_SynchronizeCar.GetComponent<TRCarController>().CarTransform.localPosition;
        Vector3 carLocalPosition = m_SynchronizeCar.GetComponent<CarController>().CarTransform.localPosition;

        //
        // Loot At Position
        //
        float lookatLength = m_SynchronizeCarSW.Distance + m_SynchronizeCarLookAtDistance;
        if (lookatLength > m_SynchronizeCarSW.Spline.Length)
            lookatLength = lookatLength - m_SynchronizeCarSW.Spline.Length;
        else if (lookatLength < 0.0f)
            lookatLength = m_SynchronizeCarSW.Spline.Length + lookatLength;
        float lookAtLengthF = m_SynchronizeCarSW.Spline.DistanceToTF(lookatLength);
        m_LookAtObject.position = m_SynchronizeCarSW.Spline.Interpolate(lookAtLengthF);
        m_LookAtObject.rotation = m_SynchronizeCarSW.Spline.GetOrientationFast(lookAtLengthF);
        m_LookAtObject.localPosition += new Vector3( carLocalPosition.x * 0.5f, 0.0f, 0.0f ); 

        //
        // Target Position( Car Position)
        //
        m_TargetObject.position = m_SynchronizeCar.position;
        m_TargetObject.rotation = m_SynchronizeCar.rotation;
        m_TargetObject.localPosition += new Vector3(carLocalPosition.x * 0.5f, carLocalPosition.y, carLocalPosition.z); 
        
        //
        // Camera Position
        //
        float cameraLenght = m_SynchronizeCarSW.Distance - m_cameraDistance;
        if (cameraLenght < 0.0f)
            cameraLenght = m_SynchronizeCarSW.Spline.Length + cameraLenght;
        float cameraLengthF = m_SynchronizeCarSW.Spline.DistanceToTF(cameraLenght);
        m_CameraObject.position = m_SynchronizeCarSW.Spline.Interpolate(cameraLengthF);
        m_CameraObject.rotation = m_SynchronizeCarSW.Spline.GetOrientationFast(cameraLengthF);
        m_CameraObject.localPosition += new Vector3(carLocalPosition.x * 0.5f, 0.0f, 0.0f);
        m_CameraObject.position += m_CameraObject.TransformDirection(Vector3.up) * m_cameraHeight;

        Vector3 dir = m_TargetObject.position - m_LookAtObject.position;
        dir = Vector3.Normalize(dir);

        Vector3 lhs = Vector3.Normalize( m_LookAtObject.position - m_TargetObject.position );
        Vector3 rhs = Vector3.Normalize( m_TargetObject.forward );
        Vector3 cross = Vector3.Cross(lhs, rhs);

        m_CameraObject.position += m_CameraObject.TransformDirection(Vector3.right) * (cross.y * m_cameraRight);

        m_cameraPos = m_CameraObject.position;
        

        //
        // Rotation
        //
        m_lookAtPos = m_TargetObject.position;
        m_lookAtPos += m_TargetObject.rotation * (Vector3.forward * m_LookAtRotationOffset);
    }

    
    private void UpdatePlayGameCamera2()
    {
        m_SynchronizeCarCameraDummy.transform.localPosition = new Vector3(0.0f, m_cameraHeight, -m_cameraDistance);
        m_SynchronizeCarCameraDummy.transform.localRotation = Quaternion.Euler(m_cameraAngle, 0.0f, 0.0f);

        m_MainCamera.transform.position = Vector3.Lerp( m_MainCamera.transform.position,
                                                        m_SynchronizeCarCameraDummy.transform.position,
                                                        Time.deltaTime * m_cameraMoveSmoothLag);

        m_MainCamera.transform.rotation = Quaternion.Lerp( m_MainCamera.transform.rotation, 
                                                           m_SynchronizeCarCameraDummy.transform.rotation,
                                                           Time.deltaTime * m_cameraRotateSmoothLag);
    }

   
    private void UpdatePlayGameCamera3()
    {
        m_SynchronizeCarCameraDummy.transform.localPosition = new Vector3(0.0f, m_cameraHeight, -m_cameraDistance);
        m_SynchronizeCarCameraDummy.transform.localRotation = Quaternion.Euler(m_cameraAngle, 0.0f, 0.0f);

        //m_MainCamera.transform.position = m_SynchronizeCarCameraDummy.transform.position;
        //m_MainCamera.transform.rotation = m_SynchronizeCarCameraDummy.transform.rotation;
        m_MainCamera.transform.position = Vector3.Lerp(m_MainCamera.transform.position,
                                                        m_SynchronizeCarCameraDummy.transform.position,
                                                        Time.deltaTime * m_cameraMoveSmoothLag);

        m_MainCamera.transform.rotation = Quaternion.Lerp(m_MainCamera.transform.rotation,
                                                           m_SynchronizeCarCameraDummy.transform.rotation,
                                                           Time.deltaTime * m_cameraRotateSmoothLag);
    }

    public void PlayJoinGameCamera(CarController aTargetCar)
    {
        PlayBGM();
        StartCoroutine(DelayJoinGameCamera(aTargetCar));
    }

    public void PlayPlayGameCamera()
    {
        m_cameraPlayMode = CameraPlayMode.PlayGame;
    }

    public void PlayCameraShake(float aDuration)
    {
        if (0f < m_shakeDuration)
            return;

        m_shakeDuration = aDuration;
        StartCoroutine("ShakeCamera");
    }

    IEnumerator ShakeCamera()
    {        
        float intensity = m_shakeIntensity;
        float decay = m_shakeIntensity / m_shakeDuration;

        while (intensity > 0f)
        {
            m_MainCamera.transform.localPosition = m_MainCamera.transform.localPosition + Random.insideUnitSphere * intensity;
            m_MainCamera.transform.localRotation = new Quaternion(m_MainCamera.transform.localRotation.x + Random.Range(-intensity, intensity) * .2f,
                                                                   m_MainCamera.transform.localRotation.y + Random.Range(-intensity, intensity) * .2f,
                                                                   m_MainCamera.transform.localRotation.z + Random.Range(-intensity, intensity) * .2f,
                                                                   m_MainCamera.transform.localRotation.w + Random.Range(-intensity, intensity) * .2f);

            intensity -= decay * Time.deltaTime;
            yield return null;
        }

        m_shakeDuration = 0f;
    }


    internal void PlayBGM()
    {
        if (!TRGlobalData.instance.PlayBGM)
        {
            return;
        }

        AudioSource source = m_MainCamera.gameObject.GetComponent<AudioSource>();
        if (null == source)
        {
            return;
        }

        //source.clip = TRStatic.GetGameManager().m_bgmAudioClip;
        //source.loop = true;
        source.Play();
    }

    internal void StopBGM()
    {
        AudioSource source = m_MainCamera.gameObject.GetComponent<AudioSource>();
        if (null == source)
        {
            return;
        }
        
        source.Stop();
    }

#region EVENT_CALLBACK
    public void onCameraMode1()
    {

    }

    public void onCameraMode2()
    {

    }

    public void onCameraMode3()
    {

    }
#endregion

#region DRAW_GIZMOS
    void OnDrawGizmos()
    {
        //if (null == m_SynchronizeCarSW && null != m_SynchronizeCar)
        //{
        //    m_SynchronizeCarSW = m_SynchronizeCar.GetComponentInChildren<SplineWalkerCon>();
        //    m_SynchronizeCarCameraDummy = m_SynchronizeCar.FindChild("CameraDummy");
        //}

        //Gizmos.color = Color.white;

        //m_SynchronizeCarSW.TF = m_SynchronizeCarSW.InitialF;

        //if (null != m_SynchronizeCar)
        //{
        //    TRCarRender carRender = m_SynchronizeCar.GetComponentInChildren<TRCarRender>();
        //    if (null != carRender)
        //    {
        //        Transform transform = carRender.transform;
        //        Vector3 position = transform.position;

        //        Gizmos.color = Color.red;
        //        Matrix4x4 temp = Gizmos.matrix;
        //        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        //        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        //        Gizmos.matrix = temp;

        //        Gizmos.DrawLine(position, transform.TransformDirection(Vector3.back) * m_cameraDistance + position);


                 
        //        Gizmos.color = Color.blue;
        //        Vector3 lookAtPosition;
        //        Quaternion lookAtRotation;
        //        ////
        //        //// Loot At Position
        //        ////
        //        //Debug.Log("m_SynchronizeCarSW.Distance : " + m_SynchronizeCarSW.Distance + m_SynchronizeCarSW.TF);
        //        float lookatLength = m_SynchronizeCarSW.Distance + m_SynchronizeCarLookAtDistance;
        //        if (lookatLength > m_SynchronizeCarSW.Spline.Length)
        //            lookatLength = lookatLength - m_SynchronizeCarSW.Spline.Length;
        //        else if (lookatLength < 0.0f)
        //            lookatLength = m_SynchronizeCarSW.Spline.Length + lookatLength;
        //        float lookAtLengthF = m_SynchronizeCarSW.Spline.DistanceToTF(lookatLength);

        //        lookAtPosition = m_SynchronizeCarSW.Spline.Interpolate(lookAtLengthF);
        //        lookAtRotation = m_SynchronizeCarSW.Spline.GetOrientationFast(lookAtLengthF);
        //        Gizmos.DrawSphere(lookAtPosition, 0.2f);

                
        //        Vector3 a = Vector3.Normalize( lookAtPosition - position );
        //        Vector3 b = Vector3.Normalize( transform.forward );
                
        //        Gizmos.color = Color.yellow;
        //        Gizmos.DrawLine(position, position + (a*100.0f) );
        //        Gizmos.color = Color.red;
        //        Gizmos.DrawLine(position, position + (b * 100.0f));
        //        //float dotValue = Vector3.Dot(lookAtPosition, transform.position);
        //        //float dotValue2 = Vector3.Dot(transform.position, lookAtPosition);
        //        float dotValue = Vector3.Dot(a, b);
        //        float angleValue = Vector3.Angle(a, b);
        //        Vector3 crossValue = Vector3.Cross(a, b);
        //        Debug.Log(string.Format("Dot [{0}], Angle [{1}], Cross [{2}]", dotValue, angleValue,crossValue));

        //        m_cameraRight = crossValue.y *5.0f;
                
        //        Gizmos.color = Color.yellow;
        //        Vector3 cameraPosition;
        //        Quaternion cameraRotation;
        //        float cameraLenght = m_SynchronizeCarSW.Distance - m_cameraDistance;
        //        if (cameraLenght < 0.0f)
        //            cameraLenght = m_SynchronizeCarSW.Spline.Length + cameraLenght;
        //        float cameraLengthF = m_SynchronizeCarSW.Spline.DistanceToTF(cameraLenght);
        //        cameraPosition = m_SynchronizeCarSW.Spline.Interpolate(cameraLengthF);
        //        Gizmos.DrawSphere(cameraPosition, 0.2f);
        //        cameraRotation = m_SynchronizeCarSW.Spline.GetOrientationFast(cameraLengthF);
        //        cameraPosition += transform.TransformDirection( Vector3.up ) * m_cameraHeight;
        //        cameraPosition += transform.TransformDirection(Vector3.right) * m_cameraRight;
        //        Gizmos.DrawSphere(cameraPosition, 0.2f);



        //        m_MainCamera.transform.position = cameraPosition;
        //        m_MainCamera.transform.LookAt(transform.position);

                

        //        Gizmos.color = Color.white;
        //        m_TestPos = Vector3.Normalize(position - lookAtPosition);
        //        Gizmos.DrawLine(lookAtPosition, m_TestPos * 100.0f);
        //    }
            
            //Gizmos.DrawSphere(position, 0.5f);

        //}     

        //if ( null != m_LookAtObject && 
        //     null != m_TargetObject &&
        //     null != m_CameraObject )
        //{
        //    Gizmos.color = Color.red;

        //    Gizmos.DrawCube(m_LookAtObject.position, Vector3.one * 0.2f);
        


        //    Gizmos.color = Color.yellow;

        //    Gizmos.DrawCube(m_TargetObject.position, Vector3.one * 0.2f);
        


        //    Gizmos.color = Color.green;

        //    Gizmos.DrawCube(m_CameraObject.position, Vector3.one * 0.2f);


        //    Gizmos.color = Color.white;
        //    Vector3 dir = m_TargetObject.position - m_LookAtObject.position;
        //    dir = Vector3.Normalize(dir);
        //    Gizmos.DrawLine(m_LookAtObject.position, m_LookAtObject.position + (dir * 100.0f));

        //    Vector3 lhs = Vector3.Normalize(m_LookAtObject.position - m_TargetObject.position);
        //    Vector3 rhs = Vector3.Normalize(m_TargetObject.forward);
            
        //    Vector3 cross = Vector3.Cross(lhs, rhs);

        //    float fAngle = AngleDir(lhs, rhs, m_TargetObject.up);
        //    float fAngle2 = ContAngle(lhs, rhs);
            
        //    Debug.Log("fAngle : " + fAngle + "angle2 : " + fAngle2 );


        //    //m_CameraObject.Rotate(m_TargetObject.up, cross.y);
        //    ////m_cameraRight = cross.y * 5.0f;
        //    //Gizmos.color = Color.gray;

        //    Vector3 test = m_TargetObject.right * (fAngle * 5.0f);
        //    Gizmos.DrawLine(m_CameraObject.position, m_CameraObject.position + test);
        //}

        
    }
#endregion

    public float ContAngle(Vector3 fwd, Vector3 targetDir)
    {
        float angle = Vector3.Angle(fwd, targetDir);

        if (AngleDir(fwd, targetDir, Vector3.up) == -1)
        {
            angle = 360.0f - angle;
            if (angle > 359.9999f)
                angle -= 360.0f;
            return angle;
        }
        else
            return angle;
    }

    public int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0.0)
            return 1;
        else if (dir < 0.0)
            return -1;
        else
            return 0;
    }

}
