using UnityEngine;
using UnityEngine.AI;

namespace AGS.Graber.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class GrabPlayerController : MonoBehaviour
    {
       // [Inject] AppModel _model;

        private NavMeshAgent _agent;
        private bool _touch;

        private Vector2 force;
        private Vector2 firstPressPos;

        [SerializeField] private float _sensetivity = 0f;
        [SerializeField] GrabPlayerCharacter _character;

        private Vector3 _mouseForce;
        private Vector3 _mouseFirstPressPos;
        private Vector3 _mouseMultiplyer;

        private void Awake()
        {
            //Todo do better linking
            _agent = GetComponent<NavMeshAgent>();
        }

        void FixedUpdate()
        {
           // if (!_model.IsStartGame.Value || _model.IsPauseGame.Value) return;

            ControlHero();

            if (_touch)
            {

#if UNITY_EDITOR

                if (!Input.anyKey)
                {
                   _character.Interacting = false;
                    _touch = false;
                }
                Vector3 currentVel = new Vector3(-_mouseForce.x, 0, -_mouseForce.y);
                _agent.velocity = currentVel * _character.CharacterSpeed;
                _agent.transform.forward = currentVel.normalized;

#elif UNITY_IOS || UNITY_ANDROID

                if (Input.touchCount == 0)
                {
                    _touch = false;
                     _character.Interacting = false;
                }

		        Vector3 currentVel = new Vector3(-force.x, 0, -force.y);
		        _agent.velocity = currentVel * _character.CharacterSpeed;
		        _agent.transform.forward = currentVel.normalized;
#endif
            }
            else
            {
                _agent.velocity = Vector3.zero;
            }
        }

        void ControlHero()
        {
            if (Input.touchCount > 0)
            {
                Touch input = Input.GetTouch(0);
                switch (input.phase)
                {
                    case TouchPhase.Began:
                        _touch = false;
                        force = Vector2.zero;
                        firstPressPos = input.position;
                        _character.Interacting = true;
                        break;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        var tiuchvel = Vector2.zero;
                        if ((firstPressPos - input.position).magnitude >= 50f)
                        {
                            firstPressPos = Vector2.SmoothDamp(firstPressPos, Vector2.Lerp(input.position, firstPressPos, 0.1f), ref tiuchvel, 0.10f);
                        }
                        Vector2 zeroTouchPos = firstPressPos - input.position;
                        Vector2 normZeroTouch = Vector2.Lerp((firstPressPos - (input.position)).normalized, zeroTouchPos.normalized, Time.deltaTime * 1f);
                        if (zeroTouchPos.magnitude >= 20f)
                        {
                            force = Vector2.Lerp(force, normZeroTouch * _sensetivity, Time.deltaTime * 5f);
                            _touch = true;
                        }
                        _character.Interacting = true;
                        break;
                    case TouchPhase.Canceled:
                    case TouchPhase.Ended:
                        _touch = false;
                        force = Vector2.zero;
                        _character.Interacting = false;
                        break;
                }
            }

#if UNITY_EDITOR

            if (Input.GetMouseButtonDown(0))
            {
                _character.Interacting = true;
                _touch = false;
                _mouseForce = Vector3.zero;
                _mouseFirstPressPos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                _character.Interacting = true;
                var tiuchvel = Vector3.zero;

                if ((_mouseFirstPressPos - Input.mousePosition).magnitude >= 50f)
                {
                    _mouseFirstPressPos = Vector3.SmoothDamp(_mouseFirstPressPos, Vector2.Lerp(Input.mousePosition, _mouseFirstPressPos, 0.1f), ref tiuchvel, 0.10f);
                }

                Vector3 zeroTouchPos = _mouseFirstPressPos - Input.mousePosition;
                Vector3 normZeroTouch = Vector3.Lerp((_mouseFirstPressPos - (Input.mousePosition)).normalized, zeroTouchPos.normalized, Time.deltaTime * 1f);
                if (zeroTouchPos.magnitude >= 20f)
                {
                    _mouseForce = Vector3.Lerp(_mouseForce, normZeroTouch * _sensetivity, Time.deltaTime * 5f);
                    _touch = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _character.Interacting = false;
                _touch = false;
                _mouseForce = Vector2.zero;

            }
#endif
        }
    }
}