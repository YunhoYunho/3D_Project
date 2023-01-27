using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : SingleTon<SoundManager>
{
    public Sound[] effects;
    public Sound[] bgms;

    public AudioSource[] effectPlayer;
    public AudioSource bgmPlayer;

    public string[] playSoundName;

    private void Start()
    {
        playSoundName = new string[effectPlayer.Length];
    }

    public void PlayEffect(string name)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (name == effects[i].name)
            {
                for (int j = 0; j < effectPlayer.Length; j++)
                {
                    if (!effectPlayer[j].isPlaying)
                    {
                        effectPlayer[j].clip = effects[i].clip;
                        effectPlayer[j].Play();
                        playSoundName[j] = effects[i].name;
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용 중입니다.");
                return;
            }
        }
        Debug.Log(name + "사운드가 SoundManager에 등록되지 않았습니다.");
    }

    public void PlayBGM(string _name)
    {
        for (int i = 0; i < bgms.Length; i++)
        {
            if (_name == bgms[i].name)
            {
                bgmPlayer.clip = bgms[i].clip;
                bgmPlayer.Play();
                return;
            }
        }
        Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았습니다.");
    }

    public void StopAnySound()
    {
        for (int i = 0; i < effectPlayer.Length; i++)
        {
            effectPlayer[i].Stop();
        }
    }

    public void StopEffect(string _name)
    {
        for (int i = 0; i < effectPlayer.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                effectPlayer[i].Stop();
                break;
            }
        }
        Debug.Log("재생 중인" + _name + "사운드가 없습니다. ");
    }
}
