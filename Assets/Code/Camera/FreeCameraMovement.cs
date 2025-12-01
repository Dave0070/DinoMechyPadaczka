using UnityEngine;

public class FreeCameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Podstawowa prêdkoœæ poruszania
    public float lookSpeed = 2f; // Prêdkoœæ obracania
    public float sensitivity = 2f; // Czu³oœæ myszy
    public float scrollSpeed = 1f; // Zmiana prêdkoœci za pomoc¹ scrolla

    private float rotationX = 0f;
    private float currentSpeed;
    private float scrollSpeedSetting; // Przechowuje prêdkoœæ ustawion¹ przez scroll
    private bool isShiftPressed = false; // Flaga do sprawdzania, czy Shift jest wciœniêty
    private bool isCursorVisible = false; // Flaga do sprawdzania, czy kursor jest widoczny
    private bool isCameraLocked = false; // Flaga do sprawdzania, czy kamera jest zablokowana

    void Start()
    {
        // Ukryj kursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentSpeed = moveSpeed;
        scrollSpeedSetting = moveSpeed; // Ustawienie pocz¹tkowej prêdkoœci
    }

    void Update()
    {
        // Sprawdzenie, czy naciœniêto Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleCharacterSelectionMode();
        }

        // Sprawdzenie, czy naciœniêto Esc
        if (Input.GetKeyDown(KeyCode.Escape) && isCameraLocked)
        {
            ExitCharacterSelectionMode();
        }

        // Jeœli kamera jest zablokowana, nie wykonuj dalszych operacji
        if (isCameraLocked)
        {
            return;
        }

        // Ruch kamery
        float moveHorizontal = Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;

        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;
        transform.position += move;

        // Obracanie kamery
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Ograniczenie ruchu w osi Y

        transform.localRotation = Quaternion.Euler(rotationX, transform.localEulerAngles.y + mouseX, 0);

        // Zmiana prêdkoœci za pomoc¹ scrolla
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isShiftPressed = true; // Ustaw flagê, ¿e Shift jest wciœniêty
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            scrollSpeedSetting += scrollInput * scrollSpeed; // Zmiana prêdkoœci na podstawie scrolla
            scrollSpeedSetting = Mathf.Max(scrollSpeedSetting, 0); // Upewnij siê, ¿e prêdkoœæ nie jest ujemna
            currentSpeed = scrollSpeedSetting; // Ustaw aktualn¹ prêdkoœæ na prêdkoœæ ustawion¹ przez scroll
        }
        else
        {
            if (isShiftPressed)
            {
                // Jeœli Shift by³ wciœniêty, ustaw prêdkoœæ na prêdkoœæ ustawion¹ przez scroll
                currentSpeed = scrollSpeedSetting;
            }
            else
            {
                // Przywróæ podstawow¹ prêdkoœæ, gdy Shift nie jest wciœniêty
                currentSpeed = moveSpeed;
            }
            isShiftPressed = false; // Resetuj flagê
        }
    }

    private void ToggleCharacterSelectionMode()
    {
        isCameraLocked = !isCameraLocked; // Zmieñ stan blokady kamery
        Cursor.visible = isCameraLocked; // Ustaw widocznoœæ kursora
        Cursor.lockState = isCameraLocked ? CursorLockMode.None : CursorLockMode.Locked; // Zablokuj lub odblokuj kursor
    }

    private void ExitCharacterSelectionMode()
    {
        isCameraLocked = false; // Odblokuj kamer        isCameraLocked = false; // Odblokuj kamerê
        Cursor.visible = false; // Ukryj kursor
        Cursor.lockState = CursorLockMode.Locked; // Zablokuj kursor
    }
}