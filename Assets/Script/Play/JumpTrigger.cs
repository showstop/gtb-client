using UnityEngine;
using UnityEngine.Networking;

public class JumpTrigger : MonoBehaviour 
{
    [SerializeField]
    private float _checkSpeed;

    void OnTriggerEnter(Collider aCol)
    {
        CarController target = aCol.gameObject.GetComponentInParent<CarController>();
        if (null == target)
        {
            return;
        }

        if (_checkSpeed > target._speed)
        {
            return;
        }

        float startVelocity = Mathf.Pow(target._speed - _checkSpeed, 0.125f) * Constants.JUMP_START_VELOCITY;
        if (target.isLocalPlayer)
        {
            YPLog.Log("=== [jump trigger], localPlayer!!!!");

            target.CmdStartJump(false, startVelocity);
        }
        else if (target._isAI && NetworkServer.active)
        {
            target.StartJump(false, startVelocity);
            target.RpcStartJump(false, startVelocity);
        }
    }
    
    public static void Create()
    {
        GameObject go = new GameObject(Constants.JUMP_Trigger, typeof(JumpTrigger));
        
        BoxCollider collider = go.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.center = new Vector3(0f, 0.5f, 0f);
        collider.size = new Vector3(2.5f, 1f, 0.01f);

        SplineWalkerDistance con = go.AddComponent<SplineWalkerDistance>();        
    }
}
