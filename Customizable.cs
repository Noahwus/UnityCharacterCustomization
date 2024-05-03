using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class Customizable : MonoBehaviour
{
    public List<Customization> Customizations;
    public List<ClassCustomization> CCCustomizations;
    int _currentCustomizationIndex;
    int _currentCCCustomizationIndex;

    public Customization CurrentCustomization { get; private set; }
    public TextMeshProUGUI _CurrCustomText;
    public Slider _hueSlider;
    public Toggle _hueToggle;
    public Toggle _skindexToggle;

    public Material skinTexture;
    public List<Texture2D> SkinTones;
    int _skintoneIndex = 0;

    public int _UV;
    public List<Vector2> _materialUVIndex;
    public float div = .25f;
    public bool isBald = false;

    //public 
    void Awake()
    {
        //populate the customizations 
        int i = 0;
        foreach (var customization in Customizations)
        {
            customization.IndexID = i;

            customization.AddRenderers();
            //customization.AddMaterials();
            customization.UpdateRenderers();
            customization.UpdateSubObjects();
            i++;
        }

        
        if(div == null || div == 0) { div = .25f; }
        //Populate the List of UV Offsets
        for (float y = 0f; y < 1f; y += div)
        {
            for (float x = 0f; x < 1f; x += div)
            {
                Vector2 cord = new Vector2(x, y);
                _materialUVIndex.Add(cord);
                //Debug.Log(cord);
            }
        }
       _UV = 0; 
    }

    void Update()
    {
        SelectCustomizationWithUpDownArrows();

        //iterate through the options avaible for the current Customization
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CurrentCustomization.NextMaterial(true);
            CurrentCustomization.NextSubObject(true);
            //CurrentCustomization.UpdateMaterial(new Vector2(0,0)); //reset UV
            CheckBaldness();
            //text.text = CurrentCustomization.DisplayName;
        }
        //randomize all customizations
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectRandomCustomization();
        }
    }

    public void NextCustomizable() //Next customizable: Hair > Hat 
    {
        _currentCustomizationIndex++;
        if (_currentCustomizationIndex == 2) { _currentCustomizationIndex++; }
        //check max values of the current index    
        if (_currentCustomizationIndex < 0)
            _currentCustomizationIndex = Customizations.Count - 1;

        if (_currentCustomizationIndex >= Customizations.Count)
            _currentCustomizationIndex = 0;

        //change the currst Customization to the Index number
        CurrentCustomization = Customizations[_currentCustomizationIndex];
        
    }

    public void PrevCustomizable() //Prev customizable: Hat > Hair
    {
        _currentCustomizationIndex--;
        if (_currentCustomizationIndex == 2) { _currentCustomizationIndex--; }

        if (_currentCustomizationIndex < 0)
            _currentCustomizationIndex = Customizations.Count - 1;

        if (_currentCustomizationIndex >= Customizations.Count)
            _currentCustomizationIndex = 0;

        //change the currst Customization to the Index number
        CurrentCustomization = Customizations[_currentCustomizationIndex];
    }

    public void NextCustomizableOption(bool neg) //Hair1 > Hair2
    {
        //CurrentCustomization.NextMaterial(neg);
        Customizations[_currentCustomizationIndex].NextSubObject(true);
        //CurrentCustomization.NextSubObject(neg);
        Customizations[_currentCustomizationIndex].NextMaterial(true);

        var kek = isBald;
        if (kek != CheckBaldness())
        {
            CCCustomizations[_currentCCCustomizationIndex].CCBaldUpdate(isBald);
        }
    }
    //
    

    public void PreCustomizablevOption() //Hair2 > Hair1
    {
        CurrentCustomization.NextMaterial(false);
        CurrentCustomization.NextSubObject(false);

        var kek = isBald;
        if (kek != CheckBaldness())
        {
            CCCustomizations[_currentCCCustomizationIndex].CCBaldUpdate(isBald);
        }
    }//*/

    
    public void NextCCClass()
    {
        _currentCCCustomizationIndex++;
        if (_currentCCCustomizationIndex < 0) _currentCCCustomizationIndex = CCCustomizations.Count - 1;
        if (_currentCCCustomizationIndex >= CCCustomizations.Count) _currentCCCustomizationIndex = 0;

        for(var i = 0; i< CCCustomizations.Count; i++)
        {
            if(i != _currentCCCustomizationIndex){ 
               CCCustomizations[i].CCSubObjectUpdate(i==_currentCCCustomizationIndex, CheckBaldness());
            }
        }

        //Debug.Log(CheckBaldness() + " isBald");
        CCCustomizations[_currentCCCustomizationIndex].CCSubObjectUpdate(true, CheckBaldness());

        //CCCustomizations[_currentCCCustomizationIndex]
    }
    public void NextCCClass(int k)
    {
        _currentCCCustomizationIndex = k;
        if (_currentCCCustomizationIndex < 0) _currentCCCustomizationIndex = CCCustomizations.Count - 1;
        if (_currentCCCustomizationIndex >= CCCustomizations.Count) _currentCCCustomizationIndex = 0;

        for (var i = 0; i < CCCustomizations.Count; i++)
        {
            if (i != _currentCCCustomizationIndex)
            {
                CCCustomizations[i].CCSubObjectUpdate(i == _currentCCCustomizationIndex, CheckBaldness());
            }
        }

        //Debug.Log(CheckBaldness() + " isBald");
        CCCustomizations[_currentCCCustomizationIndex].CCSubObjectUpdate(true, CheckBaldness());

        //CCCustomizations[_currentCCCustomizationIndex]
    }




    void SelectCustomizationWithUpDownArrows()
    {
        //change what Customization option is 'hightlighted' iterating up
        if (Input.GetKeyDown(KeyCode.DownArrow)) { _currentCustomizationIndex++; 
            if (_currentCustomizationIndex == 2) { _currentCustomizationIndex++; }  }
        //change what Customization option is 'highlighted' interating down
        if (Input.GetKeyDown(KeyCode.UpArrow)) { _currentCustomizationIndex--; 
            if (_currentCustomizationIndex == 2) { _currentCustomizationIndex--; } }  
            //*/
        //check max values of the current index    
        if (_currentCustomizationIndex < 0)
            _currentCustomizationIndex = Customizations.Count - 1;
        
        if (_currentCustomizationIndex >= Customizations.Count)
            _currentCustomizationIndex = 0;
   
        //change the currst Customization to the Index number
        CurrentCustomization = Customizations[_currentCustomizationIndex];
        if(_CurrCustomText != null)
        {
            _CurrCustomText.text = CurrentCustomization.DisplayName;
        }//*/
    }

    public bool CheckBaldness()
    {
        //Check if False
        isBald = false; if (!Customizations[0].CheckForActiveSubObject()) { isBald = true; }
        
        return isBald;
        
        //Align the Subobjects and Material UV between subobjects
        //Customizations[2]._subObjectIndex = Customizations[1]._subObjectIndex;
        //Customizations[2].UVUpdateMaterial(_materialUVIndex[ Customizations[1]._Skindex], Customizations[1]._Skindex);

    }

    public void SkindexAlign()
    {
        _UV = Customizations[_currentCustomizationIndex]._Skindex;
        for(int i = 0; i<Customizations.Count; i++)
        {
            Customizations[i].UVUpdateMaterial(_materialUVIndex[_UV], _UV);
        }
    }

    public void SkindexNext()
    { 
        _UV = Customizations[_currentCustomizationIndex]._Skindex; _UV++;
        if (_UV < 0) _UV = _materialUVIndex.Count - 1;
        if (_UV >= _materialUVIndex.Count) _UV = 0;
        Customizations[_currentCustomizationIndex].UVUpdateMaterial(_materialUVIndex[_UV], _UV);
        
        if (_skindexToggle.isOn)
        {
            SkindexAlign();
        }
    }

    public void SkindexPrev()
    {
        _UV = Customizations[_currentCustomizationIndex]._Skindex; _UV--;
        if (_UV < 0) _UV = _materialUVIndex.Count - 1;
        if (_UV >= _materialUVIndex.Count) _UV = 0;
        Customizations[_currentCustomizationIndex].UVUpdateMaterial(_materialUVIndex[_UV], _UV);
        
        if (_skindexToggle.isOn)
        {
            SkindexAlign();
        }
    }


    public void updateHue()
    {
        //Debug.Log(_hueToggle.isOn);
        
        if (!_hueToggle.isOn) 
        { 
            var cust = Customizations[_currentCustomizationIndex];
            cust.UVUpdateMaterial( _hueSlider.value);
        }

        else
        {
            AlignAllOptions();
        }
        
    }

    public void AlignAllOptions()
    {   
        if(_hueSlider != null) {float matHue = _hueSlider.value;  }
        
        int k = Random.Range(0, _materialUVIndex.Count-1);
        //_UV = Customizations[k]._Skindex;
        
        for(int i = 0; i < Customizations.Count; i++)
        {

            Customizations[i].UVUpdateMaterial(_materialUVIndex[k], k);
        }
        
    }

    public void nextSkinTone(bool next)
    {
        if (!next) { _skintoneIndex--; }
        else { _skintoneIndex++; }

        if(_skintoneIndex < 0) { _skintoneIndex = SkinTones.Count - 1; }
        if(_skintoneIndex >= SkinTones.Count) { _skintoneIndex = 0; }

        skinTexture.mainTexture = SkinTones[_skintoneIndex];
        for (int i = 0; i<Customizations.Count; i++)
        {
            Customizations[i].SkinToneMaterial(SkinTones[_skintoneIndex]);
        }
    }

    //
    public void SelectRandomCustomization()
    {
        int k = Random.Range(0, CCCustomizations.Count);
        _currentCCCustomizationIndex = k;
        NextCCClass(k);
        //skinTexture.mainTexture = SkinTones[Random.Range(0, SkinTones.Count)];
        for (int i = 0; i < Customizations.Count; i++)
        {
            var me = Customizations[i];

            me.RandSubObject();
            me.RandMaterial();
            int j = Random.Range(0, _materialUVIndex.Count);
            _UV = j;
            Vector2 b = _materialUVIndex[j];
            me.UVUpdateMaterial(b, j);

            CheckBaldness();

            me.UpdateSubObjects();
            me.UpdateRenderers();

           
        }
        
        AlignAllOptions();
        CCCustomizations[_currentCCCustomizationIndex].CCBaldUpdate(isBald);
    }
    
   
}

