using UnityEngine;

public class PhoneNavigation : MonoBehaviour
{
    //A very simple script for rudamentary phone navigation 

    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject messagesScreen;
    [SerializeField] private GameObject conversationsScreen;
    [SerializeField] private GameObject galleryScreen;

    private void Start()
    {
        GoToHomeScreen();
    }

    public void GoToHomeScreen()
    {
        homeScreen.SetActive(true);
        messagesScreen.SetActive(false);
        conversationsScreen.SetActive(false);
        galleryScreen.SetActive(false);
    }

    public void GoToMessages()
    {
        homeScreen.SetActive(false);
        messagesScreen.SetActive(true);
        conversationsScreen.SetActive(false);
        galleryScreen.SetActive(false);
    }

    public void GoToConversation()
    {
        homeScreen.SetActive(false);
        messagesScreen.SetActive(false);
        conversationsScreen.SetActive(true);
        galleryScreen.SetActive(false);
    }

    public void GoToGallery()
    {
        homeScreen.SetActive(false);
        messagesScreen.SetActive(false);
        conversationsScreen.SetActive(false);
        galleryScreen.SetActive(true);
    }
}
