using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using Auth = Unity.Services.Authentication.AuthenticationService;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private Button signInButton;

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
        // 버튼 클릭 이벤트 연결
        signInButton.onClick.AddListener(async () =>
        {
            await SignInAsync();
        });
    }

    private void BingingEvents()
    {
        // 익명 로그인 완료됐을 때 호출되는 콜백 연결
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("익명 로그인 성공");
            Debug.Log($"Player Id: {Auth.Instance.PlayerId}");
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
