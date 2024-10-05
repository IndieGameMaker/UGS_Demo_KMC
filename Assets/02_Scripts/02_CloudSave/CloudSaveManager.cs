using Unity.Services.Core;
using UnityEngine;
using Auth = Unity.Services.Authentication.AuthenticationService;

public class CloudSaveManager : MonoBehaviour
{
    private async void Awake()
    {
        // 유니티 서비스 초기화
        await UnityServices.InitializeAsync();

        // 로그인 성공시 이벤트 연결
        Auth.Instance.SignedIn += () => Debug.Log(Auth.Instance.PlayerId);

        // 익명 로그인
        await Auth.Instance.SignInAnonymouslyAsync();
    }
}
