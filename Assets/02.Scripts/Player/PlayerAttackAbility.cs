using UnityEngine;
using Photon.Pun;

public class PlayerAttackAbility : PlayerAbility
{
    [SerializeField] private int _attackAnimationIndex = 3;
    [SerializeField] private bool _sequentialAttackAnimation = true;
    [SerializeField] private bool _randomAttackAnimation = false;

    private float _attackTimer = 0f;
    private int _attackAnimationNumber = 0;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;

        _attackTimer += Time.deltaTime;
        if (_attackTimer < _owner.Stat.AttackSpeed) return;

        if (!_sequentialAttackAnimation && !_randomAttackAnimation)
        {
            _randomAttackAnimation = true;
        }

        // 트랜스폼(위치, 회전, 스케일), 애니메이션(float 파라미터)와 같이 상시로 동기화가 필요한 데이터: IPunObservable (OnPhotonSerializeView)
        // 애니메이션 트리거처럼 간헐적으로 특정한 이벤트가 발생했을 때만 변화하는 데이터 동기화는 이벤트 동기화: RPC
        // RPC: Remote Procedure Call (원격 함수 호출): 물리적으로 떨어져 있는 다른 디바이스의 내 포톤뷰 함수를 호출하는 기능이다.
        // RPC로 호출할 함수는 반드시 [PunRPC] 어트리뷰트를 함수 앞에 명시해줘야 한다.

        // RPC 메서드 호출 방식: PhotonView.RPC(nameof(메서드 이름), RpcTarget.대상, 매개변수)
        // 다른 컴퓨터에 있는 내 플레이어 오브젝트의 메서드를 실행한다.

        if (Input.GetMouseButtonDown(0) && _sequentialAttackAnimation)
        {
            _owner.PhotonView.RPC(nameof(PlayAttackAnimation), RpcTarget.All, _attackAnimationNumber);
            _owner.Stat.Stamina -= _owner.Stat.AttackNeedStamina;
            if (_attackAnimationNumber >= _attackAnimationIndex)
            {
                _attackAnimationNumber = 0;
            }
            _attackAnimationNumber += 1;
            _attackTimer = 0f;
        }

        if (Input.GetMouseButtonDown(0) && _randomAttackAnimation)
        {
            _attackAnimationNumber = Random.Range(0, _attackAnimationIndex);
            _owner.PhotonView.RPC(nameof(PlayAttackAnimation), RpcTarget.All, _attackAnimationNumber);
            _owner.Stat.Stamina -= _owner.Stat.AttackNeedStamina;
            _attackTimer = 0f;
        }

        [PunRPC]
        void PlayAttackAnimation(int animationNumber)
        {
            _animator.SetTrigger($"Attack{_attackAnimationNumber}");
        }
    }
}
