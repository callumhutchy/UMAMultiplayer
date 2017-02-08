using UnityEngine;
using System.Collections;
using UMA;
using UMA.PoseTools;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;


public class UMAMakerCharacterCreator : MonoBehaviour
{

    public UMAGeneratorBase generator;
    public SlotLibrary slotLibrary;
    public OverlayLibrary overlayLibrary;
    public RaceLibrary raceLibrary;
    public RuntimeAnimatorController animController;

    private UMADynamicAvatar umaDynamicAvatar;
    private UMAData umaData;
    private UMADnaHumanoid umaDNA;
    private UMADnaTutorial umaTutorialDNA;

    private UserStats user;

    public Animator anim;

    private int numberOfSlots = 20;

    //Character Customisation Sliders
    [Range(0.0f, 1.0f)]
    public float bodyMass = 0.5f;
    public Slider bodyMassSlider;

    [Range(0.0f, 1.0f)]
    public float headSize = 0.5f;
    public Slider headSizeSlider;
    [Range(0.0f, 1.0f)]
    public float headWidth = 0.5f;
    public Slider headWidthSlider;

    [Range(0.0f, 1.0f)]
    public float bodySize = 0.5f;
    public Slider bodySizeSlider;

    [Range(0.0f, 1.0f)]
    public float earPosition = 0.5f;
    public Slider earPositionSlider;
    [Range(0.0f, 1.0f)]
    public float earRotation = 0.5f;
    public Slider earRotationSlider;
    [Range(0.0f, 1.0f)]
    public float earSize = 0.5f;
    public Slider earSizeSlider;

    [Range(0.0f, 1.0f)]
    public float eyesRotation = 0.5f;
    public Slider eyesRotationSlider;
    [Range(0.0f, 1.0f)]
    public float eyesSize = 0.5f;
    public Slider eyesSizeSlider;

    [Range(0.0f, 1.0f)]
    public float jawPosition = 0.5f;
    public Slider jawPositionSlider;
    [Range(0.0f, 1.0f)]
    public float jawSize = 0.5f;
    public Slider jawSizeSlider;

    [Range(0.0f, 1.0f)]
    public float lipSize = 0.5f;
    public Slider lipSizeSlider;

    [Range(0.0f, 1.0f)]
    public float cheeksPosition = 0.5f;
    public Slider cheeksPositionSlider;
    [Range(0.0f, 1.0f)]
    public float cheeksPronunciation = 0.5f;
    public Slider cheeksPronunciationSlider;

    [Range(0.0f, 1.0f)]
    public float chinSize = 0.5f;
    public Slider chinSizeSlider;

    [Range(0.0f, 1.0f)]
    public float mouthSize = 0.5f;
    public Slider mouthSizeSlider;

    [Range(0.0f, 1.0f)]
    public float neckThickness = 0.5f;
    public Slider neckThicknessSlider;

    [Range(0.0f, 1.0f)]
    public float noseCurve = 0.5f;
    public Slider noseCurveSlider;
    [Range(0.0f, 1.0f)]
    public float noseFlatten = 0.5f;
    public Slider noseFlattenSlider;
    [Range(0.0f, 1.0f)]
    public float noseInclination = 0.5f;
    public Slider noseInclinationSlider;
    [Range(0.0f, 1.0f)]
    public float nosePosition = 0.5f;
    public Slider nosePositionSlider;
    [Range(0.0f, 1.0f)]
    public float noseSize = 0.5f;
    public Slider noseSizeSlider;
    [Range(0.0f, 1.0f)]
    public float noseWidth = 0.5f;
    public Slider noseWidthSlider;

    //Camera Positions
    Vector3 closeUpCamPos = new Vector3(0f, 1.6f, -8.6f);
    Vector3 fullBodyCam = new Vector3(0f, 1f, -10f);

    public bool hairState = false;
    private bool lastHairState = false;

    public Color eyeColor = Color.blue;
    private Color lastEyeColor;

    public ColorPicker pickerEyeColor;

    public Color hairColor = Color.black;
    private Color lastHairColor;

