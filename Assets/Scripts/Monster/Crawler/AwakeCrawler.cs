using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeCrawler : MonoBehaviour
{
    public GameObject Crawler = null;
    private void Awake()
    {
        Crawler.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            Crawler.SetActive(true);
        }
    }
}
