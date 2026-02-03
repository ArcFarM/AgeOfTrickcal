using UnityEngine;
//UnitBase.cs : 각 유닛들의 능력치 속성을 정리하는 ScriptableObject
[CreateAssetMenu(fileName = "NewUnitData", menuName = "NewScriptableObject")]
public class UnitStats : ScriptableObject
{
    [Header("기본 정보")]
    public string unitID;
    public enum UnitOwner
    {
        Player,
        Enemy
    }
    public UnitOwner owner;
    public float actionTimer; //초 단위, idle 상태에서 다음 행동 시작까지 소요될 시간

    [Header("능력치")]
    public float maxHealth;
    public float currHealth;
    public float attackDamage;
    public float attackCooldown; // 초 단위
    public float moveSpeed;
    public float attackRange;

    [Header("생산 정보")]
    public int productionCost;
    public float productionTime; // 초 단위

    [Header("보상")]
    public int killScore;
    public int killReward;
}