    public ColorPicker pickerHairColor;

    public float skinTone = 0.5f;
    private float lastSkinTone;
    public Slider skinToneSlider;

    public Slider skinColorRed;
    public Slider skinColorGreen;
    public Slider skinColorBlue;

    Color skinColor = new Color(1, 1, 1, 1);
    Color lastSkinColor;

    public string SaveString = "";
    public bool save;
    public bool load;

    //public UMAExpressionPlayer expressionPlayer;

    private enum UMASlot
    {
        Eyes,
        Mouth,
        Head,
        Torso,
        Hands,
        Legs,
        Feet,
        Hair,
        ShoulderArmour,
        Gloves,
        TorsoArmour,
        Hat,
        LegArmour

    };

    void Start()
    {

        user = GameObject.FindGameObjectWithTag("UserStats").GetComponent<UserStats>();

        pickerHairColor.onValueChanged.AddListener(color =>
        {
            hairColor = color;
        });
        pickerEyeColor.onValueChanged.AddListener(color =>
        {
            eyeColor = color;
        });

        GenerateUMA();
        //bodyMassSlider.value = bodyMass;
    }

    void Update()
    {

        skinTone = skinToneSlider.value;
        skinColor = new Color(skinTone + skinColorRed.value, skinTone + skinColorGreen.value, skinTone + skinColorBlue.value, 1);

        //Body Values

        bodyMass = bodyMassSlider.value;
        bodySize = bodySizeSlider.value;

        //Face Values
        ///earRotation = earRotationSlider.value;
        //earSize = earSizeSlider.value;

        if (earPositionSlider.value != umaDNA.earsPosition)
        {
            SetFeature(earPositionSlider.value, "earposition");
        }
        if (earRotationSlider.value != umaDNA.earsRotation)
        {
            SetFeature(earRotationSlider.value, "earrotation");
        }
        if (earSizeSlider.value != umaDNA.earsSize)
        {
            SetFeature(earSizeSlider.value, "earsize");
        }

        if (eyesRotationSlider.value != umaDNA.eyeRotation)
        {
            SetFeature(eyesRotationSlider.value, "eyerotation");
        }
        if (eyesSizeSlider.value != umaDNA.eyeSize)
        {
            SetFeature(eyesSizeSlider.value, "eyesize");
        }

        if (headSizeSlider.value != umaDNA.headSize)
        {
            SetFeature(headSizeSlider.value, "headsize");
        }
        if (headWidthSlider.value != umaDNA.headWidth)
        {
            SetFeature(headWidthSlider.value, "headwidth");
        }

        if (jawPositionSlider.value != umaDNA.jawsPosition)
        {
            SetFeature(jawPositionSlider.value, "jawposition");
        }
        if (jawSizeSlider.value != umaDNA.jawsSize)
        {
            SetFeature(jawSizeSlider.value, "jawsize");
        }

        if (lipSizeSlider.value != umaDNA.lipsSize)
        {
            SetFeature(lipSizeSlider.value, "lipsize");
        }

        if (cheeksPositionSlider.value != umaDNA.lowCheekPronounced)
        {
            SetFeature(cheeksPositionSlider.value, "cheekposition");
        }
        if (cheeksPronunciationSlider.value != umaDNA.lowCheekPronounced)
        {
            SetFeature(cheeksPronunciationSlider.value, "cheekpronunciation");
        }

        if (chinSizeSlider.value != umaDNA.mandibleSize)
        {
            SetFeature(chinSizeSlider.value, "chinsize");
        }

        if (mouthSizeSlider.value != umaDNA.mouthSize)
        {
            SetFeature(mouthSizeSlider.value, "mouthsize");
        }

        if (neckThicknessSlider.value != umaDNA.neckThickness)
        {
            SetFeature(neckThicknessSlider.value, "neckthickness");
        }

        if (noseCurveSlider.value != umaDNA.noseCurve)
        {
            SetFeature(noseCurveSlider.value, "nosecurve");
        }
        if (noseFlattenSlider.value != umaDNA.noseFlatten)
        {
            SetFeature(noseFlattenSlider.value, "noseflatten");
        }
        if (noseInclinationSlider.value != umaDNA.noseInclination)
        {
            SetFeature(noseInclinationSlider.value, "noseinclination");
        }
        if (nosePositionSlider.value != umaDNA.nosePosition)
        {
            SetFeature(nosePositionSlider.value, "noseposition");
        }
        if (noseSizeSlider.value != umaDNA.noseSize)
        {
            SetFeature(noseSizeSlider.value, "nosesize");
        }
        if (noseWidthSlider.value != umaDNA.noseWidth)
        {
            SetFeature(noseWidthSlider.value, "nosewidth");
        }

        if (bodyMass != umaDNA.upperMuscle)
        {
            SetBodyMass(bodyMass);
        }

        if (bodySize != umaDNA.legsSize)
        {
            SetSize(bodySize);
        }

        if (hairState && !lastHairState)
        {
            lastHairState = hairState;
            SetSlot((int)UMASlot.Hair, "MaleHairShaggy");
            AddOverlay((int)UMASlot.Hair, "MaleHairShaggy", hairColor);
            umaData.isMeshDirty = true;
            umaData.isTextureDirty = true;
            umaData.isShapeDirty = true;
            umaData.Dirty();
        }
        else if (!hairState && lastHairState)
        {
            lastHairState = hairState;
            RemoveSlot((int)UMASlot.Hair);
            umaData.isMeshDirty = true;
            umaData.isTextureDirty = true;
            umaData.isShapeDirty = true;
            umaData.Dirty();
        }

        if (hairColor != lastHairColor && hairState)
        {
            lastHairColor = hairColor;
            ColorOverlay((int)UMASlot.Hair, "MaleHairShaggy", hairColor);
            ColorOverlay((int)UMASlot.Head, "MaleEyebrow01", hairColor);
            umaData.isTextureDirty = true;
            umaData.Dirty();
        }

        if (skinColor != lastSkinColor)
        {
            lastSkinColor = skinColor;
            AddOverlay((int)UMASlot.Head, "MaleHead02", skinColor);
            AddOverlay((int)UMASlot.Torso, "MaleBody02", skinColor);
            umaData.isTextureDirty = true;
            umaData.Dirty();
        }

        if (eyeColor != lastEyeColor)
        {
            lastEyeColor = eyeColor;
            AddOverlay((int)UMASlot.Eyes, "EyeOverlayAdjust", eyeColor);
            umaData.isTextureDirty = true;
            umaData.Dirty();
        }

        if (save)
        {
            save = false;
            Save("Test");
        }
        if (load)
        {
            load = false;
            Load();
            // LoadAsset();
        }
    }

