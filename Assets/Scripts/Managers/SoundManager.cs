using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class SoundManager : Singleton<SoundManager> {

    public AudioSource inGameLoopAudioSource;
    public bool isMultipleSounds = false;
    public bool isSound = true;

	#region OneShotSoundClips 

	/*
	 * Audio Clip Files for One Shot
	 */

	public AudioClip buttonClickSound;
	public AudioClip pouringSound;
	public AudioClip waterSound;
	public AudioClip sparkleSound;
    public AudioClip iapSuccessSound;
    public AudioClip dailyBonusSound;
    
    #endregion

    #region InGameLoopSoundClips
    /*
	 * Audio Clip Files for In Game Loop Sound
	 */    
    public AudioClip saltPourSound;
	public AudioClip blenderSound;


    #endregion

    #region BackGroundSoundClips
    /*
	 * Audio Clip Files for BG Loop Sound
	 */
    public AudioClip menuBGSound;
	public AudioClip gameBGSound;
    public AudioClip endGameBGSound;

	#endregion


    
    #region DefaultMethods

    //TODO: Make sure you override your awake by override keyword
	/*override*/ void Awake()
    {
		DontDestroyOnLoad (this.gameObject);
	}
	
	void Start(){


        /*
         * If Sound is mute previously Mute the sound.
         */
        //TODO : Need to change issound with your player prefrence
//        if (!isSound)
//        {
//            GetComponent<AudioSource>().mute = true;
//        }
//        else
//        {
//            GetComponent<AudioSource>().mute = false;
//        }

		/*
		 * Checking whether dual sound enable or not.
		 * If Enable setting MenuBGSound and GameBGSound Accourding.
		 * Else always set GameBGSound.
		 */

        if (isMultipleSounds) 
        {
            this.PlayMenuBackgroundSound();
		} 
        else 
        {
            this.PlayBackgroundSound();
		}
        

		/*
		 * If Sound is not playing Play the musicAudioSource
		 */

        if (!GetComponent<AudioSource>().isPlaying)
		{
            GetComponent<AudioSource>().Play();
		}
	}

	#endregion



	#region InGameLoopSoundMethods

	/*
	 * Stop Playing previous in game loop sound
     * checking is new in game loop sound available playing it.
	 */
    
    public void PlaySaltPourSound()
    {
        StopInGameLoop();

        if(saltPourSound)
        {
			inGameLoopAudioSource.clip = saltPourSound;
            inGameLoopAudioSource.Play();
        }
    }

	public void PlayBlenderSound()
	{
		StopInGameLoop();
		
		if(blenderSound)
		{
			inGameLoopAudioSource.clip = blenderSound;
			inGameLoopAudioSource.Play();
		}
	}

    public void StopInGameLoop()
    {
        inGameLoopAudioSource.Stop();
    }

    #endregion

    #region BGSoundMethods
    
    /*
	 * Playing Different Background Sounds
	 */

    public void PlayBackgroundSound() 
    {
        if (gameBGSound)
        {
            GetComponent<AudioSource>().clip = gameBGSound;
        }
    }

    public void PlayEndGameSound()
    {
        if (endGameBGSound)
        {
            GetComponent<AudioSource>().clip = endGameBGSound;
        }
    }

    public void PlayMenuBackgroundSound()
    {
        if (menuBGSound)
        {
            GetComponent<AudioSource>().clip = menuBGSound;
        }
    }

    #endregion

    #region OneShotSoundMethods

    /*
	 * Playing one shot for each OneShotSound.
     * Muting and UnMuting sound Accordingly.
	 */
    public void PlayButtonClickSound()
    {
        if(buttonClickSound)
        {
            GetComponent<AudioSource>().PlayOneShot(buttonClickSound);
        }
	}

    public void PlayPouringSound()
    {
        if (pouringSound)
        {
			GetComponent<AudioSource>().PlayOneShot(pouringSound);
        }
	}

    public void PlayWaterSound()
    {
        if (waterSound)
        {
			GetComponent<AudioSource>().PlayOneShot(waterSound);
        }
	}

    public void PlaySparkleSound()
    {
        if (sparkleSound)
        {
			GetComponent<AudioSource>().PlayOneShot(sparkleSound);
		}
	}

    public void PlayIAPSucessSound()
    {
        if (iapSuccessSound)
        {
            GetComponent<AudioSource>().PlayOneShot(iapSuccessSound);
        } 
	}

    public void PlayDailyBonusSound()
    {
        if (dailyBonusSound)
        {
            GetComponent<AudioSource>().PlayOneShot(dailyBonusSound);
        }
    }

    public void PlayMuteSound()
    {
		isSound = false;
		GetComponent<AudioSource>().mute = true;
		//TODO : Need to save the sound states 
	}

    public void PlayUnMuteSound()
    {
		isSound = true;
		GetComponent<AudioSource>().mute = false;
		//TODO : Need to save the sound states 
	}

	#endregion
}