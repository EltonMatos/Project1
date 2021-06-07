using System;
using UnityEngine;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        public string menuName;
        public bool isOpen;

        
        //TODO some show issues related to this awake, but if there is any active game object there is a problem when not putting this
        // private void Awake()
        // {
        //     print(menuName + isOpen);
        //     gameObject.SetActive(isOpen)
        // }

        public void Open()
        {            
            gameObject.SetActive(true);
            isOpen = true;
        }

        public void Close()
        {
            gameObject.SetActive(false);
            isOpen = false;
        }
    }
}