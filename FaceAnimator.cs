using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class FaceAnimator : MonoBehaviour
{
    public GameObject Mouth; // assign the renderer component in the inspector
    private int mouthIndex;
    private Renderer rend;
    public Vector2 initialMouthOffset; // Customizable's Sets the Initial UV offset for the Mouth
    public List<Mouth> UVOffsets; // a list of UV coordinates to set the texture offset to
    public GameObject Eyes; // assign the Eyes GameObject in the inspector
    public List<GameObject> Eyemojis; // assign the Eyemojis GameObjects in the inspector
    public void Awake()
    {
        rend = Mouth.GetComponent<Renderer>();
        
    }
    public void Start() 
    {
        initialMouthOffset = rend.material.GetTextureOffset("_MainTex");
        mouthIndex = 0;
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {   
            ShowEyemojis(Random.Range(0, Eyemojis.Count));
            ChangeOffset(Random.Range(0, UVOffsets.Count));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            RestoreOffset();
            HideEyemojis();
        }
    }

    public void NextMouth(bool neg)
    {
        if (neg) { mouthIndex--; }
        else { mouthIndex++; } 
        
        if(mouthIndex > UVOffsets.Count)
        {
            mouthIndex = 0;
        }
        if(mouthIndex < 0)
        {
            mouthIndex = UVOffsets.Count - 1;
        }

        ChangeOffset(mouthIndex);
    }

    public void ChangeOffset(int i)
    {
        Vector2 vect = UVOffsets[i].MouthUVLoc;
        rend.material.SetTextureOffset("_MainTex", vect);
        
        //Debug.Log(UVOffsets[i].MouthUVLoc + " " + "UV " + UVOffsets[i].DisplayName);
    }


    public void RestoreOffset()
    {
        //rend.material.SetTextureOffset("_MainTex", initialMouthOffset);
        ChangeOffset(mouthIndex);
    }

    public void ShowEyemojis(int i)
    {
        Eyes.SetActive(false);
        for (int j = 0; j < Eyemojis.Count; j++)
        {
            if (j == i)
            {
                Eyemojis[j].SetActive(true);
            }
            else
            {
                Eyemojis[j].SetActive(false);
            }
        }
    }

    private void Blink()
    {
        ShowEyemojis(3);
        var dd = Random.Range(0,2);
        
        if (dd == 1)
        {
           RandomReaction(); 
        }
    }

    private void BlinkEnd()
    {
        HideEyemojis();
    }

    private void RandomReaction()
    {
        var randmouth = Random.Range(0, UVOffsets.Count);
        ChangeOffset(randmouth);
    }

    public void HideEyemojis()
    {
        Eyes.SetActive(true);
        foreach (GameObject eyemoji in Eyemojis)
        {
            eyemoji.SetActive(false);
        }
    }

    private void RestoreDefaults()
    {
        HideEyemojis();
        ChangeOffset(mouthIndex);
    }
}

[Serializable]
public class Mouth
{
    public string DisplayName;
    public Vector2 MouthUVLoc;
}
