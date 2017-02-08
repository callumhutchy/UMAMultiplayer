using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UMA;
using UMA.PoseTools;
using System.IO;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    
    List<string[]> charactersList = new List<string[]>();

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

    public bool load;

    public UserStats user;

    public UMAExpressionPlayer expressionPlayer;

    public VerticalLayoutGroup vlg;
    public Button buttonPrefab;

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
        user = GameObject.FindGameObjectWithTag("UserStats").GetComponent<UserStats>();
        GenerateUMA();
        StartCoroutine(LoadCharacters());



    }

    void InstantiateNewCharacterButton(string[] file)
    {
        Button button = Instantiate(buttonPrefab);
        //Attach Uma to Character controller parent


        button.GetComponentInChildren<Text>().text = file[0];
        button.GetComponent<CharacterSelectButton>().loadString = (string)file[1];
        button.transform.parent = vlg.gameObject.transform;
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

        umaDynamicAvatar.animationController = animController;



        //umaDynamicAvatar.UpdateNewRace();

        //Attach Uma to Character controller parent
        go.transform.parent = this.gameObject.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;

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

    public void OnPlay()
    {
        user.ConnectToServer();
        SceneManager.LoadScene("TestWorld");
    }

    public void Load(string file)
    {
        SaveString = file;

        //Regenerate UMA using String
        UMATextRecipe recipe = ScriptableObject.CreateInstance<UMATextRecipe>();
        recipe.recipeString = SaveString;
        umaDynamicAvatar.Load(recipe);
        Destroy(recipe);
    }

    IEnumerator LoadCharacters()
    {
        WWWForm logform = new WWWForm();
        logform.AddField("user", user.currentUser);
        WWW logw = new WWW("192.168.1.108/RetrieveCharacters.php?", logform);

        //WaitForCharacters:

        yield return logw;




        if (logw.text == "")
        {
            Debug.Log("no data retrieved");
            //goto WaitForCharacters;
        }

        //Debug.Log(logw.text);
        string input = WWW.UnEscapeURL(logw.text);


        if (input == "nocharacters")
        {
            SceneManager.LoadScene("CharacterCreation");
        }
        else
        {
            SplitStrings(input);
        }


    }

    void SplitStrings(string data)
    {

        //Debug.Log("Number of characters is: " + numberOfRows);

        string[] characters = data.Split('#');
        Debug.Log(characters[0]);
        Debug.Log(characters.Length - 1);
        for (int i = 0; i < characters.Length - 1
            ; i++)
        {
            charactersList.Add(characters[i].Split('/'));
        }

        if (charactersList.Count == user.numberOfCharacters)
        {
            for (int i = 0; i < charactersList.Count; i++)
            {
                InstantiateNewCharacterButton(charactersList[i]);
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