    void GenerateUMA()
    {
        //TODO: Create Unique names for UMA so multiplayer can work
        GameObject go = new GameObject("MyUMA");
        umaDynamicAvatar = go.AddComponent<UMADynamicAvatar>();

        umaDynamicAvatar.Initialize();
        umaData = umaDynamicAvatar.umaData;

        umaDynamicAvatar.umaGenerator = generator;
        umaData.umaGenerator = generator;

        umaData.umaRecipe.slotDataList = new SlotData[numberOfSlots];

        umaDNA = new UMADnaHumanoid();
        umaTutorialDNA = new UMADnaTutorial();
        umaData.umaRecipe.AddDna(umaDNA);
        umaData.umaRecipe.AddDna(umaTutorialDNA);

        CreateMale();

        umaDynamicAvatar.animationController = animController;

        umaDynamicAvatar.UpdateNewRace();

        //Attach Uma to Character controller parent
        go.transform.parent = this.gameObject.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;

    }

    void CreateMale()
    {
        var umaRecipe = umaDynamicAvatar.umaData.umaRecipe;

        umaRecipe.SetRace(raceLibrary.GetRace("HumanMale"));

        //Instantiate Slots
        SetSlot((int)UMASlot.Eyes, "MaleEyes");
        SetSlot((int)UMASlot.Mouth, "MaleInnerMouth");
        SetSlot((int)UMASlot.Head, "MaleFace");
        //SetSlot((int)UMASlot.Torso, "MaleTorso");
        SetSlot((int)UMASlot.Torso, "TorsoTee");
        SetSlot((int)UMASlot.TorsoArmour, "Tee01");
        SetSlot((int)UMASlot.Hands, "MaleHands");
        //SetSlot((int)UMASlot.Legs, "MaleLegs");
        SetSlot((int)UMASlot.LegArmour, "Trousers01");
        //SetSlot((int)UMASlot.Feet, "MaleFeet");
        SetSlot((int)UMASlot.Feet, "SimpleLeatherShoes");

        //Add Overlays

        //Body Parts
        AddOverlay((int)UMASlot.Eyes, "EyeOverlay");
        AddOverlay((int)UMASlot.Mouth, "InnerMouth");
        AddOverlay((int)UMASlot.Head, "MaleHead02");
        AddOverlay((int)UMASlot.Torso, "MaleBody02");
        AddOverlay((int)UMASlot.Feet, "SimpleLeatherShoes");
        AddOverlay((int)UMASlot.LegArmour, "Trousers01");
        AddOverlay((int)UMASlot.TorsoArmour, "Tee01");

        //Apparel
        AddOverlay((int)UMASlot.Torso, "MaleUnderwear01");
        AddOverlay((int)UMASlot.Head, "MaleEyebrow01", hairColor);

        //Set Overlays (For slots which copy overlay of another slot)

        //Copied Torso
        SetOverlay((int)UMASlot.Hands, (int)UMASlot.Torso);
        //SetOverlay((int)UMASlot.Legs, (int)UMASlot.Torso);
        //SetOverlay((int)UMASlot.Feet, (int)UMASlot.Torso);




        //Example changing DNA 0.5f is default 
        //umaDNA.headSize = 1f;

    }

