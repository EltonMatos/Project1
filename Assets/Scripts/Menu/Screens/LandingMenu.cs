using Network;
using TMPro;

namespace Menu.Screens
{
    public class LandingMenu : Menu
    {
        //TODO duplicate nickname should not be allowed
        public void ConnectToLobby(TMP_InputField nicknameInput)
        {
            GameConnection.Instance.ConnectToLobby(nicknameInput.text);
        }
    }
}