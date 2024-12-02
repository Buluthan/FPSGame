
using Cinemachine;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    public CinemachineVirtualCamera virtualCamera; // Cinemachine sanal kamerasý
    [SerializeField] private AudioSource footstepSound; // Adým sesi kaynaðý

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;  // Hareket hýzý
    [SerializeField] private float sprintSpeedMultiplier = 2f;  // Sprint hýzý çarpaný
    [SerializeField] private float sprintTransitSpeed = 5f;  // Sprint geçiþ hýzý
    //[SerializeField] private float turningSpeed = 2f;  // Dönüþ hýzý (kullanýlmýyor)
    [SerializeField] private float gravity = 0;  // Yerçekimi kuvveti (0 olarak ayarlanmýþ)
    [SerializeField] private float jumpHeight = 2f;  // Zýplama yüksekliði

    private float verticalVelocity;  // Dikey hýz
    private float currentSpeed;  // Anlýk hýz
    private float currentSpeedMultiplier;  // Anlýk hýz çarpaný
    private float xRotation;  // X ekseninde dönüþ açýsý

    // Kamera sallanma ayarlarý
    [Header("Camera Bob Settings")]
    [SerializeField] private float bobFrequency = 1f;  // Sallantý frekansý
    [SerializeField] private float bobAmplitude = 1f;  // Sallantý genliði

    private CinemachineBasicMultiChannelPerlin noiseComponent;  // Cinemachine gürültü bileþeni
    // private float bobTimer = 0f;  // Sallantý zamanlayýcýsý

    // Adým sesi ayarlarý
    [Header("Footstep Settings")]
    // [SerializeField] private LayerMask terrainLayerMask;  // Zemin katmaný maskesi
    [SerializeField] private float stepInterval = 1f;  // Adým aralýðý

    private float nextStepTimer = 0;  // Bir sonraki adým zamanlayýcýsý

    // Ses efektleri
    [Header("SFX")]
    [SerializeField] private AudioClip[] groundFootsteps;  // Zemin adým sesleri
    //[SerializeField] private AudioClip[] grassFootsteps;  // Çim adým sesleri
    //[SerializeField] private AudioClip[] gravelFootsteps;  // Çakýl adým sesleri

    [Header("Input")]
    [SerializeField] private float mouseSensitivity;  // Fare hassasiyeti
    private float moveInput;  // Ýleri/geri hareket girdisi
    private float turnInput;  // Saða/sola dönüþ girdisi
    private float mouseX;  // Fare X ekseni hareketi
    private float mouseY;  // Fare Y ekseni hareketi

    private void Start()
    {
        // Bileþenleri al
        controller = GetComponent<CharacterController>();
        noiseComponent = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // Ýmleci kilitle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Girdi yönetimi
        InputManagement();
        Movement();

        // Adým sesi çal
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
        // Hareket vektörünü oluþtur
        Vector3 move = new Vector3(turnInput, 0, moveInput);
        // Hareket vektörünü kameranýn yönüne göre dönüþtür
        move = virtualCamera.transform.TransformDirection(move);

        // Sprint kontrolü
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeedMultiplier = sprintSpeedMultiplier;
        }
        else
        {
            currentSpeedMultiplier = 1f;
        }

        // Anlýk hýzý hesapla
        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed * currentSpeedMultiplier, sprintTransitSpeed * Time.deltaTime);

        // Hareket vektörünü hýzla çarp
        move *= currentSpeed;

        // Dikey kuvveti hesapla
        move.y = VerticalForceCalculation();

        // Karakteri hareket ettir
        controller.Move(move * Time.deltaTime);
    }

    private void Turn()
    {
        // Fare hareketini hassasiyetle çarp
        mouseX *= mouseSensitivity * Time.deltaTime;
        mouseY *= mouseSensitivity * Time.deltaTime;

        // X ekseninde dönüþü hesapla
        xRotation -= mouseY;

        // Dönüþü sýnýrla
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        // Kamerayý döndür
        virtualCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Karakteri döndür
        transform.Rotate(Vector3.up * mouseX);
    }

    private void CameraBob()
    {
        // Karakter yerde ve hareket halindeyse kamera sallanmasýný etkinleþtir
        if (controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            noiseComponent.m_AmplitudeGain = bobAmplitude * currentSpeedMultiplier;
            noiseComponent.m_FrequencyGain = bobFrequency * currentSpeedMultiplier;
        }
        else
        {
            // Deðilse kamera sallanmasýný devre dýþý býrak
            noiseComponent.m_AmplitudeGain = 0.0f;
            noiseComponent.m_FrequencyGain = 0.0f;
        }
    }

    private void PlayFootstepSound()
    {
        // Karakter yerde ve hareket halindeyse
        if (controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            // Adým aralýðý kontrolü
            if (Time.time >= nextStepTimer)
            {
                // Adým seslerini belirle
                AudioClip[] footstepClips = DetermineAudioClips();

                // Adým sesi çal
                if (footstepClips.Length > 0)
                {
                    AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

                    footstepSound.PlayOneShot(clip);
                }

                // Bir sonraki adým zamanlayýcýsýný ayarla
                nextStepTimer = Time.time + (stepInterval / currentSpeedMultiplier);
            }
        }
    }

    private AudioClip[] DetermineAudioClips()
    {
        // Zemine ýþýn gönder
        /*RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, 1.5f, terrainLayerMask))
        {
            // Zeminin etiketine göre adým seslerini belirle
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
    

        // Varsayýlan olarak zemin adým seslerini döndür
        return groundFootsteps;
    }

    private float VerticalForceCalculation()
    {
        // Karakter yerde ise
        if (controller.isGrounded)
        {
            // Dikey hýzý sýfýrla
            verticalVelocity = -1;

            // Zýplama kontrolü
            if (Input.GetButtonDown("Jump"))
            {
                // Zýplama kuvvetini hesapla
                verticalVelocity = Mathf.Sqrt(jumpHeight * gravity * 2);
            }
        }
        else
        {
            // Havadaysa yerçekimini uygula
            verticalVelocity -= gravity * Time.deltaTime;
        }
        // Dikey hýzý döndür
        return verticalVelocity;
    }

    private void InputManagement()
    {
        // Girdi deðerlerini al
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }
}
