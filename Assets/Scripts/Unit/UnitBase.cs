using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class UnitBase : MonoBehaviour
{
    #region Variables
    [Header("적용할 유닛 정보")]
    [SerializeField] protected UnitStats stat;
    [Header("유닛 스프라이트/이미지")]
    [SerializeField] protected Sprite unitSprite;
    [Header("유닛 히트박스")]
    [SerializeField] protected BoxCollider2D hitBox;
    #endregion

    #region Properties
    public UnitStats Stat => stat;
    public Sprite UnitSprite => unitSprite;
    public BoxCollider2D HitBox => hitBox;
    #endregion

    #region Initialization
    protected void Awake() {
        //null check
        if(stat == null) {
            Debug.LogWarning("UnitStats Missing!");
        }
        if (unitSprite == null) {
            Debug.LogWarning("UnitSprite Missing!");
        }
        if (hitBox == null) {
            Debug.LogWarning("UnitHitBox Missing!");
        }
    }

    protected void Start() {
        //초기화
        stat.currHealth = stat.maxHealth;
        //생산이 완료되기 전까지는 비활성화
        gameObject.SetActive(false);
    }
    #endregion

    #region StateMachine

    protected enum UnitState {
        Idle,
        Move,
        Attack,
        Dead
    }

    protected float timer = 0f;
    protected float attackTimer = 0f;   
    protected UnitState currentState = UnitState.Idle;

    protected void Update() {
        timer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        if (timer > stat.actionTimer) {
            timer = 0f;
            switch(currentState) {
                case UnitState.Idle:
                    //Idle 상태 : 다음 행동을 결정하는 단계
                    WhenIdle();
                    break;
                case UnitState.Move:
                    //Move 상태 : 적이 감지되면 Attack 상태로 전환
                    WhenMove();
                    break;
                case UnitState.Attack:
                    //Attack 상태 : 공격 후 Idle 상태로 전환
                    WhenAttack();
                    currentState = UnitState.Idle;
                    break;
                case UnitState.Dead:
                    //Dead 상태 : 사망 처리 후 유닛 제거
                    break;
            }
        }
    }

    protected void WhenIdle() {
        //대기 상태에서는 기본적으로 이동을 시도
        currentState = UnitState.Move;
    }
    protected void WhenMove() {
        //이동 상태에서 할 일
        //1. 사거리 내에 적이 존재하면 공격/유휴 상태로 전환
        //전체 유닛 + 건물 오브젝트는 싱글톤에서 리스트로 관리됨
        if(CheckEnemy() != null) {
            //2. 공격이 가능한 상태면 공격 상태로 전환
            if(attackTimer >= stat.attackCooldown) {
                currentState = UnitState.Attack;
            } //2-2. 공격 쿨타임이 다 안돌았으면 유휴 상태로 전환
            else currentState = UnitState.Idle;

        } else {
            //3. 적이 없으면 이동 지속
            //var target = singleton
        }
    } 

    protected UnitBase CheckEnemy() {
        //공격 사거리 내에 적이 존재하는 지 확인
        //TODO : 싱글톤 내에 유닛을 담아놓는 리스트를 만들고, 유닛 소유주에 따라 다른 리스트를 참조 해야 함
        /*
          var unitList = (stat.owner == UnitOwner.Player ?
                          singleton.Instance.enemyUnitList :
                          singleton.Instacne.playerUnitList);
           var target = unitList.Last();
           float dist = Vector2.Distance(transform.position, target.transform.position);
        return dist <= stat.attackRange ? target.GetComponent<UnitBase>() : null;
         */
        return null;
    }
    protected void WhenAttack() {
        attackTimer = 0f;
        //공격 실행
        //TODO : 공격 애니메이션 재생
        //var target = CheckEnemy();
        //if(target != null) target.TakeDamage(stat.attackDamage);
    }

    public void TakeDamage(float damage) {
        stat.currHealth -= damage;
        if(stat.currHealth <= 0) {
            stat.currHealth = 0;
            currentState = UnitState.Dead;
            WhenDead();
        }
    }
    protected void WhenDead() {
        //TODO : 사망 애니메이션 재생
        //애니메이션 재생 후 오브젝트 제거
        Destroy(gameObject, 1f);
    }

    #endregion
}
