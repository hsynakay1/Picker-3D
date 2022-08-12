using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;

[Serializable] 
public class BallAreaTechnicalOperations
{
    public GameObject Elevator;
    public GameObject Walls;
    public TextMeshProUGUI NumberText;
    public int BallToBeThrown;
    public GameObject[] Balls;
    public Transform barrier1;
    public Transform barrier2;
}
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject pickerObject;
    [SerializeField] private GameObject[] pickerPalettes;
    [SerializeField] private GameObject[] bonusBalls;
    [SerializeField] private GameObject ballController;
    [SerializeField] private float speed = 5;
    [SerializeField] private List<BallAreaTechnicalOperations> TechnicalOperations = new List<BallAreaTechnicalOperations>();
    

    private bool pickerMotionStatus;

    bool palletToBe;

    int thrownBallCount;
    int allCheckPointCount;
    int checkPointIndex = 0;
    int counter;

    float FingerPosX;
    private void Awake()
    {
        pickerMotionStatus = false;
    }
    void Start()
    {
        for (int i = 0; i < TechnicalOperations.Count; i++)
        {
            TechnicalOperations[i].NumberText.text = thrownBallCount + "/" + TechnicalOperations[i].BallToBeThrown;
        }

        allCheckPointCount = TechnicalOperations.Count - 1;
    }
    public void Status()
    {
        pickerMotionStatus = true;
    }

    void Update()
    {
        
        if (pickerMotionStatus)
        {
            pickerObject.transform.position += speed * Time.deltaTime * pickerObject.transform.forward;

            if (Time.timeScale != 0)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector3 TouchPosition = Camera.main.ScreenToViewportPoint(new Vector3(touch.position.x, touch.position.y, 10f));

                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            FingerPosX = TouchPosition.x - pickerObject.transform.position.x;
                            break;
                        case TouchPhase.Moved:
                            if (TouchPosition.x - FingerPosX > -1.15 && TouchPosition.x - FingerPosX < 1.15)
                            {
                                pickerObject.transform.position = Vector3.Lerp(pickerObject.transform.position,
                                    new Vector3(TouchPosition.x - FingerPosX, pickerObject.transform.position.y, pickerObject.transform.position.z),
                                    3f);
                            }
                            break;

                    }
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    pickerObject.transform.position = Vector3.Lerp(pickerObject.transform.position,
                        new Vector3(pickerObject.transform.position.x - .1f, pickerObject.transform.position.y, pickerObject.transform.position.z), .5f);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    pickerObject.transform.position = Vector3.Lerp(pickerObject.transform.position,
                        new Vector3(pickerObject.transform.position.x + .1f, pickerObject.transform.position.y, pickerObject.transform.position.z), .5f);
                }
            }
        }

    }
    
    public void ComeToBorder()
    {
        if (palletToBe)
        {
            pickerPalettes[0].SetActive(false);
            pickerPalettes[1].SetActive(false);
        }

        pickerMotionStatus = false;
        Invoke(nameof(StageControl), 3f);
        Collider[] HitColl = Physics.OverlapBox(ballController.transform.position, ballController.transform.localScale / 2, Quaternion.identity);
       

        int i = 0;
        while (i < HitColl.Length)
        {
            HitColl[i].GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, .8f), ForceMode.Impulse);
            i++;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(ballController.transform.position, ballController.transform.localScale);
    }

    public void CountBalls()
    {
        thrownBallCount++;
        TechnicalOperations[checkPointIndex].NumberText.text = thrownBallCount + "/" + TechnicalOperations[checkPointIndex].BallToBeThrown;
    }

    void StageControl()
    {
        if (thrownBallCount >= TechnicalOperations[checkPointIndex].BallToBeThrown)
        {
            counter += thrownBallCount;
            StartCoroutine(ConditionMet());
            foreach (var item in TechnicalOperations[checkPointIndex].Balls)
            {
                item.SetActive(false);
            }

        }
        else
        {
            CanvasManager.Instance.LoseCanvas();
        }
    }
    IEnumerator ConditionMet()
    {
        yield return new WaitForSeconds(1);
        TechnicalOperations[checkPointIndex].Elevator.transform.DOLocalMoveZ(0.0f, 1);
        TechnicalOperations[checkPointIndex].Walls.SetActive(false);
        TechnicalOperations[checkPointIndex].Elevator.GetComponent<Renderer>().material = TechnicalOperations[checkPointIndex].Walls.GetComponentInChildren<Renderer>().material;
        TechnicalOperations[checkPointIndex].NumberText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        TechnicalOperations[checkPointIndex].barrier1.DOLocalRotate(new Vector3(-150, -90, 0), 1);
        TechnicalOperations[checkPointIndex].barrier2.DOLocalRotate(new Vector3(-150, 90, 0), 1);
        yield return new WaitForSeconds(0.5f);
        pickerMotionStatus = true;
        Debug.Log(checkPointIndex);
        pickerObject.transform.localScale += new Vector3(0.10f, 0.0f, 0.10f); 

        if (checkPointIndex == allCheckPointCount)
        {
            StartCoroutine(LastDance());
        }
        else
        {
            checkPointIndex++;
            thrownBallCount = 0;

        }
    }
    
    public void ShowPalette()
    {
        palletToBe = true;
        pickerPalettes[0].SetActive(true);
        pickerPalettes[1].SetActive(true);
    }
    public void AddBonusBalls(int bonusBallIndex)
    {
        bonusBalls[bonusBallIndex].SetActive(true);
    }
    IEnumerator LastDance()
    {
        int ballCount = TechnicalOperations[0].Balls.Length + TechnicalOperations[1].Balls.Length + TechnicalOperations[2].Balls.Length;
        float percent = (counter * 100) / ballCount;
        speed = speed * percent / 10;
        yield return new WaitForSeconds(5);
        pickerMotionStatus = false;
        yield return new WaitForSeconds(1);
        CanvasManager.Instance.WinCanvas();
    }
    
   
}
