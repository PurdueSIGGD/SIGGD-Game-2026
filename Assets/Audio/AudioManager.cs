using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static bool hasTrackAvailable = true;
    static EventInstance currentMusicTrack;
    static EventReference delayMusicTrack;

    public static void PlayOneShot(EventReference audio, Vector3 position = default) => RuntimeManager.PlayOneShot(audio, position);

    public static void PlayTrack(EventReference audio)
    {
        if (currentMusicTrack.isValid())
        {
            currentMusicTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusicTrack.release();
        }
        if (!RuntimeManager.HaveAllBanksLoaded || RuntimeManager.AnySampleDataLoading())
        {
            hasTrackAvailable = false;
            delayMusicTrack = audio;
        } 
        else {
            hasTrackAvailable = true;
            var instance = RuntimeManager.CreateInstance(audio);
            instance.start();
        }
    }
    void Update()
    {
        if(RuntimeManager.HaveAllBanksLoaded && !hasTrackAvailable && !RuntimeManager.AnySampleDataLoading())
        {
            DelayPlayTrack(delayMusicTrack);
        }
    }
    static void DelayPlayTrack(EventReference audio)
    {
        hasTrackAvailable = true;
        var instance = RuntimeManager.CreateInstance(audio);
        instance.start();
    }
}