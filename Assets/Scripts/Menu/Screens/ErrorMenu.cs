using System;
using Network;
using Photon.Realtime;
using TMPro;

namespace Menu.Screens
{
    public class ErrorMenu : Menu
    {
        
        public TMP_Text errorText;

        private void OnEnable()
        {
            ErrorInfo error = GameConnection.Instance.PhotonErrorInfo;
            if (error == null) return;
            
            errorText.text +=  "\n" + error.Info;
        }
    }
}