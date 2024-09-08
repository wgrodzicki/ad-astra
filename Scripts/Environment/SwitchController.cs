using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class controls an interactive switch-like button that can be used by the player
/// to affect the behaviour of target (switchable) objects.
/// </summary>
public class SwitchController : MonoBehaviour, IInteractable
{
    #region
    [Tooltip("Objects that will be affected by intercting with this switch.")]
    public List<SwitchableObject> SwitchableObjects;
    [Tooltip("Switch spatial orientation.")]
    [SerializeField]
    private Orientation _orientation;
    [Tooltip("How quickly should the switch move.")]
    [SerializeField]
    private float _movingSpeed = 0.0f;
    [Tooltip("How far should the switch hide relative to its length.")]
    [SerializeField]
    private float _hideDepth = 0.5f;
    [Tooltip("Whether the switch should return to the default position after being used.")]
    [SerializeField]
    private bool _isBoomerang = false;
    [Tooltip("Delay before the switch comes back to its default positon after being used.")]
    [SerializeField]
    private float _hideDelay = 1.0f;
    [Tooltip("Sound effect to be played when the switch is used.")]
    [SerializeField]
    private AudioClip _soundEffect;

    private float _speed = 0.0f;
    private float _length = 0.0f;
    private float _distanceDown = 0.0f;
    private float _distanceUp = 0.0f;
    private GameObject _sound;

    // State controllers
    private bool _isWithinPlayerRange = false;
    private bool _isMoving = false;
    private bool _isHidden = false;
    private bool _isVisible = true;
    private bool _shouldHide = false;
    private bool _shouldShow = false;

    public UnityEvent OnSwitchControllerUsed;

    private enum Orientation
    {
        VerticalBottom,
        VerticalTop,
        HorizontalLeft,
        HorizontalRight
    }

    public bool IsUsable { get; set; } = true;
    public bool IsBeingUsed { get; set; }
    #endregion

    /// <summary>
    /// Enables the player to interact with the switch.
    /// </summary>
    public void Use()
    {
        if (!IsUsable)
            return;

        if (!_isWithinPlayerRange)
            return;

        IsBeingUsed = true;
        OnSwitchControllerUsed.Invoke();
    }

    /// <summary>
    /// Generates and playes a pooled sound or stops the existing one if has been played.
    /// </summary>
    /// <param name="sound"></param>
    /// <returns></returns>
    public GameObject GenerateSound(GameObject sound)
    {
        if (sound == null)
        {
            GameObject newSound = SoundPool.Instance.GetPooledSound();
            if (newSound != null)
            {
                newSound.GetComponent<AudioSource>().clip = _soundEffect;
                newSound.SetActive(true);
                newSound.GetComponent<AudioSource>().Play();
                return newSound;
            }
            return newSound;
        }
        else
        {
            if (sound.GetComponent<AudioSource>().isPlaying)
                return sound;

            sound.GetComponent<AudioSource>().Stop();
            return null;
        }
    }

    private void Start()
    {
        _length = this.gameObject.transform.localScale.y;
        InputManager.Instance.OnUseButtonPressed += Use;
    }

    private void Update()
    {
        _speed = _movingSpeed * Time.deltaTime;

        if (SwitchableObjects.Count <= 0)
            IsUsable = false;

        AffectTargetObjects();

        if (!HideSwitch())
            ShowSwitch();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player"))
            return;

        if (!IsUsable)
            return;

        _isWithinPlayerRange = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player"))
            return;

        if (!IsUsable)
            return;

        _isWithinPlayerRange = false;
    }

    /// <summary>
    /// Affects target objects whose behaviour depends on the switch.
    /// </summary>
    private void AffectTargetObjects()
    {
        if (SwitchableObjects.Count <= 0)
            return;

        if (!_isWithinPlayerRange)
            return;

        if (!IsBeingUsed)
            return;

        if (SwitchableObjects.Any(x => x.IsSwitching == true))
            return;

        if (!_isMoving)
        {
            if (_isVisible)
            {
                _shouldHide = true;
                _sound = GenerateSound(null);
            }

            // Mark switch for showing if still hidden
            if (_isHidden)
                _shouldShow = true;

            // Affect targets
            SwitchableObjects.ForEach(x => x.Switch(true));
            _isMoving = true;
        }
    }

    /// <summary>
    /// Handles hiding movement of the switch depending on its mode and spatial orientation.
    /// </summary>
    /// <returns></returns>
    private bool HideSwitch()
    {
        if (!_shouldHide)
            return false;

        // Check if moved far enough
        if (_distanceDown >= _length * _hideDepth)
        {
            _isHidden = true;
            _isVisible = false;
            _shouldHide = false;

            // Immediately return to the default position if in boomerang mode
            if (_isBoomerang)
            {
                StartCoroutine(WaitBeforeShowing(_hideDelay));
            }
            // Otherwise wait to be triggered by the player
            else
            {
                _isMoving = false;
                _shouldHide = false;
            }

            _distanceDown = 0.0f;
            return false;
        }

        // Apply linear movement
        switch (_orientation)
        {
            case Orientation.VerticalBottom:
                this.gameObject.transform.position -= new Vector3(0.0f, _speed, 0.0f);
                break;
            case Orientation.VerticalTop:
                this.gameObject.transform.position += new Vector3(0.0f, _speed, 0.0f);
                break;
            case Orientation.HorizontalLeft:
                this.gameObject.transform.position -= new Vector3(_speed, 0.0f, 0.0f);
                break;
            case Orientation.HorizontalRight:
                this.gameObject.transform.position += new Vector3(_speed, 0.0f, 0.0f);
                break;
        }
        _distanceDown += _speed;
        return true;
    }

    /// <summary>
    /// Waits a specified amount of time before showing the switch again after hiding.
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator WaitBeforeShowing(float delay)
    {
        yield return new WaitForSeconds(delay);
        _shouldShow = true;
    }

    /// <summary>
    /// Handles showing movement of the switch depending on its spatial orientation.
    /// </summary>
    private void ShowSwitch()
    {
        if (!_shouldShow)
            return;

        // Check if moved far enough
        if (_distanceUp >= _length * _hideDepth)
        {
            _isVisible = true;
            _isHidden = false;
            _shouldShow = false;
            _isMoving = false;
            IsBeingUsed = false;
            _distanceUp = 0.0f;
            return;
        }

        // Apply linear movement
        switch (_orientation)
        {
            case Orientation.VerticalBottom:
                this.gameObject.transform.position += new Vector3(0.0f, _speed, 0.0f);
                break;
            case Orientation.VerticalTop:
                this.gameObject.transform.position -= new Vector3(0.0f, _speed, 0.0f);
                break;
            case Orientation.HorizontalLeft:
                this.gameObject.transform.position += new Vector3(_speed, 0.0f, 0.0f);
                break;
            case Orientation.HorizontalRight:
                this.gameObject.transform.position -= new Vector3(_speed, 0.0f, 0.0f);
                break;
        }
        _distanceUp += _speed;
    }

    private void OnDsiable()
    {
        InputManager.Instance.OnUseButtonPressed -= Use;
    }
}
