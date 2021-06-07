using System;
using UnityEngine;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance;
        
        public Menu[] menus;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            //TODO may need to change to open only one menu
            foreach (Menu menu in menus)
            {
                if (menu.isOpen)
                {
                    menu.Open();
                }
            }
        }

        public void OpenMenu(string menuName)
        {
            foreach (Menu menu in menus)
            {
                if (menu.menuName == menuName)
                {
                    menu.Open();
                } else if (menu.isOpen)
                {
                    menu.Close();
                }
            }
        }
        
        public void OpenMenu(Menu menuParam)
        {
            foreach (Menu menu in menus)
            {
                if (menu == menuParam)
                {
                    menu.Open();
                } else if (menu.isOpen)
                {
                    menu.Close();
                }
            }
            menuParam.Open();
        }

        public void CloseMenu(Menu menu)
        {
            menu.Close();
        }
    }
}