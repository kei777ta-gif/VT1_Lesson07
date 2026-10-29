using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;//基本移動速度

    [SerializeField] private CameraController playeraCamera;

    [SerializeField] private PlayerInput playerInput;

    private Vector3 Velocity;//移動ベクトル

    private Vector3 inputMoveVector;//移動用の入力値

    void Start()
    {
        MoveInputEnable();//移動入力の有効化
    }
    void Update()
    {
        Movement();//移動処理
    }

    public void Movement()
    {
        //入力値を実際の移動平面に変換
        var inputAxis=new Vector3(inputMoveVector.x,0,inputMoveVector.y);

        //カメラの向いている方向を移動ベクトルとする
        //カメラの正面へベクトルを取得
        var cameraFoward = playeraCamera.transform.forward;
        //入力値と合わせる
        var moveVector = (inputAxis.x * playeraCamera.transform.right) + (inputAxis.z * cameraFoward);
        moveVector.y = 0;

        //(仮)入力をそのまま移動ベクトルへ
        Velocity = moveVector.normalized;

        Vector3 finalVelocity=moveSpeed*Velocity*Time.deltaTime;
        transform.Translate(finalVelocity, Space.World);
    }

    public void MoveInputEnable()
    {
        if (playerInput == null)
        {
            Debug.Log("PLayerInputが設定されていません");
                return;
        }

        //移動入力の登録
        playerInput.actions["Move"].performed += OnMoveInput;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        var value= context.ReadValue<Vector2>();
        inputMoveVector = value;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context) 
    {
        inputMoveVector = Vector3.zero;//変数の値をリセット
    }
}
