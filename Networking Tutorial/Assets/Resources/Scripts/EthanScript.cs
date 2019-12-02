using System;
using Photon.Pun;
using UnityEngine;

namespace Resources.Scripts
{
    public class EthanScript : MonoBehaviourPun
    {
        private Animator _mAnim;
        private int _jumpHash;
        private int _fallHash;
        private int _crouchHash;
        private int _punchHash;
        private int _kickHash;
        private string _jumpState;
        private string _fallState;
        private string _punchState;
        private string _kickState;
        private readonly Vector3 _leftArmWorld = new Vector3(-15, -5, -20);
        private readonly Vector3 _rightArmWorld = new Vector3(50,10,20);

        // Start is called before the first frame update
        void Start()
        {
            SetUpStatesAndParameters();
            _mAnim = GetComponent<Animator>();
            if (!_mAnim)
            {
                Debug.LogError("Missing Animator Component", this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            float move = Input.GetAxis("Vertical");
            float rotation = Input.GetAxis("Horizontal") * 10;
            _mAnim.SetFloat("VelocityZ", move);
            _mAnim.SetFloat("VelocityX", rotation);
        
            for(int i = 0; i < _mAnim.layerCount; i++) ResetBools(i);

            if (Input.GetKeyDown(KeyCode.Space)) _mAnim.SetBool(_jumpHash, true);

            if (Input.GetKeyDown(KeyCode.K)) _mAnim.SetBool(_kickHash, true);

            if (Input.GetKeyDown(KeyCode.Z)) _mAnim.SetBool(_crouchHash, !_mAnim.GetBool(_crouchHash));

            if (Input.GetKeyDown(KeyCode.P)) _mAnim.SetBool(_punchHash, true);
        }

        private void LateUpdate()
        {
            if (Input.GetKey(KeyCode.Y) && photonView.IsMine) BadDab();
        }

        private Vector3 WorldToLocal(Vector3 worldPosition) => worldPosition.x * transform.right + worldPosition.y * transform.up + worldPosition.z * transform.forward;

        private void BadDab()
        {
            _mAnim.GetBoneTransform(HumanBodyBones.RightShoulder).LookAt(WorldToLocal(_rightArmWorld));
            _mAnim.GetBoneTransform(HumanBodyBones.LeftLowerArm).LookAt(WorldToLocal(_leftArmWorld));

        }

        private void SetUpStatesAndParameters()
        {
            StateDriver.Initialise();
            ParameterDriver.Initialise();
        
            _jumpState = StateDriver.GetState(StateDriver.States.Jump);
            _fallState = StateDriver.GetState(StateDriver.States.Fall);
            _punchState = StateDriver.GetState(StateDriver.States.Punch);
            _kickState = StateDriver.GetState(StateDriver.States.Kick);
            _jumpHash = ParameterDriver.GetParameterHash(ParameterDriver.Parameters.Jump);
            _fallHash = ParameterDriver.GetParameterHash(ParameterDriver.Parameters.Fall);
            _crouchHash = ParameterDriver.GetParameterHash(ParameterDriver.Parameters.Crouch);
            _punchHash = ParameterDriver.GetParameterHash(ParameterDriver.Parameters.Punch);
            _kickHash = ParameterDriver.GetParameterHash(ParameterDriver.Parameters.Kick);
        }

        private AnimatorStateInfo GetCurrentStateHash(int layer) => _mAnim.GetCurrentAnimatorStateInfo(layer);

        private void ResetBools(int layer)
        {
            AnimatorStateInfo currentState = GetCurrentStateHash(layer);
            switch (layer)
            {
                case 0:
                    ResetBool(currentState, _jumpHash, _jumpState);
                    ResetBool(currentState, _fallHash, _fallState);
                    break;
                case 1:
                    ResetBool(currentState, _punchHash, _punchState);
                    ResetBool(currentState, _kickHash, _kickState);
                    break;
            }
        }

        private void ResetBool(AnimatorStateInfo currentState, int hash, String state)
        {
            if (!currentState.IsName(state) && _mAnim.GetBool(hash)) _mAnim.SetBool(hash, false);
        }
    }
}
