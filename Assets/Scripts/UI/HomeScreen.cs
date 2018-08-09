using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class HomeScreen : MonoBehaviour, IDragHandler, IEndDragHandler
{

    public const int NUM_SCREENS = 5;
    public GameObject[] screens;
    public int focusedScreenIndex;

    public float slideSpeed = 1f;
    public Coroutine dragRoutine;

    public ClickerNetworkManager clickerNetworkManager;

    void Awake()
    {
        clickerNetworkManager = FindObjectOfType<ClickerNetworkManager>();
    }


    public void GoToFightScene(int area)
    {
        clickerNetworkManager.SetDestinationArea(area);
        NetworkManager.singleton.ServerChangeScene("FightScene");
    }

    public void GoToBossScene()
    {
        NetworkManager.singleton.ServerChangeScene("BossScene");
    }

    public void GoToCraftScene()
    {

    }

    public void ShowCharacterScreen()
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        if(dragRoutine != null)
        {
            StopCoroutine(dragRoutine);
        }

        transform.Translate(eventData.delta.x, 0, 0);

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -375*(NUM_SCREENS-1), 0), transform.position.y);
        focusedScreenIndex = (int)((-1 * transform.position.x) + 187.5) / 375;

    }

    IEnumerator SlideToCenter()
    {
        while(transform.position.x % 375 != 0)
        {
            float desiredX = -375 * focusedScreenIndex;

            var pos = transform.position;
            float deltaX = (transform.position.x - desiredX);

            pos.x -= deltaX * slideSpeed * Time.deltaTime;

            if(Math.Abs(transform.position.x - desiredX) < 1)
            {
                pos.x = desiredX;
            }

            transform.position = pos;

            yield return null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragRoutine = StartCoroutine(SlideToCenter());
    }


}
