using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("General Setup Setting")]
    [Tooltip("How fast the ship moves up and down based upon player input")][SerializeField] float controlSpeed=30f;
    [Tooltip("how far player moves horizontally ")][SerializeField] float xRange=15f;
    [Tooltip("how far player moves vertically ")][SerializeField] float yRange=15f;

    [Header("Laser gun array")]
    [Tooltip("Add all player lasers here ")]
    [SerializeField] GameObject[] lasers;


    [Header("Screen position based on tuning")]
    [SerializeField] float positionPitchFactor = 2f;
    [SerializeField] float positionYawFactor  = 3f;
    

    [Header("Screen position based on tuning")]
    [SerializeField] float controlPitchFactor = -28f;
    [SerializeField] float controlRollFactor  = -35f;
    float xThrow , yThrow;
    

    void Update()
    {
       ProcessTranslation();
       ProcessRotation();
       ProcessFiring();
    }

    void ProcessRotation()
    {
        float pithDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch =  pithDueToPosition + pitchDueToControlThrow ;
        float yaw   =  transform.localPosition.x * positionYawFactor;
        float roll  = xThrow * controlRollFactor;
        transform.localRotation= Quaternion.Euler(/*pitch*/pitch,/*val*/yaw,/*roll*/roll);
    }
    void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow*Time.deltaTime*controlSpeed;
        float rawXPos = transform.localPosition.x+xOffset;
        float xClampedXPos= Mathf.Clamp(rawXPos,-xRange,xRange);

        float yOffset = yThrow*Time.deltaTime*controlSpeed;
        float rawYPos = transform.localPosition.y+yOffset;
        float yClampedYPos= Mathf.Clamp(rawYPos,-yRange,yRange);

        transform.localPosition = new Vector3 (xClampedXPos,yClampedYPos,transform.localPosition.z);
    
    }

    void ProcessFiring()
    {
        if (Input.GetButton("Fire1"))
        {
            SetLaserActive(true);
        }
        else
        {
            SetLaserActive(false);
        }
    }

    void SetLaserActive (bool isActive)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule= laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled= isActive;
        }

    }
    
}
