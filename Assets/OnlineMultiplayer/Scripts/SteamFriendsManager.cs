using UnityEngine;
using Steamworks;
using UnityEngine.UI;

public class SteamFriendsManager : MonoBehaviour
{
    
    public RawImage pp;
    public Text playerName;

    public Transform friendsContent;
    public GameObject friendObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        if (!SteamClient.IsValid) 
        {
            Debug.Log("Steamclient is not valid");
            return;
        }

        playerName.text = SteamClient.Name;
        InitFriends();
        var img = await SteamFriends.GetLargeAvatarAsync(SteamClient.SteamId);
        pp.texture = GetTextureFromImage(img.Value);
    }

    public static Texture2D GetTextureFromImage(Steamworks.Data.Image image)
    {
        var texture = new Texture2D((int)image.Width, (int)image.Height); 

        for (int x = 0; x < image.Width; x ++)
        {
            for (int y = 0; y < image.Height; y ++) 
            {
                var pixel = image.GetPixel(x, y);
                texture.SetPixel(x, (int)image.Height - y, new Color(pixel.r/255.0f, pixel.g / 255.0f, pixel.b / 255.0f, pixel.a / 255.0f));
            }
        }
        texture.Apply();
        return texture;
    }


    public async void InitFriends()
    {

        int yvalue = -25;
        

        foreach (var friend in SteamFriends.GetFriends())
        {
            Debug.Log(
                $"{friend.Name}({friend.Id}) | online: {friend.IsOnline} | playing this game: {friend.IsPlayingThisGame}"
            );

            GameObject f = Instantiate(friendObj, friendsContent);
            var img = await SteamFriends.GetLargeAvatarAsync(friend.Id);
            f.GetComponentInChildren<RawImage>().texture = GetTextureFromImage(img.Value);
            f.GetComponentInChildren<Text>().text = friend.Name;
            f.GetComponent<FriendObject>().steamid = friend.Id;
            var rectTransform = f.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2 (125, yvalue);
            yvalue -= 50;
        }
    }

}
