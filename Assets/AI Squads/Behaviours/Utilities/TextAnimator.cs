using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Text))]
public class TextAnimator : MonoBehaviour
{
    [SerializeField] bool loop = false;
    [SerializeField] bool hide_on_finish = false;
    [SerializeField] float hide_delay = 2;
    [SerializeField] string start_text;
    [SerializeField] string animated_text;
    [SerializeField] string end_text;
    [SerializeField] float animation_delay_speed = 1;
    [SerializeField] AudioSource audio_source;
    [SerializeField] AudioClip typing_sound_effect;
    [SerializeField] AudioClip end_sound_effect;

    private Text text_component;
    private float animation_timer = 0;
    private int current_character = 0;
    private string current_text;
    private bool animation_finished = false;


    void Start ()
    {
        text_component = GetComponent<Text>();
        text_component.text = "";//clear any old text just in case
	}
	

	void Update ()
    {
        if (!Application.isPlaying)
        {
            EditorDisplay();
            return;
        }


        AnimateText();
    }


    void EditorDisplay()
    {
        text_component.text = start_text + animated_text + end_text;
    }


    void AnimationFinished()
    {
        text_component.enabled = false;
    }


    void AnimateText()
    {
        string text = start_text;

        if (animated_text.Length > 0)//if theres text to animate
        {
            animation_timer += Time.deltaTime;

            if (animation_timer > animation_delay_speed)//after defined delay
            {
                if (current_character < animated_text.Length)//if we haven't finished the animation
                {
                    PlaySFX(typing_sound_effect, true);
                    current_text += animated_text[current_character];//add the next character
                    ++current_character;
                }
                else
                {
                    if (!loop)
                    {
                        if (hide_on_finish && animation_finished == false)
                        {
                            Invoke("AnimationFinished", hide_delay);
                            PlaySFX(end_sound_effect, true);
                            animation_finished = true;
                        }

                        return;
                    }

                    RestartAnimation();
                }
                animation_timer = 0;
            }
        }

        text += current_text;
        text += end_text;
        text_component.text = text;//display text
    }


    private void PlaySFX(AudioClip _clip, bool _interrupt = false)
    {
        if (audio_source == null || _clip == null)
            return;

        if (!_interrupt)
        {
            if (audio_source.isPlaying)
                return;
        }

        audio_source.PlayOneShot(_clip);
    }


    private void RestartAnimation()
    {
        current_text = "";//start again from the begining
        current_character = 0;
    }
}