    //////UMA morph routines

    void Dirty()
    {
        umaData.isShapeDirty = true;
        umaData.Dirty();
    }

    void SetFeature(float change, string feature)
    {
        switch (feature)
        {
            case "earposition":
                umaDNA.earsPosition = change;
                break;
            case "earrotation":
                umaDNA.earsRotation = change;
                break;
            case "earsize":
                umaDNA.earsSize = change;
                break;
            case "eyerotation":
                umaDNA.eyeRotation = change;
                break;
            case "eyesize":
                umaDNA.eyeSize = change;
                break;
            case "headsize":
                umaDNA.headSize = change;
                break;
            case "headwidth":
                umaDNA.headWidth = change;
                break;
            case "jawposition":
                umaDNA.jawsPosition = change;
                break;
            case "jawsize":
                umaDNA.jawsSize = change;
                break;
            case "lipsize":
                umaDNA.lipsSize = change;
                break;
            case "cheekposition":
                umaDNA.lowCheekPosition = change;
                break;
            case "cheekpronunciation":
                umaDNA.lowCheekPronounced = change;
                break;
            case "chinsize":
                umaDNA.mandibleSize = change;
                break;
            case "mouthsize":
                umaDNA.mouthSize = change;
                break;
            case "neckthickness":
                umaDNA.neckThickness = change;
                break;
            case "nosecurve":
                umaDNA.noseCurve = change;
                break;
            case "noseflatten":
                umaDNA.noseFlatten = change;
                break;
            case "noseinclination":
                umaDNA.noseInclination = change;
                break;
            case "noseposition":
                umaDNA.nosePosition = change;
                break;
            case "nosesize":
                umaDNA.noseSize = change;
                break;
            case "nosewidth":
                umaDNA.noseWidth = change;
                break;

        }
        Dirty();
    }

    /// <summary>
    /// Change the Body Mass appearance of the UMA.
    /// Doesn't affect the physical properies of the Rigidbody.
    /// </summary>
    /// <param name="mass"></param>
    void SetBodyMass(float mass)
    {
        umaDNA.upperMuscle = mass;
        umaDNA.upperWeight = mass;
        umaDNA.lowerMuscle = mass;
        umaDNA.lowerWeight = mass;
        umaDNA.armWidth = mass;
        umaDNA.forearmWidth = mass;
        Dirty();
    }

