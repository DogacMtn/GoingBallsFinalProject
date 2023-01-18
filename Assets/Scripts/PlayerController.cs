using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.UI;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public float speed;
    private int _point;
    private int _live = 3;
    public GameObject restartPanel;
    public TextMeshProUGUI deadText;
    
    public TextMeshProUGUI countText;
    public TextMeshProUGUI liveText;
    private float speedincrease = 1;
    private new Vector3 _spawnPoint;
    private bool isRespawned = true;
    private bool isFinished = false;
    
    private Rigidbody _rb;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _point = 0;
        SetCountText();
        SetLiveText();
        _spawnPoint = transform.position;

    }


    

    void FixedUpdate()
    {
        if (isFinished)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            speedincrease = 0;
        }
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        
        if (isRespawned == false)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        
        if ((moveHorizontal != 0 || moveVertical != 0))
        {
            
            isRespawned = true;
        }
       
        
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        _rb.AddForce(movement * speed * speedincrease);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            other.gameObject.SetActive(false);
            _point += 1;
            SetCountText();
            
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            isFinished = true;
            restartPanel.SetActive(true);
            liveText.gameObject.SetActive(false);
        }
        
        
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            _spawnPoint = transform.position;
        }

        if (other.gameObject.CompareTag("DeadZone"))
        {
            if (_live > 0)
            {
                transform.position = _spawnPoint;
                isRespawned = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                _live = _live - 1;

                SetLiveText(); 
            }
            else
            {
                Dead();
            }
            
            

        }
        
    }


    private void OnCollisionEnter(Collision other)
    {
        
        
        if (other.transform.tag=="Speed")
        {
            speedincrease = 3.8f;
        }
        
        if (other.transform.tag=="Slow")
        {
            speedincrease = 0.1f;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.tag == "Speed")
        {
            speedincrease = 1;
        }
    }

    void SetCountText()
    {
        countText.text = "Point: " + _point.ToString();
    }
    
    void SetLiveText()
    {
        liveText.text = "Live: " + _live.ToString() + "/3";
    }


    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    private void Dead()
    {
        isFinished = true;
        restartPanel.SetActive(true);
        liveText.gameObject.SetActive(false);
        deadText.text = "You are dead loser. \n Try again.";
    }

}