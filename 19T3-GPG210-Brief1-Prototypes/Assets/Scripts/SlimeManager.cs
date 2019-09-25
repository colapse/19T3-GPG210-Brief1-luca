using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Serialization;

public class SlimeManager : MonoBehaviour
{
    public delegate void FocusChangeDel (GameObject gameObject);
    public FocusChangeDel onFocusChange;
      
    public void NotifyFocusChange(GameObject gameObject)
    {
        if(onFocusChange != null)
            onFocusChange (gameObject);
    }
    
    public Material defaultSlimeMaterial;
    public Material activeSlimeMaterial;
    public GameObject slimePrefab;
    
    public float slimeMinVolume = 1;
    public float slimeMaxVolume = 1;
    
    public List<Slime> slimes;

    public Slime activeSlime;
    // Start is called before the first frame update
    void Start()
    {
        if (activeSlime != null)
        {
            activeSlime.GetComponent<Renderer>().material = activeSlimeMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            SplitSlime(activeSlime);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            int nextSlimeIndex = activeSlime != null?slimes.IndexOf(activeSlime)+1:0;
            if (nextSlimeIndex > slimes.Count - 1)
            {
                nextSlimeIndex = 0;
            }
            
            if(activeSlime != null)
                activeSlime.GetComponent<Renderer>().material = defaultSlimeMaterial;

            activeSlime = slimes[nextSlimeIndex];
            if (activeSlime != null)
            {
                activeSlime.GetComponent<Renderer>().material = activeSlimeMaterial;
            }
            else
            {
                slimes.RemoveAll(item => item == null);

                if (slimes.Count > 0)
                {
                    activeSlime = slimes[0];
                    activeSlime.GetComponent<Renderer>().material = activeSlimeMaterial;
                }
            }
            NotifyFocusChange(activeSlime.gameObject);
        }
    }

    void SplitSlime(Slime slime)
    {
        if (slime.Volume >= slimeMinVolume * 2 && slimePrefab)
        {
            GameObject newSlimeObj = Instantiate(slimePrefab,slime.transform.position,slime.transform.rotation);
            Slime newSlime = newSlimeObj.GetComponent<Slime>();

            newSlime.Volume = slime.Volume / 2;
            slime.Volume /= 2;
            
            if(newSlime)
                slimes.Add(newSlime);
            Debug.Log(slime.transform.right);
            newSlime.rb.AddForce(slime.transform.right*30*newSlime.Volume);
            slime.rb.AddForce(slime.transform.right*-30*newSlime.Volume);
        }
    }
}
