using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    public GameObject SElow;//sphere over string
    public GameObject SA;
    public GameObject SD;
    public GameObject SG;
    public GameObject SB;
    public GameObject SEhigh;

    readonly Dictionary<float, float> fretSphereDicX = new Dictionary<float, float>() {
{0f,0.8834f},
{1f,0.84f},
{2f,0.774f},
{3f,0.714f},
{4f,0.656f},
{5f,0.6f},
{6f,0.55f},
{7f,0.503f},
{8f,0.455f},
{9f,0.414f},
{10f,0.372f},
{11f,0.333f},
{12f,0.296f},
{13f,0.262f},
{14f,0.23f},
{15f,0.199f},
{16f,0.171f},
{17f,0.143f},
{18f,0.118f},
{19f,0.093f},
{20f,0.069f},
{21f,0.047f},
{22f,0.027f},
{23f,0.006f}
    };

    readonly Dictionary<float, float> spherePosElow = new Dictionary<float, float>() {
{ 0f,0.0366f},
{ 1f,0.0373f},
{ 2f,0.0385f},
{ 3f,0.0396f},
{ 4f,0.0406f},
{ 5f,0.0415f},
{ 6f,0.0423f},
{ 7f,0.0432f},
{ 8f,0.0441f},
{ 9f,0.0448f},
{ 10f,0.0455f},
{ 11f,0.0462f},
{ 12f,0.047f},
{ 13f,0.048f},
{ 14f,0.049f},
{ 15f,0.05f},
{ 16f,0.051f},
{ 17f,0.051f},
{ 18f,0.051f},
{ 19f,0.051f},
{ 20f,0.051f},
{ 21f,0.051f},
{ 22f,0.052f},
{ 23f,0.052f}
    };

    readonly Dictionary<float, float> spherePosA = new Dictionary<float, float>() {
{ 0f,0.0256f},
{ 1f,0.0264f},
{ 2f,0.027f},
{ 3f,0.0277f},
{ 4f,0.0282f},
{ 5f,0.0287f},
{ 6f,0.0292f},
{ 7f,0.0297f},
{ 8f,0.0301f},
{ 9f,0.0306f},
{ 10f,0.031f},
{ 11f,0.031f},
{ 12f,0.032f},
{ 13f,0.032f},
{ 14f,0.032f},
{ 15f,0.032f},
{ 16f,0.032f},
{ 17f,0.033f},
{ 18f,0.033f},
{ 19f,0.033f},
{ 20f,0.033f},
{ 21f,0.034f},
{ 22f,0.034f},
{ 23f,0.034f}
    };

    readonly Dictionary<float, float> spherePosD = new Dictionary<float, float>() {
{ 0f,0.0144f},
{ 1f,0.0145f},
{ 2f,0.0147f},
{ 3f,0.0149f},
{ 4f,0.0149f},
{ 5f,0.0151f},
{ 6f,0.0146f},
{ 7f,0.0148f},
{ 8f,0.0149f},
{ 9f,0.0151f},
{ 10f,0.0152f},
{ 11f,0.015f},
{ 12f,0.015f},
{ 13f,0.015f},
{ 14f,0.015f},
{ 15f,0.015f},
{ 16f,0.016f},
{ 17f,0.017f},
{ 18f,0.017f},
{ 19f,0.017f},
{ 20f,0.017f},
{ 21f,0.017f},
{ 22f,0.017f},
{ 23f,0.017f}
    };

    readonly Dictionary<float, float> spherePosG = new Dictionary<float, float>() {
{ 0f,0.0022f},
{ 1f,0.0022f},
{ 2f,0.0021f},
{ 3f,0.002f},
{ 4f,0.0007f},
{ 5f,0.0006f},
{ 6f,0.0005f},
{ 7f,0.0004f},
{ 8f,0.0003f},
{ 9f,0.0003f},
{ 10f,0.0003f},
{ 11f,0f},
{ 12f,0f},
{ 13f,0f},
{ 14f,0f},
{ 15f,0f},
{ 16f,0f},
{ 17f,0f},
{ 18f,0f},
{ 19f,0f},
{ 20f,0f},
{ 21f,0f},
{ 22f,0f},
{ 23f,0f}
    };

    readonly Dictionary<float, float> spherePosB = new Dictionary<float, float>() {
{ 0f,-0.0097f},
{ 1f,-0.0101f},
{ 2f,-0.0108f},
{ 3f,-0.0114f},
{ 4f,-0.012f},
{ 5f,-0.0126f},
{ 6f,-0.0132f},
{ 7f,-0.0142f},
{ 8f,-0.0147f},
{ 9f,-0.0151f},
{ 10f,-0.0156f},
{ 11f,-0.016f},
{ 12f,-0.016f},
{ 13f,-0.017f},
{ 14f,-0.017f},
{ 15f,-0.017f},
{ 16f,-0.017f},
{ 17f,-0.017f},
{ 18f,-0.017f},
{ 19f,-0.017f},
{ 20f,-0.018f},
{ 21f,-0.018f},
{ 22f,-0.018f},
{ 23f,-0.018f}
    };

    readonly Dictionary<float, float> spherePosEhigh = new Dictionary<float, float>() {
{ 0f,-0.0206f},
{ 1f,-0.0212f},
{ 2f,-0.0221f},
{ 3f,-0.023f},
{ 4f,-0.0238f},
{ 5f,-0.0245f},
{ 6f,-0.0252f},
{ 7f,-0.0266f},
{ 8f,-0.0273f},
{ 9f,-0.0279f},
{ 10f,-0.0284f},
{ 11f,-0.029f},
{ 12f,-0.029f},
{ 13f,-0.03f},
{ 14f,-0.03f},
{ 15f,-0.031f},
{ 16f,-0.031f},
{ 17f,-0.031f},
{ 18f,-0.032f},
{ 19f,-0.032f},
{ 20f,-0.033f},
{ 21f,-0.033f},
{ 22f,-0.033f},
{ 23f,-0.033f}
    };

    private void Start()
    {
        SElow.transform.position = new Vector3(0.8834f,0.0366f,-0.344f);
        SA.transform.position = new Vector3(0.8834f, 0.0256f, -0.344f);
        SD.transform.position = new Vector3(0.8834f, 0.0144f, -0.344f);
        SG.transform.position = new Vector3(0.8834f, 0.0022f, -0.344f);
        SB.transform.position = new Vector3(0.8834f, -0.0097f, -0.344f);
        SEhigh.transform.position = new Vector3(0.8834f, -0.0206f, -0.344f);
    }

    public void MoveSphereOverString(int stringNumber, int[] all6Frets)
    {
        float fretNumber;
        if (stringNumber == 1)
        {
            fretNumber = all6Frets[0];
            float spherePosYElow = spherePosElow[fretNumber];
            SElow.transform.position = new Vector3(fretSphereDicX[fretNumber], spherePosYElow, -0.344f);
        }
        else if (stringNumber == 2)
        {
            fretNumber = all6Frets[1];
            float spherePosYA = spherePosA[fretNumber];
            SA.transform.position = new Vector3(fretSphereDicX[fretNumber], spherePosYA, -0.344f);
        }
        else if (stringNumber == 3)
        {
            fretNumber = all6Frets[2];
            float spherePosYD = spherePosD[fretNumber];
            SD.transform.position = new Vector3(fretSphereDicX[fretNumber], spherePosYD, -0.344f);
        }
        else if (stringNumber == 4)
        {
            fretNumber = all6Frets[3];
            float spherePosYG = spherePosG[fretNumber];
            SG.transform.position = new Vector3(fretSphereDicX[fretNumber], spherePosYG, -0.344f);
        }
        else if (stringNumber == 5)
        {
            fretNumber = all6Frets[4];
            float spherePosYB = spherePosB[fretNumber];
            SB.transform.position = new Vector3(fretSphereDicX[fretNumber], spherePosYB, -0.344f);
        }
        else if (stringNumber == 6)
        {
            fretNumber = all6Frets[5];
            float spherePosYEhigh = spherePosEhigh[fretNumber];
            SEhigh.transform.position = new Vector3(fretSphereDicX[fretNumber], spherePosYEhigh, -0.344f);
        }
    }
}