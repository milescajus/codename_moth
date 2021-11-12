using UnityEngine;
using System.Collections;

public class SurveyButton : MonoBehaviour
{
    public void OpenSurvey()
    {
        Application.OpenURL("https://forms.gle/azmkaVzwbUDrXhiw7");
    }
}
