using UnityEngine;
using System.Collections.Generic;

namespace Singleton {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        //리스트 바로 접근하기 위해 (코드 축약 목적)
        public List<UnitBase> GetEnemyList(UnitStats.UnitOwner owner) {
            return (owner == UnitStats.UnitOwner.Player ?
                playerUnitList :
                enemyUnitList
            );
        }
        public List<UnitBase> GetPlayerList(UnitStats.UnitOwner owner) {
            return (owner == UnitStats.UnitOwner.Player ?
                enemyUnitList :
                playerUnitList
            );
        }

        //각 유저의 유닛 리스트
        public List<UnitBase> playerUnitList = new List<UnitBase>();
        public List<UnitBase> enemyUnitList = new List<UnitBase>();

        //생산 가능 유닛 리스트
        public Dictionary<string, GameObject> unitDict = new Dictionary<string, GameObject>();

    }

}
