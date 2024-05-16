using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;

public class CarUI : MonoBehaviour
{
    public Rigidbody carTarget;
    public CartTest carController;
    private float maxSpeed;
    private float currentSpeed;
    private int currentShift;

    public float minSpeedArrowAngle;
    public float[] maxSpeedArrowAngle;

    [Header("UI")]
    public RectTransform arrow;
    public TMP_Text gearLabel;

    // Update is called once per frame
    void Update()
    {
        maxSpeed = carController.maxspeed;
        currentShift = carController.currentShift;
        // De 3.6f convert het naar km/h
        currentSpeed = carTarget.velocity.magnitude;

        if (gearLabel != null )
        {
            gearLabel.text = currentShift.ToString();
        }
        if (arrow != null )
        {
            arrow.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle[currentShift], currentSpeed / maxSpeed));
        }
    }
}