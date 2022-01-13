using MKoterba.Synchronization;
using System.Threading.Tasks;
using UnityEngine;

public class SoundPlayer : Executable
{
    public AudioSource audioSource;
    public AudioClip clip;

    private bool playing;
    private TaskCompletionSource<bool> completion;

    private void Update()
    {
        if (playing)
        {
            if (!audioSource.isPlaying)
            {
                playing = false;
                completion?.SetResult(true);
            }
        }
    }

    public override void Execute()
    {
        throw new System.NotImplementedException();
    }

    public override void Execute(TaskCompletionSource<bool> completion)
    {
        this.completion = completion;
        Play();
    }

    public void Play()
    {
        audioSource.clip = clip;
        audioSource.Play();
        playing = true;
    }
}
