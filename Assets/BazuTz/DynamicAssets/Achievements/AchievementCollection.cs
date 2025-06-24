using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievementCollection", menuName = "Autosport/AchievementCollection", order = 100)]
public class AchievementCollection : ScriptableObject {

    [field: SerializeField] public List<Achievement> Achievements { get; private set; }

}
