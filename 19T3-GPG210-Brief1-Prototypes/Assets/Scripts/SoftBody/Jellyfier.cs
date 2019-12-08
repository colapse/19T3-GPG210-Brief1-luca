using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SoftBody;
using UnityEngine;
using Random = System.Random;

public class Jellyfier : MonoBehaviour
{
    public float bounceSpeed;
    public float fallForce;
    public float stiffness;
    public float bottomStiffness;
    public bool bindBottomVertices = true;
    public int bottomVerticesCount = 12;

    private MeshFilter meshFilter;
    private Mesh mesh;

    private JellyVertex[] jellyVertices;
    private Vector3[] currentMeshVertices;

    private MeshCollider mc;

    public bool useTrigger = false;

    // HACK
    private int[] bottomVertIndexes;
    private Color32[] bcolor;
    private Vector3[] bottomVecs;
    
    // Start is called before the first frame update
    void Start()
    {
        mc = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        mesh.MarkDynamic();

        GetVertices();

        //bottomVertIndexes = currentMeshVertices.OrderBy(x => x.y).Select((v, ind) => ind).Take(12).ToArray();  //V1
        //bottomVertIndexes = currentMeshVertices.OrderBy(x => x.y).Select(v => Array.IndexOf(currentMeshVertices,v)).Take(12).ToArray(); //V2
        bottomVertIndexes = currentMeshVertices.Select((v1,i1)=>new {v1,i1}).OrderBy(x => x.v1.y).Select((v, ind) => v.i1).Take(bottomVerticesCount).ToArray(); //V3
        bcolor = new Color32[bottomVertIndexes.Length];
        bottomVecs = new Vector3[bottomVertIndexes.Length];

        for (int i = 0; i < bottomVertIndexes.Length; i++)
        {
            bcolor[i] = new Color32((byte) UnityEngine.Random.Range(0, 255), (byte) UnityEngine.Random.Range(0, 255),
                (byte) UnityEngine.Random.Range(0, 255), 255);
        }
        
        
        //bottomVertIndexes =  currentMeshVertices.OrderByDescending(x => x.y).Where(); //Select((v, ind) => ind).Take(12).ToArray();

        /*
        Array.Sort(currentMeshVertices, delegate(Vector3 first, Vector3 second){
            return first.y.CompareTo(second.y);
        });
        
        Array.Sort(jellyVertices, delegate(JellyVertex first, JellyVertex second){
            return first.initialVertexPosition.y.CompareTo(second.initialVertexPosition.y);
        });*/



    }

    private void GetVertices()
    {
        jellyVertices = new JellyVertex[mesh.vertices.Length];
        currentMeshVertices = new Vector3[mesh.vertices.Length];

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            jellyVertices[i] = new JellyVertex(i,mesh.vertices[i], mesh.vertices[i], Vector3.zero);
            currentMeshVertices[i] = mesh.vertices[i];
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //HACK
        for (int i = 0; i < currentMeshVertices.Length; i++)
        {
            if (bottomVertIndexes.Contains(i))
            {
                bottomVecs[i] = currentMeshVertices[bottomVertIndexes[i]];
            }
        }*/
        
        // HACK
        if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
 
            RaycastHit hit = new RaycastHit();
 
            if (Physics.Raycast (ray, out hit))
            {
                ApplyPressureToPoint(hit.point, 5);
            }
        }
        
        UpdateVertices();
    }

    private void UpdateVertices()
    {
        for (int i = 0; i < jellyVertices.Length; i++)
        {
            float finalStiffness = stiffness;

            if (bindBottomVertices && bottomVertIndexes.Contains(i))
            {
                finalStiffness = bottomStiffness;
            }
            jellyVertices[i].UpdateVelocity(bounceSpeed);
            jellyVertices[i].Settle(finalStiffness);

            jellyVertices[i].currentVertexPosition += jellyVertices[i].currentVelocity * Time.deltaTime;
            currentMeshVertices[i] = jellyVertices[i].currentVertexPosition;
        }
        /*for (int i = 0; i < jellyVertices.Length; i++)
        {
            jellyVertices[i].UpdateVelocity(bounceSpeed);
            float finalStiffness = /*bottomVecs.Contains(currentMeshVertices[i]) #1#bottomVertIndexes.Contains(i)? 5*stiffness:stiffness;//(bottomVertIndexes.Contains(i))?5*stiffness:stiffness;
            jellyVertices[i].Settle(finalStiffness);

            jellyVertices[i].currentVertexPosition += jellyVertices[i].currentVelocity * Time.deltaTime;
            currentMeshVertices[i] = jellyVertices[i].currentVertexPosition;
        }*/

        mesh.vertices = currentMeshVertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        mc.sharedMesh = mesh;

    }

    private void OnDrawGizmos()
    {
        if (bottomVertIndexes != null && bottomVertIndexes.Length > 0)
        {
            for (int i = 0; i < bottomVertIndexes.Length; i++)
            {
                Gizmos.color = bcolor[i];// Color.red;
                Gizmos.DrawSphere(transform.TransformPoint(currentMeshVertices[bottomVertIndexes[i]]),.1f);
            }
            
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        ContactPoint[] collisionPoints = other.contacts;
        for (int i = 0; i < collisionPoints.Length; i++)
        {
            Vector3 inputPoint = collisionPoints[i].point + (collisionPoints[i].point * .1f);
            ApplyPressureToPoint(inputPoint, fallForce);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!useTrigger)
            return;
        // TODO Hack.
        ApplyPressureToPoint(other.transform.position, fallForce);
    }

    private void ApplyPressureToPoint(Vector3 point, float pressure)
    {
        if (jellyVertices != null)
        {
            for (int i = 0; i < jellyVertices.Length; i++)
            {
                bool ignoreY = (bindBottomVertices && bottomVertIndexes.Contains(i))?true:false;
                jellyVertices[i].ApplyPressureToVertex(transform, point, pressure, ignoreY);
            }
        }
    }
}
