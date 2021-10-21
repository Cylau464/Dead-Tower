using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine;

public class GPServices : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _text;

    public static GPServices Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        // enables saving game progress.
        .EnableSavedGames()
        // requests the email address of the player be available.
        // Will bring up a prompt for consent.
        .RequestEmail()
        // requests a server auth code be generated so it can be passed to an
        //  associated back end server application and exchanged for an OAuth token.
        .RequestServerAuthCode(false)
        // requests an ID token be generated.  This OAuth token can be used to
        //  identify the player to other services such as Firebase.
        .RequestIdToken()
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = false;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
        LogIn();
    }

    private void Update()
    {
        Debug.Log("GP login status " + PlayGamesPlatform.Instance.IsAuthenticated().ToString());
    }

    public void LogIn()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated() == true)
            PlayGamesPlatform.Instance.SignOut();

        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (success) =>
        {
            Debug.Log("GP Services authenticated is " + success);
        });
    }
}
