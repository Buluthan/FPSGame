using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
 public float speed = 5f; // Hareket hızı
    public float jumpHeight = 1.5f; // Zıplama yüksekliği
    public float gravity = -9.81f; // Yer çekimi kuvveti
    public Transform cameraTransform; // Kamera transformu
    public float mouseSensitivity = 100f; // Fare hassasiyeti

    private CharacterController characterController;
    private Vector3 velocity; // Oyuncunun dikey hareketini takip eder
    private bool isGrounded; // Oyuncu zeminde mi?
    private float xRotation = 0f; // Kamera X ekseni dönüşü

    private AttackSystem attackSystem; // Saldırı sistemi referansı

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        attackSystem = GetComponent<AttackSystem>(); // AttackSystem bileşenini al
        Cursor.lockState = CursorLockMode.Locked; // Farenin ekran içinde kilitlenmesini sağlar
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleAttack();
    }

    void HandleMovement()
    {
        // Zeminde olup olmadığını kontrol et
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Zeminde hafif bir yapışma kuvveti uygular
        }

        // WASD / Ok tuşlarıyla hareket
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * speed * Time.deltaTime);

        // Zıplama
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Yer çekimi
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        // Fare girişini al
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // X ekseni rotasyonunu sınırlayarak yukarı/aşağı bakışı kontrol et
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Kamerayı döndür
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Oyuncu karakterini sağa/sola döndür
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleAttack()
    {
        // Sol fare tuşuna basıldığında saldırı yap
        if (Input.GetMouseButtonDown(0) && attackSystem != null)
        {
            attackSystem.PerformAttack();
        }
    }
}

