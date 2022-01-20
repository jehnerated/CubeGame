using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupScript : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject arrowPrefab;
    public GameObject highlightBox;
    public GameObject highlightButton;
    public GameObject winPanel;

    public AudioSource leftSpin;
    public AudioSource rightSpin;
    public AudioSource upSpin;
    public AudioSource downSpin;
    public AudioSource winSound;

    public Camera mainCamera;

    public int gridsize = 5;

    int rotateStep = 0;
    int rotateAxis = 0;

    bool isRotating = false;
    bool matchFound = false;

    float rotateRow = 0;

    List<GameObject> cubes = new List<GameObject>();
    List<GameObject> frontFaces = new List<GameObject>();

    void Start()
    {
        if (PlayerPrefs.GetInt("Grid Size") < 12 && PlayerPrefs.GetInt("Grid Size") > 3)
        {
            gridsize = PlayerPrefs.GetInt("Grid Size");
        }
        else
        {
            gridsize = 5;
        }

        float gridOffset = ((gridsize * 2.5f) / 2) - 1.25f;
        float randomX = 0;
        float randomY = 0;
        float randomZ = 0;

        GameObject gameCube;
        GameObject upArrow;
        GameObject downArrow;
        GameObject leftArrow;
        GameObject rightArrow;

        mainCamera.transform.position = new Vector3(0, 0, -10 - (gridsize * 1f));
        mainCamera.fieldOfView = 60 + (gridsize * 2);
        //mainCamera.orthographicSize = gridsize * 2f;
        //mainCamera.transform.position = new Vector3(-(gridsize / 2), gridsize / 2, -10);
        
        for (int n = 0; n < gridsize; n++)
        {
            for (int i = 0; i < gridsize; i++)
            {
                randomX = 90 * Random.Range(0, 3);
                randomY = 90 * Random.Range(0, 3);
                randomZ = 90 * Random.Range(0, 3);

                gameCube = (GameObject)Instantiate(cubePrefab, new Vector3((i * 2.5F) - gridOffset, (n * 2.5F) - gridOffset, 0), Quaternion.Euler(randomX, randomY, randomZ));
                gameCube.name = "GameCube_" + System.Convert.ToChar(n + 65) + (i + 1).ToString();
                cubes.Add(gameCube);
            }
            upArrow = (GameObject)Instantiate(arrowPrefab, new Vector3((n * 2.5F) - gridOffset, gridOffset + 2.5f, 0), Quaternion.Euler(-90, 0, 90));
            upArrow.name = "UpArrow_" + System.Convert.ToChar(n + 65);
            upArrow.tag = "Up Arrow";

            downArrow = (GameObject)Instantiate(arrowPrefab, new Vector3((n * 2.5F) - gridOffset, -gridOffset - 2.5f, 0), Quaternion.Euler(90, 0, 90));
            downArrow.name = "DownArrow_" + System.Convert.ToChar(n + 65);
            downArrow.tag = "Down Arrow";

            rightArrow = (GameObject)Instantiate(arrowPrefab, new Vector3(gridOffset + 2.5f, (n * 2.5F) - gridOffset, 0), Quaternion.Euler(0, 90, 0));
            rightArrow.name = "DightArrow_" + (n + 1).ToString();
            rightArrow.tag = "Right Arrow";

            leftArrow = (GameObject)Instantiate(arrowPrefab, new Vector3(-gridOffset - 2.5f, (n * 2.5F) - gridOffset, 0), Quaternion.Euler(0, -90, 0));
            leftArrow.name = "LeftArrow_" + (n + 1).ToString();
            leftArrow.tag = "Left Arrow";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    switch (hit.transform.tag)
                    {
                        case "Up Arrow":
                            rotateAxis = 2;
                            rotateRow = hit.transform.position.x;
                            upSpin.Play();
                            isRotating = true;
                            break;
                        case "Down Arrow":
                            rotateAxis = 4;
                            rotateRow = hit.transform.position.x;
                            downSpin.Play();
                            isRotating = true;
                            break;
                        case "Left Arrow":
                            rotateAxis = 1;
                            rotateRow = hit.transform.position.y;
                            leftSpin.Play();
                            isRotating = true;
                            break;
                        case "Right Arrow":
                            rotateAxis = 3;
                            rotateRow = hit.transform.position.y;
                            rightSpin.Play();
                            isRotating = true;
                            break;
                    }

                }
            }
        }

        if (isRotating == true)
        {
            if(rotateStep < 10)
            {
                RotateRow(rotateAxis, rotateRow);
                rotateStep += 1;
            }
            else
            {
                isRotating = false;
                rotateStep = 0;
                CheckForMatches();
                highlightButton.SetActive(true);
            }

        }
    }

    public void RotateRow(int Axis, float coordinate)
    {
        float rotateAmount = 9;
        if (Axis % 2 == 0)
        {
            foreach (GameObject i in cubes)
            {
                if(Axis == 2) rotateAmount = 9;
                if (Axis == 4) rotateAmount = -9;

                if (i.transform.position.x == coordinate)
                {
                    i.transform.Rotate(rotateAmount, 0, 0, Space.World);
                }
            }
        }
        else
        {
            foreach (GameObject i in cubes)
            {
                if (Axis == 1) rotateAmount = 9;
                if (Axis == 3) rotateAmount = -9;
                if (i.transform.position.y == coordinate)
                {
                    i.transform.Rotate(0, rotateAmount, 0, Space.World);
                }
            }
        }
    }

    public void CheckForMatches()
    {
        frontFaces.Clear();


        foreach (GameObject i in cubes)
        {
            for (int n = 0; n < i.transform.childCount; n++)
            {
                Transform child = i.transform.GetChild(n);
                if(child.position.z < -0.1)
                {
                    frontFaces.Add(child.gameObject);
                }
            }
        }

        foreach(GameObject j in frontFaces)
        {

            foreach (GameObject n in frontFaces)
            {

                if (
                        (j.transform.position.y - n.transform.position.y > 2f) &&
                        (j.transform.position.y - n.transform.position.y < 3f) &&
                        (j.transform.position.x - n.transform.position.x < 0.5f) &&
                        (j.transform.position.x - n.transform.position.x > -0.5f)
                   )
                {
                    if (n.transform.name == j.transform.name)
                    {
                        matchFound = true;
                        highlightBox.SetActive(true);
                        highlightBox.transform.position = new Vector3(n.transform.position.x, n.transform.position.y, -0.8f);
                        highlightBox.transform.rotation = Quaternion.Euler(90, 0, 0);
                    }
                }

                if (
                        (j.transform.position.x - n.transform.position.x > 2f) &&
                        (j.transform.position.x - n.transform.position.x < 3f) &&
                        (j.transform.position.y - n.transform.position.y < 0.5f) &&
                        (j.transform.position.y - n.transform.position.y > -0.5f)
                   )
                {
                    if (n.transform.name == j.transform.name)
                    {
                        matchFound = true;
                        highlightBox.SetActive(true);
                        highlightBox.transform.position = new Vector3(n.transform.position.x, n.transform.position.y, -0.8f);
                        highlightBox.transform.rotation = Quaternion.Euler(0, -90, 90);
                    }
                }
            }
        }

        if(matchFound == false)
        {
            winPanel.SetActive(true);
            winSound.Play();
            winPanel.GetComponent<Animator>().SetBool("GameOver", true);
        }

        frontFaces.Clear();
        matchFound = false;
    }
}
