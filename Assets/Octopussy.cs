using System.Collections;
using System.Collections.Generic;
using System.Linq;

//using System.Numerics;
using UnityEngine;

public class Octopussy : MonoBehaviour
{

    public GameObject joint1;

    public GameObject joint2;

    public GameObject joint3;

    public GameObject joint4;

    public GameObject Sphere;

    [Range(-180, 180)]
    public float FirstY;

    [Range(-30, 30)]
    public float SecondX;

    [Range(-30, 30)]
    public float ThirdX;

    [Range(-30, 30)]
    public float ForthX;

    Quaternion r1, r2, r3, r4;
    Vector3 newJ1, newJ2, newJ3, newJ4, sp;

    void Start()
    {
        newJ1 = joint1.transform.position; newJ2 = joint2.transform.position; newJ3 = joint3.transform.position; newJ4 = joint4.transform.position;
        sp = Sphere.transform.position;
        r1 = joint1.transform.localRotation;
        r2 = joint2.transform.localRotation;
        r3 = joint3.transform.localRotation;
        r4 = joint4.transform.localRotation;
    }


    void Update()
    {
        Quaternion target1 = Quaternion.Euler(0, FirstY, 0);
        joint1.transform.localRotation = Quaternion.Slerp(joint1.transform.localRotation, target1, Time.deltaTime * 1.0f);

        Quaternion target2 = Quaternion.Euler(SecondX, 0, 0);
        joint2.transform.localRotation = Quaternion.Slerp(joint2.transform.localRotation, target2, Time.deltaTime * 1.0f);

        Quaternion target3 = Quaternion.Euler(ThirdX, 0, 0);
        joint3.transform.localRotation = Quaternion.Slerp(joint3.transform.localRotation, target3, Time.deltaTime * 1.0f);

        Quaternion target4 = Quaternion.Euler(ForthX, 0, 0);
        joint4.transform.localRotation = Quaternion.Slerp(joint4.transform.localRotation, target4, Time.deltaTime * 2.0f);

        List<Vector3> newJ = new List<Vector3>() { newJ1, newJ2, newJ3, newJ4, sp };
        List<float> lFloatY = new List<float>() { FirstY, 0f, 0f, 0f };
        List<float> lFloatX = new List<float>() { 0f, SecondX, ThirdX, ForthX };
        List<Quaternion> rot = new List<Quaternion>() { r1, r2, r3, r4 };

        List<Color> colors = new List<Color>() { Color.blue, Color.yellow, Color.green, Color.blue };

        //Vector3 vector1 = Sphere.transform.position - newJ1;
        //vector1 = Quaternion.Euler(r1.x, r1.y + FirstY, r1.z) * vector1;
        //Sphere.transform.position = joint1.transform.position + vector1;

        for (int i = 0; i < newJ.Count - 1; i++)
        {
            for (int j = 1; j < newJ.Count; j++)
            {
                Vector3 vector1 = newJ[j] - newJ[i];
                vector1 = Quaternion.Euler(rot[i].x + lFloatX[i], rot[i].y + lFloatY[i], rot[i].z) * vector1;
                newJ[j] = newJ[i] + vector1;
            }
            // var t =Instantiate(Sphere);
            //t.transform.position = newJ.Last();
        }

        Sphere.transform.position = newJ[4];


    }
}
