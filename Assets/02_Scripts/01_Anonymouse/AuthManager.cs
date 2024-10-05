using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using Auth = Unity.Services.Authentication.AuthenticationService;

public class AuthManager : MonoBehaviour
{
    [Header("Anonymous UI")]
    [SerializeField] private Button signInButton;
    [SerializeField] private Button signOutButton;
    [SerializeField] private Button playerNameSaveButton;
    [SerializeField] private TMP_InputField playerNameIF;

    [Header("UserName & Password UI")]
    [SerializeField] private Button signUpUserNameButton;
    [SerializeField] private Button signInUserNameButton;
    [SerializeField] private TMP_InputField userNameIF;
    [SerializeField] private TMP_InputField passwordIF;

    private async void Awake()
    {
        // UGS 초기화 완료됐을 때 호출되는 콜백 연결
        UnityServices.Initialized += () =>
        {
            Debug.Log("UGS 초기화 완료");
        };

        // Unity Gaming Service 초기화
        await UnityServices.InitializeAsync();

        BingingEvents();
        BindingUIEvents();
    }

    private void BindingUIEvents()
    {
        #region 익명로그인 바인딩
        // 로그인 버튼 클릭 이벤트 연결
        signInButton.onClick.AddListener(async () =>
        {
            if (!Auth.Instance.IsSignedIn) await SignInAsync();
        });

        // 로그아웃 버튼 클릭 이벤트 연결
        signOutButton.onClick.AddListener(() => Auth.Instance.SignOut());

        // 플레이어 이름 변경 버튼 클릭
        // 50자, 중간에 공백 허용 X, Zack#1234
        playerNameSaveButton.onClick.AddListener(async () =>
        {
            await Auth.Instance.UpdatePlayerNameAsync(playerNameIF.text);

            Debug.Log($"Player Name : {Auth.Instance.PlayerName}");
        });
        #endregion

        #region UserName & Password 바인딩
        // 회원가입 이벤트 연결
        signUpUserNameButton.onClick.AddListener(async () =>
        {
            /*
                UserName  : 대소문자 구별 없다. 3~20자, [. - @] 가능
                Password  : 대소문자 구별 함. 8~30자, 영문자 대문자 1, 소문자 1, 숫자 1, 특수문자 1
            */
            try
            {
                await Auth.Instance.SignUpWithUsernamePasswordAsync(userNameIF.text, passwordIF.text);
            }
            catch (AuthenticationException e)
            {
                Debug.LogError("Auth Error :" + e.Message);
            }
            catch (RequestFailedException e)
            {
                Debug.LogError("Request Error :" + e.Message);
            }
        });
        #endregion
    }

    private void BingingEvents()
    {
        // 익명 로그인 완료됐을 때 호출되는 콜백 연결
        AuthenticationService.Instance.SignedIn += () =>
        {
            // Zack#3855
            Debug.Log("익명 로그인 성공");
            Debug.Log($"Player Id: {Auth.Instance.PlayerId}");
            Debug.Log($"Player Name: {Auth.Instance.PlayerName?.Split('#')[0]}");
        };

        // 익명 로그인 로그아웃 콜백
        Auth.Instance.SignedOut += () =>
        {
            Debug.Log("로그 아웃");
        };

        // 로그인 실패
        Auth.Instance.SignInFailed += (e) =>
        {
            Debug.Log($"로그인 실패 : {e.Message}");
        };

        // 세션 종료시 호출되는 콜백
        Auth.Instance.Expired += () => Debug.Log("세션 종료");

        #region UserName & Password Callbacks
        // 회원가입 성공시 호출되는 콜백

        #endregion
    }

    // 익명 로그인 로직
    private async Task SignInAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (AuthenticationException e)
        {
            Debug.Log(e.Message);
        }
    }
}
