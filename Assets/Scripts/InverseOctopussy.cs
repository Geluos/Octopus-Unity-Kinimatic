using System.Collections;
using System.Collections.Generic;
using System.Linq;

//using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;

public class InverseOctopussy : MonoBehaviour
{

    public GameObject joint1;

    public GameObject joint2;

    public GameObject joint3;

    public GameObject joint4;

    public GameObject Sphere;

    [Range(-120, 120)]
    private float FirstY;

    [Range(0, 40)]
    private float SecondX;

    [Range(10, 60)]
    private float ThirdX;

    [Range(0, 40)]
    private float ForthX;

    Quaternion r1, r2, r3, r4;
    Vector3 newJ1, newJ2, newJ3, newJ4, sp;

    float timer = 0f;

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

        timer += Time.deltaTime;

        if (timer > 1.0f)
        {
            timer -= 1.0f;

            Inverse();
        }

    }

    struct Podbor
    {
        public float y1, x1, x2, x3;
        public float distance;

        public Podbor(float y1, float x1, float x2, float x3) : this()
        {
            this.y1 = y1;
            this.x1 = x1;
            this.x2 = x2;
            this.x3 = x3;

            distance = 0f;
        }
    }

    Podbor mutate(Podbor other)
    {
        Podbor result = new Podbor();
        float q = Random.Range(0f, 100f);

        if (q < 40)
        {
            result.y1 = other.y1;
        }
        else if (q < 90)
        {
            result.y1 = other.y1 + Random.Range(-5, 5);
        }
        else
        {
            Random.Range(-120, 120);
        }
        if (result.y1 > 120f)
            result.y1 = 120f;
        if (result.y1 < -120f)
            result.y1 = -120f;

        if (q < 40)
        {
            result.x2 = other.x2;
        }
        else if (q < 90)
        {
            result.x2 = other.x2 + Random.Range(-5, 5);
        }
        else
        {
            Random.Range(-120, 120);
        }
        if (result.x2 > 40f)
            result.x2 = 40f;
        if (result.x2 < 0f)
            result.x2 = 0f;

        if (q < 40)
        {
            result.x2 = other.x2;
        }
        else if (q < 90)
        {
            result.x2 = other.x2 + Random.Range(-5, 5);
        }
        else
        {
            Random.Range(10f, 60f);
        }
        if (result.x2 > 60)
            result.x2 = 60f;
        if (result.x2 < 10)
            result.x2 = 10;

        if (q < 40)
        {
            result.x3 = other.x3;
        }
        else if (q < 90)
        {
            result.x3 = other.x3 + Random.Range(-5, 5);
        }
        else
        {
            Random.Range(0, 40);
        }
        if (result.x3 > 40)
            result.x3 = 40;
        if (result.x3 < 0)
            result.x3 = 0;

        result.distance = Vector3.Distance(Sphere.transform.position, CalculatePosition(result.y1, result.x1, result.x2, result.x3));

        return result;
    }

    private void Inverse()
    {
        //Genetic Algo

        List<Podbor> list = new List<Podbor>();

        for(int i=0; i<40; i++)
        {
            var l = new Podbor(Random.Range(-120, 120), Random.Range(0, 40), Random.Range(10, 60), Random.Range(0, 40));
            l.distance = Vector3.Distance(Sphere.transform.position, CalculatePosition(l.y1, l.x1, l.x2, l.x3));
            list.Add(l);
        }
        {
            var l = new Podbor(FirstY, SecondX, ThirdX, ForthX);
            l.distance = Vector3.Distance(Sphere.transform.position, CalculatePosition(l.y1, l.x1, l.x2, l.x3));
            list.Add(l);
        }

        for (int i=0; i<50; ++i)
        {
           var t = list.OrderBy(x => x.distance).Take(10).ToList();
            
            for(int j=0; j<30; ++j)
            {
                int q = Random.Range(0, 10);
                t.Add(mutate(t.ElementAt(q)));
            }

            list = t.ToList();
        }

        var res = list.First();

        FirstY = res.y1;
        SecondX = res.x1;
        ThirdX = res.x2;
        ForthX = res.x3;

    }

    Vector3 CalculatePosition(float y1, float x1, float x2, float x3)
    {
        List<Vector3> newJ = new List<Vector3>() { newJ1, newJ2, newJ3, newJ4, sp };
        //List<float> lFloatY = new List<float>() { FirstY, 0f, 0f, 0f };
        List<float> lFloatX = new List<float>() { 0f, x1, x2, x3 };
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
                vector1 = Quaternion.Euler(rot[i].x + lFloatX[i], rot[i].y, rot[i].z) * vector1;
                newJ[j] = newJ[i] + vector1;
            }
            // var t =Instantiate(Sphere);
            //t.transform.position = newJ.Last();
        }

        Vector2 vy = new Vector2(newJ[4].x * Mathf.Cos(Mathf.PI / 180f * (y1 - 90)) - newJ[4].z * Mathf.Sin(Mathf.PI / 180f * (y1 - 90))
            , newJ[4].x * Mathf.Sin(Mathf.PI / 180f * (y1 - 90)) + newJ[4].z * Mathf.Cos(Mathf.PI / 180f * (y1 - 90)));

        newJ[4] = new Vector3(vy.y, newJ[4].y, vy.x);

        return newJ[4];
    }
}