[Serializable]
public class ClassCustomization
{
    public string cc_Class;
    public List<GameObject> cc_SubObjects;
    public List<GameObject> cc_SubSkelObjects;
    public bool Skeleton;
    private int baldNum = 1;


    public void CCSubObjectUpdate(bool on, bool bald)
    {
        if (on)
        {
            for (var i = 0; i < cc_SubObjects.Count; i++)
            {
                if (cc_SubObjects[i])
                {   
                    cc_SubObjects[i].SetActive(true);
                    if(i == baldNum) 
                    { 
                        cc_SubObjects[0].SetActive((!bald));
                        cc_SubObjects[1].SetActive(bald);
                        Debug.Log("isbald: " + bald);
                    }
                }
                    
            }

        }
        else
        {
            for (var i = 0; i < cc_SubObjects.Count; i++)
                if (cc_SubObjects[i])
                    cc_SubObjects[i].SetActive(false);
        }

    }

    public void CCBaldUpdate(bool bald)
    {
        if(cc_SubObjects[0]) cc_SubObjects[0].SetActive((!bald));
        if(cc_SubObjects[1]) cc_SubObjects[1].SetActive(bald);
    }
}
   

[Serializable]
public class Customization
{
    [Header("Primary")]
    public string DisplayName;
    public List<Material> Materials;
    public List<GameObject> SubObjects;
    [HideInInspector] public int IndexID;
    [HideInInspector] public List<Renderer> Renderers;
  
