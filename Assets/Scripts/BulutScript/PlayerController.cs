
using Cinemachine;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    public CinemachineVirtualCamera virtualCamera; // Cinemachine sanal kameras�
    [SerializeField] private AudioSource footstepSound; // Ad�m sesi kayna��

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;  // Hareket h�z�
    [SerializeField] private float sprintSpeedMultiplier = 2f;  // Sprint h�z� �arpan�
    [SerializeField] private float sprintTransitSpeed = 5f;  // Sprint ge�i� h�z�
    //[SerializeField] private float turningSpeed = 2f;  // D�n�� h�z� (kullan�lm�yor)
    [SerializeField] private float gravity = 0;  // Yer�ekimi kuvveti (0 olarak ayarlanm��)
    [SerializeField] private float jumpHeight = 2f;  // Z�plama y�ksekli�i

    private float verticalVelocity;  // Dikey h�z
    private float currentSpeed;  // Anl�k h�z
    private float currentSpeedMultiplier;  // Anl�k h�z �arpan�
    private float xRotation;  // X ekseninde d�n�� a��s�

    // Kamera sallanma ayarlar�
    [Header("Camera Bob Settings")]
    [SerializeField] private float bobFrequency = 1f;  // Sallant� frekans�
    [SerializeField] private float bobAmplitude = 1f;  // Sallant� genli�i

    private CinemachineBasicMultiChannelPerlin noiseComponent;  // Cinemachine g�r�lt� bile�eni
    // private float bobTimer = 0f;  // Sallant� zamanlay�c�s�

    // Ad�m sesi ayarlar�
    [Header("Footstep Settings")]
    // [SerializeField] private LayerMask terrainLayerMask;  // Zemin katman� maskesi
    [SerializeField] private float stepInterval = 1f;  // Ad�m aral���

    private float nextStepTimer = 0;  // Bir sonraki ad�m zamanlay�c�s�

    // Ses efektleri
    [Header("SFX")]
    [SerializeField] private AudioClip[] groundFootsteps;  // Zemin ad�m sesleri
    //[SerializeField] private AudioClip[] grassFootsteps;  // �im ad�m sesleri
    //[SerializeField] private AudioClip[] gravelFootsteps;  // �ak�l ad�m sesleri

    [Header("Input")]
    [SerializeField] private float mouseSensitivity;  // Fare hassasiyeti
    private float moveInput;  // �leri/geri hareket girdisi
    private float turnInput;  // Sa�a/sola d�n�� girdisi
    private float mouseX;  // Fare X ekseni hareketi
    private float mouseY;  // Fare Y ekseni hareketi

    private void Start()
    {
        // Bile�enleri al
        controller = GetComponent<CharacterController>();
        noiseComponent = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // �mleci kilitle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Girdi y�netimi
        InputManagement();
        Movement();

        // Ad�m sesi �al
        PlayFootstepSound();
    }

    private void LateUpdate()
    {
        // Kamera sallanma
        CameraBob();
    }

    private void Movement()
    {
        // Zemin hareketi
        GroundMovement();
        Turn();
    }

    private void GroundMovement()
    {
        // Hareket vekt�r�n� olu�tur
        Vector3 move = new Vector3(turnInput, 0, moveInput);
        // Hareket vekt�r�n� kameran�n y�n�ne g�re d�n��t�r
        move = virtualCamera.transform.TransformDirection(move);

        // Sprint kontrol�
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeedMultiplier = sprintSpeedMultiplier;
        }
        else
        {
            currentSpeedMultiplier = 1f;
        }

        // Anl�k h�z� hesapla
        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed * currentSpeedMultiplier, sprintTransitSpeed * Time.deltaTime);

        // Hareket vekt�r�n� h�zla �arp
        move *= currentSpeed;

        // Dikey kuvveti hesapla
        move.y = VerticalForceCalculation();

        // Karakteri hareket ettir
        controller.Move(move * Time.deltaTime);
    }

    private void Turn()
    {
        // Fare hareketini hassasiyetle �arp
        mouseX *= mouseSensitivity * Time.deltaTime;
        mouseY *= mouseSensitivity * Time.deltaTime;

        // X ekseninde d�n��� hesapla
        xRotation -= mouseY;

        // D�n��� s�n�rla
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        // Kameray� d�nd�r
        virtualCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Karakteri d�nd�r
        transform.Rotate(Vector3.up * mouseX);
    }

    private void CameraBob()
    {
        // Karakter yerde ve hareket halindeyse kamera sallanmas�n� etkinle�tir
        if (controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            noiseComponent.m_AmplitudeGain = bobAmplitude * currentSpeedMultiplier;
            noiseComponent.m_FrequencyGain = bobFrequency * currentSpeedMultiplier;
        }
        else
        {
            // De�ilse kamera sallanmas�n� devre d��� b�rak
            noiseComponent.m_AmplitudeGain = 0.0f;
            noiseComponent.m_FrequencyGain = 0.0f;
        }
    }

    private void PlayFootstepSound()
    {
        // Karakter yerde ve hareket halindeyse
        if (controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            // Ad�m aral��� kontrol�
            if (Time.time >= nextStepTimer)
            {
                // Ad�m seslerini belirle
                AudioClip[] footstepClips = DetermineAudioClips();

                // Ad�m sesi �al
                if (footstepClips.Length > 0)
                {
                    AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

                    footstepSound.PlayOneShot(clip);
                }

                // Bir sonraki ad�m zamanlay�c�s�n� ayarla
                nextStepTimer = Time.time + (stepInterval / currentSpeedMultiplier);
            }
        }
    }

    private AudioClip[] DetermineAudioClips()
    {
        // Zemine ���n g�nder
        /*RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, 1.5f, terrainLayerMask))
        {
            // Zeminin etiketine g�re ad�m seslerini belirle
            string tag = hit.collider.tag;

            switch (tag)
            {
                case "Ground":
                    return groundFootsteps;
                case "Grass":
                    return grassFootsteps;
                case "Gravel":
                    return gravelFootsteps;
                default:
                    return groundFootsteps;*/
    

        // Varsay�lan olarak zemin ad�m seslerini d�nd�r
        return groundFootsteps;
    }

    private float VerticalForceCalculation()
    {
        // Karakter yerde ise
        if (controller.isGrounded)
        {
            // Dikey h�z� s�f�rla
            verticalVelocity = -1;

            // Z�plama kontrol�
            if (Input.GetButtonDown("Jump"))
            {
                // Z�plama kuvvetini hesapla
                verticalVelocity = Mathf.Sqrt(jumpHeight * gravity * 2);
            }
        }
        else
        {
            // Havadaysa yer�ekimini uygula
            verticalVelocity -= gravity * Time.deltaTime;
        }
        // Dikey h�z� d�nd�r
        return verticalVelocity;
    }

    private void InputManagement()
    {
        // Girdi de�erlerini al
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }
}
