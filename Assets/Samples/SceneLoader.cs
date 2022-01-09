using MKoterba.Synchronization;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Executable
{
    public override void Execute()
    {
        SceneManager.LoadScene("Scene_B");
    }

    public override void Execute(TaskCompletionSource<bool> completion)
    {
        throw new System.NotImplementedException();
    }
    public void LoadScene()
    {
        SceneManager.LoadScene("Scene_B");
    }
}
