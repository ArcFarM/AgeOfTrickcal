using UnityEngine;
using Singleton;
using System.Collections;
public class Building : UnitBase
{
    protected override void Start() {
        stat.currHealth = stat.maxHealth;
        //초기화 시 건물(본진)은 반드시 리스트 0번에 위치해야 함
        var unitList = (stat.owner == UnitStats.UnitOwner.Player ?
                GameManager.Instance.playerUnitList :
                GameManager.Instance.enemyUnitList
            );
        unitList.Insert(0, this);
    }

    //유닛 생산 명령 수행
    [SerializeField] Transform spawnPoint;
    public void UnitBuild(string id, UnitStats.UnitOwner owner) {
        var unitList = GameManager.Instance.GetPlayerList(owner);
        //유닛 생산 시간 동안 대기
        GameObject unit = GameManager.Instance.unitDict[id];
        float buildTime = unit.GetComponent<UnitBase>().Stat.productionTime;
        StartCoroutine(WaitForTrainTime(buildTime));
        GameObject returnUnit = Instantiate(
            unit,
            spawnPoint.position,
            Quaternion.identity
        );
    }

    IEnumerator WaitForTrainTime(float buildTime) {
        float timeCounter = 0f;
        while (timeCounter < buildTime) {
            timeCounter += Time.deltaTime;
            yield return null;
            //TODO : 유닛 생산 진행도에 맞춰서 생산 UI 슬라이더 조정하기
        }
    }
}
