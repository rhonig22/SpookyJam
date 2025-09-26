using UnityEngine;

public class WishlistButton : MonoBehaviour
{
    public void AddToWishlist()
    {
        if (SteamManager.Initialized)
        {
            Application.OpenURL("steam://openurl/https://store.steampowered.com/app/3866090/Flipping_Phantom/");
        }
        else
        {

            Application.OpenURL("https://store.steampowered.com/app/3866090/Flipping_Phantom/");
        }
    }
}