using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;

public class CarUI : MonoBehaviour
{
    public Rigidbody carTarget;
    public DriveController carController;
    private float maxSpeed;
    private float currentSpeed;
    private int currentShift;

    public float minSpeedArrow;
    private float maxSpeedArrow;

    [Header("UI")]
    public TMP_Text speedLabel;
    public RectTransform arrow;
    public TMP_Text gearLabel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        maxSpeed = carController.maxspeed;
        currentShift = carController.currentShift;
        maxSpeedArrow = maxSpeed;
        
        // De 3.6f convert het naar km/h
        currentSpeed = carTarget.velocity.magnitude * 3.6f;
        
        if (gearLabel != null )
        {
            gearLabel.text = currentShift.ToString();
        }
        if (speedLabel != null )
        {
            speedLabel.text = ((int)currentSpeed) + "km/h";
        }
        if (arrow != null )
        {
            arrow.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(minSpeedArrow, maxSpeedArrow, currentSpeed / maxSpeed));
        }

    }
}