    void SetSize(float size)
    {
        //umaDNA.handsSize = size;
        umaDNA.legsSize = size;
        // umaDNA.armLength = size;
        //umaDNA.feetSize = size;
        umaDNA.height = size;
        Dirty();
    }

    void SetHeadSize(float size)
    {
        umaDNA.headSize = size;
        Dirty();
    }

    //////Overlay Helpers

    /// <summary>
    /// Add an Overlay to a Slot
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="overlayName"></param>
    void AddOverlay(int slot, string overlayName)
    {
        umaData.umaRecipe.slotDataList[slot].AddOverlay(overlayLibrary.InstantiateOverlay(overlayName));
    }

    /// <summary>
    /// Add an Overlay to a Slot with a Color option
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="overlayName"></param>
    /// <param name="color"></param>
    void AddOverlay(int slot, string overlayName, Color color)
    {
        umaData.umaRecipe.slotDataList[slot].AddOverlay(overlayLibrary.InstantiateOverlay(overlayName, color));
    }

    /// <summary>
    /// Set the Overlay of a Slot to share the Overlay of another slot
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="fromSlot"></param>
    void SetOverlay(int slot, int fromSlot)
    {
        umaData.umaRecipe.slotDataList[slot].SetOverlayList(umaData.umaRecipe.slotDataList[fromSlot].GetOverlayList());
    }

    /// <summary>
    /// Create a new slot with the slot name
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="slotName"></param>
    void SetSlot(int slot, string slotName)
    {
        umaData.umaRecipe.slotDataList[slot] = slotLibrary.InstantiateSlot(slotName);
    }

    /// <summary>
    /// Remove slot
    /// </summary>
    /// <param name="slot"></param>
    void RemoveSlot(int slot)
    {
        umaData.umaRecipe.slotDataList[slot] = null;
    }

    /// <summary>
    /// Remove the overlay with a certain name from a slot
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="overlayName"></param>
    void RemoveOverlay(int slot, string overlayName)
    {
        umaData.umaRecipe.slotDataList[slot].RemoveOverlay(overlayName);
    }

    /// <summary>
    /// Change the color of an overlay
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="overlayName"></param>
    /// <param name="color"></param>
    void ColorOverlay(int slot, string overlayName, Color color)
    {
        umaData.umaRecipe.slotDataList[slot].SetOverlayColor(color, overlayName);
    }

    /////////// Saving and Loading ////////////////

        string CreateCharacterString()
    {
        UMATextRecipe recipe = ScriptableObject.CreateInstance<UMATextRecipe>();
        recipe.Save(umaDynamicAvatar.umaData.umaRecipe, umaDynamicAvatar.context);
        return  recipe.recipeString;
    }


    void Save(string Name)
    {
        //Generate UMA String

        SaveString = CreateCharacterString();

        //Save string to text file, could upload to server???

        string fileName = "Assets/Characters/" + Name + ".txt";
        StreamWriter stream = File.CreateText(fileName);
        stream.WriteLine(SaveString);
        stream.Close();
    }

    void Load()
    {
        //Load String from text file or Server??
        string fileName = "Assets/Characters/Test.txt";
        StreamReader stream = File.OpenText(fileName);
        SaveString = stream.ReadLine();
        stream.Close();

        //Regenerate UMA using String
        UMATextRecipe recipe = ScriptableObject.CreateInstance<UMATextRecipe>();
        recipe.recipeString = SaveString;
        umaDynamicAvatar.Load(recipe);
    }

    void LoadAsset()
    {
        UMARecipeBase recipe = Resources.Load("Troll") as UMARecipeBase;
        umaDynamicAvatar.Load(recipe);
    }

    //Switching Panels

    public GameObject bodySliders;
    public GameObject faceSliders;

