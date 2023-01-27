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
                Debug.Log("��� ���� AudioSource�� ��� ���Դϴ�.");
                return;
            }
        }
        Debug.Log(name + "���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�.");
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
        Debug.Log(_name + "���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�.");
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
        Debug.Log("��� ����" + _name + "���尡 �����ϴ�. ");
    }
}
