
using UnityEngine;

public class BackNavigationButton : MonoBehaviour {

    public void Back() {
        Managers.GetManager<BackNavigation>().CallBack();
    }

}