    [HideInInspector] public int _subObjectIndex = 0;
    //[HideInInspector] public int _subColorIndex = 0;
    [HideInInspector] public int _materialIndex = 0;

    [HideInInspector] public int _Skindex;
    [HideInInspector] public float _materialHue = 0;


    public void NextMaterial(bool next)
    {
        if (next) { _materialIndex++; } else { _materialIndex--; }
        _materialIndex++;
        if (_materialIndex >= Materials.Count)
            _materialIndex = 0;

                UpdateRenderers();
    }

    public void SkinToneMaterial(Texture2D skin)
    {
        foreach (var renderer in Renderers)
        {
            if (renderer)
            {
                //Debug.Log(this.DisplayName + "  " + renderer.ToString());
                if (renderer.material.name == "m_SkinTone (Instance)")
                {
                    renderer.material.mainTexture = skin;
                    //renderer.material.SetTextureOffset("_MainTex", Vect);
                    //renderer.material.SetFloat("_HueShift", _materialHue);
                }
               
            }
        }
    }

    public void NextSubObject(bool next)
    {
        if (next) { _subObjectIndex++; } else { _subObjectIndex--; }

        if (_subObjectIndex >= SubObjects.Count)
            _subObjectIndex = 0;
        if (_subObjectIndex < 0)
        {
            _subObjectIndex = SubObjects.Count - 1;
        }

        UpdateSubObjects();
    }

