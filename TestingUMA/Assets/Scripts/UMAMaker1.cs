using UnityEngine;
using System.Collections;
using UMA;
using UMA.PoseTools;
using System.IO;

public class UMAMaker1 : MonoBehaviour
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

    private int numberOfSlots = 20;

    [Range(0.0f, 1.0f)]
    public float bodyMass = 0.5f;

    [Range(-1.0f, 1.0f)]
    public float happy = 0f;

    public bool vestState = false;
    private bool lastVestState = false;

    public Color vestColor = Color.white;
    private Color lastVestColor = Color.white;

    public bool hairState = false;
    private bool lastHairState = false;

    public Color hairColor;
    private Color lastHairColor;

    public string SaveString = "";
    public bool save;
    public bool load;

    public UMAExpressionPlayer expressionPlayer;

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
        GenerateUMA();
    }

    void Update()
    {
        if (bodyMass != umaDNA.upperMuscle)
        {
            SetBodyMass(bodyMass);
            umaData.isShapeDirty = true;
            umaData.Dirty();
        }

        if (happy != expressionPlayer.midBrowUp_Down)
        {
            expressionPlayer.midBrowUp_Down = happy;
            expressionPlayer.leftMouthSmile_Frown = happy;
            expressionPlayer.rightMouthSmile_Frown = happy;
        }

        if (vestState && !lastVestState)
        {
            lastVestState = true;
            AddOverlay((int)UMASlot.Torso, "SA_Tee", vestColor);
            AddOverlay((int)UMASlot.Torso, "SA_Logo");
            umaData.isTextureDirty = true;
            umaData.Dirty();
        }
        else if (!vestState && lastVestState)
        {
            lastVestState = false;
            RemoveOverlay((int)UMASlot.Torso, "SA_Tee");
            RemoveOverlay((int)UMASlot.Torso, "SA_Logo");
            umaData.isTextureDirty = true;
            umaData.Dirty();
        }

        if (vestColor != lastVestColor && vestState)
        {
            lastVestColor = vestColor;
            ColorOverlay((int)UMASlot.Torso, "SA_Tee", vestColor);
            umaData.isTextureDirty = true;
            umaData.Dirty();
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
            umaData.isTextureDirty = true;
            umaData.Dirty();
        }

        if (save)
        {
            save = false;
            Save();
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
        //Character Created Event
        umaData.OnCharacterCreated += CharacterCreatedCallback;

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
        //SetSlot((int)UMASlot.TorsoArmour, "ChallengerTorsoArmor");
        //SetSlot((int)UMASlot.Hat, "VikingHelmet");


        //Add Overlays

        //Body Parts
        AddOverlay((int)UMASlot.Eyes, "EyeOverlay");
        AddOverlay((int)UMASlot.Mouth, "InnerMouth");
        AddOverlay((int)UMASlot.Head, "MaleHead02");
        AddOverlay((int)UMASlot.Torso, "MaleBody02");
        AddOverlay((int)UMASlot.Feet, "SimpleLeatherShoes");
        // AddOverlay((int)UMASlot.TorsoArmour, "ChallengerTorsoArmor");
        AddOverlay((int)UMASlot.LegArmour, "Trousers01");
        AddOverlay((int)UMASlot.TorsoArmour, "Tee01");

        //Apparel
        AddOverlay((int)UMASlot.Torso, "MaleUnderwear01");
        AddOverlay((int)UMASlot.Head, "MaleEyebrow01", Color.black);
        //AddOverlay((int)UMASlot.Hat, "VikingHelmet");
        //AddOverlay((int)UMASlot.Torso, "SA_Tee");
        //AddOverlay((int)UMASlot.Torso, "SA_Logo");

        //Set Overlays (For slots which copy overlay of another slot)

        //Copied Torso
        SetOverlay((int)UMASlot.Hands, (int)UMASlot.Torso);
        //SetOverlay((int)UMASlot.Legs, (int)UMASlot.Torso);
        //SetOverlay((int)UMASlot.Feet, (int)UMASlot.Torso);




        //Example changing DNA 0.5f is default 
        //umaDNA.headSize = 1f;

    }

    //////UMA morph routines

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

    void Save()
    {
        //Generate UMA String
        UMATextRecipe recipe = ScriptableObject.CreateInstance<UMATextRecipe>();
        recipe.Save(umaDynamicAvatar.umaData.umaRecipe, umaDynamicAvatar.context);
        SaveString = recipe.recipeString;
        Destroy(recipe);

        //Save string to text file, could upload to server???

        string fileName = "Assets/Characters/Test.txt";
        StreamWriter stream = File.CreateText(fileName);
        stream.WriteLine(SaveString);
        stream.Close();
    }

    public string LoadFile;

    void Load()
    {
        //Load String from text file or Server??
        string fileName = "Assets/Characters/"+LoadFile+".txt";
        StreamReader stream = File.OpenText(fileName);
        SaveString = stream.ReadLine();
        stream.Close();

        //Regenerate UMA using String
        UMATextRecipe recipe = ScriptableObject.CreateInstance<UMATextRecipe>();
        recipe.recipeString = SaveString.Trim() ;
        umaDynamicAvatar.Load(recipe);
        Destroy(recipe);
    }

    void LoadAsset()
    {
        UMARecipeBase recipe = Resources.Load("Troll") as UMARecipeBase;
        umaDynamicAvatar.Load(recipe);
    }


    //When Character is created

    void CharacterCreatedCallback(UMAData umaData)
    {
        //EquipRightHand("RedStaff");

        //Expressions
        UMAExpressionSet expressionSet = umaData.umaRecipe.raceData.expressionSet;
        expressionPlayer = umaData.gameObject.AddComponent<UMAExpressionPlayer>();
        expressionPlayer.expressionSet = expressionSet;
        expressionPlayer.umaData = umaData;
        expressionPlayer.Initialize();
        expressionPlayer.enableBlinking = true;
        expressionPlayer.enableSaccades = true;

    }

    void EquipRightHand(string itemName)
    {
        GameObject staff = GameObject.Find(itemName);
        Transform hand = umaData.skeleton.GetBoneGameObject(UMASkeleton.StringToHash("RightHand")).transform;
        staff.transform.SetParent(hand);
        staff.transform.localPosition = new Vector3(-0.105f, 0f, -0.039f);
        staff.transform.localRotation = Quaternion.Euler(3f, 345f, 351f);
    }



}
