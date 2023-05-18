using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CapoController : MonoBehaviour
{
    public GameObject capo;
    //rot, X:0, Z:90, Scale: 0.5, posY: 0.0226f except fret 0 = 0.1081f
    readonly Dictionary<float, float> capoPosX = new Dictionary<float, float> {
{0f,1.734f},
{1f,1.238f},
{2f,1.127f},
{3f,1.047f},
{4f,0.9543f},
{5f,0.854f},
{6f,0.787f},
{7f,0.711f},
{8f,0.64f},
{9f,0.576f},
{10f,0.505f},
{11f,0.439f},
{12f,0.381f},
{13f,0.325f},
{14f,0.276f},
{15f,0.221f},
{16f,0.177f}
    };

    readonly Dictionary<float, float> capoPosZ = new Dictionary<float, float> {
{0f,0.013f},
{1f,-0.071f},
{2f,-0.075f},
{3f,-0.063f},
{4f,-0.0644f},
{5f,-0.074f},
{6f,-0.067f},
{7f,-0.056f},
{8f,-0.054f},
{9f,-0.046f},
{10f,-0.052f},
{11f,-0.046f},
{12f,-0.051f},
{13f,-0.043f},
{14f,-0.04f},
{15f,-0.064f},
{16f,-0.0643f}
    };

    readonly Dictionary<float, float> capoRotY = new Dictionary<float, float> {
{0f,-43.162f},
{1f,-34.3f},
{2f,-37.28f},
{3f,-40.6f},
{4f,-43f},
{5f,-47.4f},
{6f,-49.8f},
{7f,-54.23f},
{8f,-57.9f},
{9f,-62.3f},
{10f,-66.3f},
{11f,-71f},
{12f,-74.9f},
{13f,-79.1f},
{14f,-82.7f},
{15f,-86.4f},
{16f,-90f}
    };

    public TextMeshProUGUI errorDisplayText;

    void Start()
    {
        capo.transform.position = new Vector3(capoPosX[0], 0.1081f, capoPosZ[0]);
        errorDisplayText.gameObject.SetActive(false);
    }

    public void MoveCapoOverFret(int fretNumber)
    {
        if (fretNumber == 0)
        {
            errorDisplayText.gameObject.SetActive(false);
            capo.transform.position = new Vector3(capoPosX[0], 0.1081f, capoPosZ[fretNumber]);
        }
        else if (fretNumber >=1 && fretNumber <= 16)
        {
            errorDisplayText.gameObject.SetActive(false);
            capo.transform.position = new Vector3(capoPosX[fretNumber], 0.0226f, capoPosZ[fretNumber]);
        }
        else
        {
            errorDisplayText.gameObject.SetActive(true);
        }
        //no need of else code here, because it will be filtered in Update of StringClickHandler itself, and this function is called only if userCapoNum >=0 && <= 16
        //however we're doing this for sake of Unit Tests
        if (fretNumber >=0 && fretNumber <= 16)
        {
        Vector3 rotationVector = new Vector3(0f, capoRotY[fretNumber],90f);
        capo.transform.rotation = Quaternion.Euler(rotationVector);
        }
    }
}
