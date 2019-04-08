using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffBar : MonoBehaviour {

    public GameObject buffIconPrefab;

    public PlayerPawn target;

    public List<GameObject> currentBuffIcons;

    public void Awake()
    {
        currentBuffIcons = new List<GameObject>();
    }


	public void Start()
    {
        FindObjectOfType<Player>().playerPawnSetEvent.AddListener(Initialize);
    }

    void Initialize(GameObject playerPawn)
    {
        target = playerPawn.GetComponent<PlayerPawn>();
        target.GetComponent<Buffs>().buffAddedEvent.AddListener(AddBuff);
        target.GetComponent<Buffs>().buffRemovedEvent.AddListener(RemoveBuff);
    }

    //Need to be a bit more proper with this later
    void RemoveBuff(Buff buff)
    {
        foreach(GameObject go in currentBuffIcons)
        {
            if(go.GetComponentInChildren<Text>().text == buff.buffText)
            {
                currentBuffIcons.Remove(go);
                Destroy(go);
                return;
            }
        }
    }

    void AddBuff(Buff buff)
    {
        var icon = Instantiate(buffIconPrefab, transform);
        currentBuffIcons.Add(icon);
        icon.GetComponent<Image>().sprite = buff.sprite;
        icon.GetComponentInChildren<Text>().text = buff.buffText;
        var anim = icon.GetComponent<Animation>();
        anim.Play("BuffAddedAnimation");
    }


}