    public void RandSubObject()
    {
        _subObjectIndex = Random.Range(0, SubObjects.Count);
        UpdateSubObjects();
    }
    public void RandMaterial()
    {
        _materialIndex = Random.Range(0, Materials.Count);
        UpdateRenderers();
    }
    
    public Renderer RendererReturn(int i)
    {
        foreach(var renderer in Renderers)
        {
            //Debug.Log(renderer);
        }
        return Renderers[i];
    }

    public void UVUpdateMaterial(Vector2 Vect, int skindex)
    {
        //_materialIndexUV = skindex;
        _Skindex = skindex;
        foreach (var renderer in Renderers) { if (renderer)
            {
                ////////////////HUE SHIFT///////////////////
                //renderer.material.SetFloat("_HueShift", Random.Range(0, 12.5f));
                //Debug.Log(renderer.material.name);
                if (renderer.material.name != "m_SkinTone (Instance)") 
                {
                    renderer.material.SetTextureOffset("_MainTex", Vect); 
                    renderer.material.SetFloat("_HueShift", _materialHue);
                }
                else
                {
                    renderer.material.SetTextureOffset("_MainTex", new Vector2(0, 0));
                    //Debug.Log("SET TO 0");
                }

                

            }
        }
    }
    public void UVUpdateMaterial(float _MATHUE)
    {
        _materialHue = _MATHUE;
        foreach (var renderer in Renderers)
        {
            if (renderer)
            {
                if (renderer.material.name != "m_SkinTone (Instance)")
                { 
                    ////////////////HUE SHIFT///////////////////
                    renderer.material.SetFloat("_HueShift", _MATHUE);
                }
                    
            }
        }
    }



    public void UVUpdateMaterial_All(Vector2 Vect, int skindex, float _MATHUE)
    {
        _Skindex = skindex;
        _materialHue = _MATHUE;
        foreach (var renderer in Renderers)
        {
            if (renderer)
            {
                Debug.Log(renderer.material.name);
                if (renderer.material.name != "m_SkinTone (Instance)") 
                {
                    
                    renderer.material.SetTextureOffset("_MainTex", Vect); 
                    renderer.material.SetFloat("_HueShift", _MATHUE);
                    //Debug.Log(_MATHUE);
                }
                if (renderer.material.name == "m_SkinTone (Instance)")
                {
                    renderer.material.SetTextureOffset("_MainTex", new Vector2(0, 0));
                    Debug.Log("SET TO 0");
                }

                
            }
        }

    }


    public void UpdateSubObjects()
    {
        for (var i = 0; i < SubObjects.Count; i++)
            if (SubObjects[i])
                SubObjects[i].SetActive(i == _subObjectIndex);
    }

    public void HideSubObjects()
    {
        for (var i = 0; i < SubObjects.Count; i++)
            if (SubObjects[i])
                SubObjects[i].SetActive(false);
    }

    public bool CheckForActiveSubObject()
    {
        for (var i = 0; i < SubObjects.Count; i++)
        {
            if (SubObjects[i] != null && SubObjects[i].activeSelf == true)
            {
                return true;
            }
        }
        return false;
    }


    public void AddRenderers()
    {
        for (var i = 0; i < SubObjects.Count; i++)
            if (SubObjects[i]) 
            {
                var rend = SubObjects[i].GetComponent<Renderer>();
                Renderers.Add(rend);
            }    
    }


    public void UpdateRenderers()
    {
        foreach (var renderer in Renderers) { if (renderer)
            {
                foreach (var mat in Materials) { if (mat)
                    {
                        renderer.material = Materials[_materialIndex];
                    }
                }
            }
            
        }
    }
    
}
