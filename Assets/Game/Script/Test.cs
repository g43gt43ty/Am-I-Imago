using UnityEngine;

public class Test : MonoBehaviour
{
    public GolemController golemController;

    public void ChangeGoleForm()
    {
        golemController.ChangeForm(GolemController.GolemForm.Butterfly);
    }
}
