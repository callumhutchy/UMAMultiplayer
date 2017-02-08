using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UMA;
using UMA.PoseTools;
using System.IO;
using UnityEngine.SceneManagement;

public class LoadCharacterInWorld : MonoBehaviour {
    
    public UMAGeneratorBase generator;
    public SlotLibrary slotLibrary;
    public OverlayLibrary overlayLibrary;
    public RaceLibrary raceLibrary;
    public RuntimeAnimatorController animController;

    private UMADynamicAvatar umaDynamicAvatar;
    private UMAData umaData;
    private UMADnaHumanoid umaDNA;
    private UMADnaTutorial umaTutorialDNA;

    public GameObject spawn;

    private int numberOfSlots = 20;

    public bool load;

    public UserStats user;

    public UMAExpressionPlayer expressionPlayer;

    public GameObject namePlate;

    string[] files;

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

        spawn = GameObject.FindGameObjectWithTag("OriginalSpawn");
        this.gameObject.transform.position = spawn.transform.position;
        user = GameObject.FindGameObjectWithTag("UserStats").GetComponent<UserStats>();
        GenerateUMA();
       
    }
    
    void GenerateUMA()
    {
        //TODO: Create Unique names for UMA so multiplayer can work
        GameObject go = new GameObject(user.currentCharacter);
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

        umaDynamicAvatar.animationController = animController;



        //umaDynamicAvatar.UpdateNewRace();
        
        //Attach Uma to Character controller parent
        go.transform.parent = this.gameObject.transform;
        go.transform.localPosition = new Vector3(0f,-1f,0f);
        go.transform.localRotation = Quaternion.identity;
        //user.player = go;

    }
    
    /////////// Saving and Loading ////////////////

    private string SaveString;

    public void Save(string Name, string file)
    {
        //Generate UMA String



        //Save string to text file, could upload to server???

        string fileName = "Assets/Characters/" + Name + ".txt";
        StreamWriter stream = File.CreateText(fileName);
        stream.WriteLine(file);
        stream.Close();
    }

    public void Load()
    {

        SaveString = user.currentCharFile;
        user.SetUMAKit();
        //Regenerate UMA using String
        UMATextRecipe recipe = ScriptableObject.CreateInstance<UMATextRecipe>();
        recipe.recipeString = SaveString;
        Debug.Log(recipe.recipeString);  
        umaDynamicAvatar.Load(recipe);
        Destroy(recipe);

    }

    public void Load(string name, string player) {

        SaveString = name;

        //Regenerate UMA using String
        UMATextRecipe recipe = ScriptableObject.CreateInstance<UMATextRecipe>();
        recipe.recipeString = SaveString;
        Debug.Log("boom " + recipe.recipeString);
        umaDynamicAvatar.Load(recipe);
        Object prefab = Instantiate(Resources.Load("NamePlate")) as GameObject;
        GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        clone.transform.parent = umaDynamicAvatar.transform;
        clone.transform.position = new Vector3(0f, 2.5f, 0f);
        clone.GetComponent<TextMesh>().text = player;
        Destroy(recipe);



    }

    public void LoadOther(string name)
    {
        StartCoroutine(LoadCharacter(name));
    }

    IEnumerator LoadCharacter(string name)
    {
        WWWForm logform = new WWWForm();
        logform.AddField("character", name);
        WWW logw = new WWW("192.168.1.108/GetCharacter.php?", logform);
        yield return logw;
        //Debug.Log(logw.text);
        if (logw.text == "nocharacters")
        {
            SceneManager.LoadScene("CharacterCreation");
        }
        else
        {
            string[] temp = logw.text.Split('@');
            if(temp.Length == 2)
            {
                Load(temp[1], name);
            }
            else
            {
                Load(logw.text, name);
            }

            
        }


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
    
    public void OnExitClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
