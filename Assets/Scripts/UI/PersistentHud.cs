using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersistentHud : MonoBehaviour {
    public static PersistentHud persistentHud;

    public QuestFinishedPopup questBanner;
    public GameObject dropNotificationPanel;
    public Animation sceneTransitionAnimation;
    public Text tipText;

    public GameObject dropNotificationPrefab;

    public List<string> tips;

    public Text levelText;
    public ExperienceBar experienceBar;

    public Text materialsText;

    public FloatingText floatingTextPrefab;
   


    public List<RectTransform> dropNotifications;
    public int notificationThreshold = 4;

    public void Awake()
    {
        if(persistentHud != null)
        {
            Destroy(gameObject);
        }else
        {
            persistentHud = this;
        }

        dropNotifications = new List<RectTransform>();
    }

    public void Start()
    {
        FindObjectOfType<PlayerQuests>().questFinishedEvent.AddListener(ShowQuestBanner);
            FindObjectOfType<PlayerInventory>().itemPickedUpEvent.AddListener(NotifyDrop);
        FindObjectOfType<PersistanceManager>().persistantDataLoadedEvent.AddListener(()=> { UpdateExperience(0); });
        FindObjectOfType<PlayerStats>().gainedExperienceEvent.AddListener(UpdateExperience);
        FindObjectOfType<PlayerCrafting>().materialGainedEvent.AddListener(UpdateMaterials);

    }

    void UpdateExperience(int exp)
    {
        experienceBar.updateLevel(FindObjectOfType<PlayerStats>().GetBaseStatStruct().level); //ew

        if(exp == 0)
        {
            return;
        }

        Vector3 offset = new Vector3(0, -3, 0);
        var position = experienceBar.transform.position + offset;
        ShowFloatingNumber("+" + exp, new Color(194, 0, 209), new Vector3(0.75f, 0.75f), position); 
    }

    void UpdateMaterials(int mats)
    {
        materialsText.text = FindObjectOfType<PlayerCrafting>().materials.ToString();

        if (FindObjectOfType<HomeScreen>().GetFocusedScreen() != Screens.Mining)
        {
            Vector3 offset = new Vector3(0, -1, 0);
            var position = materialsText.transform.position + offset;
            ShowFloatingNumber("+" + mats, Color.cyan, new Vector3(0.5f, 0.5f, 1), position);
        }
    }

    void ShowFloatingNumber(string text, Color color, Vector3 scale, Vector3 position)
    {
        var txt = Instantiate(floatingTextPrefab, transform).GetComponent<FloatingText>();
        txt.GetComponent<Text>().color = color;
        txt.GetComponent<RectTransform>().localScale = scale;
        //Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
        //Vector3 offset = new Vector3(0, -1, 0);
        //var position = materialsText.transform.position + offset;//Camera.main.WorldToScreenPoint(materialsText.transform.position + offset);

        txt.Float(position, text);
    }
    
    public void ShowQuestBanner(Quest quest)
    {
        questBanner.Show(quest.GetQuestName());
    }

    public void NotifyDrop(Item item, bool autoSell)
    {
        
        StartCoroutine(NotifyDropRoutine("Found " + item.itemName + " Lv. " + item.itemLevel, autoSell));
        var inv = FindObjectOfType<PlayerInventory>();

        if(inv.getInventoryItems().Count >= PlayerInventory.MaxItems - notificationThreshold)
        {
            StartCoroutine(NotifyDropRoutine("Inventory almost full! (" + inv.getInventoryItems().Count + "/" + PlayerInventory.MaxItems + ")", false));
        }
        //dropNotificationPopup.Show(item);
    }

    IEnumerator NotifyDropRoutine(string message, bool autosell)
    {
        
        //Move all existing notifications up to make room for the new one
        foreach (RectTransform rt in dropNotifications)
        {
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y + rt.rect.height);
        }

        var panel = Instantiate(dropNotificationPrefab, dropNotificationPanel.transform);
        dropNotifications.Add(panel.GetComponent<RectTransform>());
        panel.GetComponentInChildren<Text>().text = message;
        panel.GetComponent<Animation>().Play("DropNotificationSlide");

        yield return new WaitForSeconds(2f);

        //Fade out
        Animation anim = panel.GetComponent<Animation>();
        anim["DropNotificationSlide"].speed = -1;
        anim["DropNotificationSlide"].time = anim["DropNotificationSlide"].length;
        anim.Play("DropNotificationSlide");
        
    }



    public void CoverSceneTransition()
    {
        sceneTransitionAnimation["SceneChangeAnimation"].speed = 1;
        sceneTransitionAnimation["SceneChangeAnimation"].time = 0;
        sceneTransitionAnimation.Play("SceneChangeAnimation");
        tipText.text = tips[Random.Range(0, tips.Count)];
    }
    public void EndSceneTransition()
    {
        sceneTransitionAnimation["SceneChangeAnimation"].speed = -1;
        sceneTransitionAnimation["SceneChangeAnimation"].time = sceneTransitionAnimation["SceneChangeAnimation"].length;
        sceneTransitionAnimation.Play("SceneChangeAnimation");
    }
}