    public void OnSceneSwitch()
    {
        if (bodySliders.activeSelf)
        {
            bodySliders.SetActive(false);
            faceSliders.SetActive(true);
            GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(closeUpCamPos.x, umaDNA.height * 3.2f, closeUpCamPos.z);
        }
        else
        {
            faceSliders.SetActive(false);
            bodySliders.SetActive(true);
            GameObject.FindGameObjectWithTag("MainCamera").transform.position = fullBodyCam;
        }
    }

    public InputField charName;

    void ReturnToAccount()
    {
        SceneManager.LoadScene("AccountManagement");
    }

    public void OnCreateCharacter()
    {
        StartCoroutine(SaveCharacter());
    }

    IEnumerator SaveCharacter()
    {
        WWWForm logform = new WWWForm();
        logform.AddField("user", user.currentUser);
        logform.AddField("character", EscapeBackSlash(CreateCharacterString()));
        logform.AddField("name", charName.text);
        WWW logw = new WWW("hutchygameserver.ddns.net/SaveCharacter.php?", logform);
        yield return logw;
        Debug.Log(logw.text);
        ReturnToAccount();
        
    }

    public void OnExitClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRandomise()
    {
       
        umaDNA.earsPosition = earPosition = earPositionSlider.value = RandomFloat();
        umaDNA.earsRotation = earRotation = earRotationSlider.value = RandomFloat();
        umaDNA.earsSize = earSize = earSizeSlider.value = RandomFloat();

        umaDNA.eyeRotation = eyesRotation = eyesRotationSlider.value = RandomFloat();
        umaDNA.eyeSize = eyesSize = eyesSizeSlider.value = RandomFloat();

        umaDNA.headSize = headSize = headSizeSlider.value = RandomFloat();
        umaDNA.headWidth = headWidth = headWidthSlider.value = RandomFloat();

        umaDNA.jawsPosition = jawPosition = jawPositionSlider.value = RandomFloat();
        umaDNA.jawsSize = jawSize = jawSizeSlider.value = RandomFloat();

        umaDNA.lipsSize = lipSize = lipSizeSlider.value = RandomFloat();

        umaDNA.lowCheekPosition = cheeksPosition = cheeksPositionSlider.value = RandomFloat();
        umaDNA.lowCheekPronounced = cheeksPronunciation = cheeksPronunciationSlider.value = RandomFloat();

        umaDNA.chinSize = chinSize = chinSizeSlider.value = RandomFloat();

        umaDNA.mouthSize = mouthSize = mouthSizeSlider.value = RandomFloat();

        umaDNA.neckThickness = neckThickness = neckThicknessSlider.value = RandomFloat();

        umaDNA.noseCurve = noseCurve = noseCurveSlider.value = RandomFloat();
        umaDNA.noseFlatten = noseFlatten = noseFlattenSlider.value = RandomFloat();
        umaDNA.noseInclination = noseInclination = noseInclinationSlider.value = RandomFloat();
        umaDNA.nosePosition = nosePosition = nosePositionSlider.value = RandomFloat();
        umaDNA.noseSize = noseSize = noseSizeSlider.value = RandomFloat();
        umaDNA.noseWidth = noseWidth = noseWidthSlider.value = RandomFloat();


        skinTone = skinToneSlider.value = Random.Range(0.1f, 0.6f);
        skinColorRed.value = RandomFloat(0.35f, 0.4f);
        skinColorGreen.value = RandomFloat(0.25f, 0.4f);
        skinColorBlue.value = RandomFloat(0.35f, 0.4f);
        skinColor = new Color(skinTone + skinColorRed.value, skinTone + skinColorGreen.value, skinTone + skinColorBlue.value, 1);

    }

    float RandomFloat()
    {
        return Random.Range(0.0f, 1.0f);
    }

    float RandomFloat(float f1, float f2)
    {
        return Random.Range(f1, f2);
    }

    string EscapeBackSlash(string str)
    {
        string test = Regex.Replace(str, @"\\", @"\\");
        Debug.Log(test);
        return test;

    }

}
