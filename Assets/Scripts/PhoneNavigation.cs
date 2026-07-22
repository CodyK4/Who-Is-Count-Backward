using UnityEngine;

public class PhoneNavigation : MonoBehaviour
{
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject messagesScreen;
    [SerializeField] private GameObject galleryScreen;

    public void Start()
    {
        homeScreen.SetActive(true);
        messagesScreen.SetActive(false);
        galleryScreen.SetActive(false);
    }

    public void GoToHomeScreen()
    {
        homeScreen.SetActive(true);
        messagesScreen.SetActive(false);
        galleryScreen.SetActive(false);
    }
    
    public void GoToMessages()
    {
        messagesScreen.SetActive(true);
        homeScreen.SetActive(false);
        galleryScreen.SetActive(false);
    }

    public void GoToGallery()
    {
        galleryScreen.SetActive(true);
        homeScreen.SetActive(false);
        messagesScreen.SetActive(false);
    }
}
