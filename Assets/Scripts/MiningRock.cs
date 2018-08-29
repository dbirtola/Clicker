using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningRock : MonoBehaviour {

    PlayerCrafting playerCrafting;

    const int MAX_ORES = 5;

    public int timeBetweenOres = 5;
    public GameObject[] ores;
    int numCurrentOres = 0;

    public bool devHoldToClick = true;

    public FloatingText floatingTextPrefab;

    void Awake()
    {
        playerCrafting = FindObjectOfType<PlayerCrafting>();
    }

    void Start()
    {
        StartCoroutine(GenerateOres());
        GetComponent<Button>().onClick.AddListener(playerCrafting.MineRock);
        playerCrafting.materialMinedEvent.AddListener(CreateText);
    }

    public void Update()
    {
        if (devHoldToClick) {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);


            if (hit == true && hit.collider.gameObject == gameObject && Input.GetMouseButton(0))
            {
                GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    void CreateText(int val)
    {
        var txt = Instantiate(floatingTextPrefab, transform).GetComponent<FloatingText>();
        // Vector3 offset = new Vector3(Random.Range(-90, 90), Random.Range(-70, 70), 0);
        //var position = Camera.main.WorldToScreenPoint(transform.position);
        txt.GetComponent<Text>().color = Color.cyan;
        Vector3 offset = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0);
        txt.transform.position = txt.transform.position + offset;
        txt.Float(transform.position, val.ToString());
        
    }

    IEnumerator GenerateOres()
    {
        while (true)
        {
            if(numCurrentOres < MAX_ORES)
            {
                int oreType = GetOreType(0);
                Vector3 offset = new Vector3(Random.Range(-70, 70), Random.Range(-50, 50), 0);
                var newOre = Instantiate(ores[oreType], transform.position + offset, Quaternion.identity);
                newOre.transform.SetParent(transform);

                newOre.GetComponent<Button>().onClick.AddListener(()=> {
                    playerCrafting.MineOre(oreType);
                    numCurrentOres--;
                    Destroy(newOre.gameObject);
                });

                numCurrentOres++;
            }

            float oreRateBonus = playerCrafting.GetComponent<PlayerStats>().GetOreRateMultiplier();
            yield return new WaitForSeconds(timeBetweenOres * 1f/(1+oreRateBonus));
        }
    }

    int GetOreType(int start)
    {
        int type = start;

        //1/3 chance to upgrade, keeps going
        if(Random.Range(0, 3) == 0 && type != ores.Length - 1)
        {
            type++;
            type = GetOreType(type);
        }

        return type;
    }
}
