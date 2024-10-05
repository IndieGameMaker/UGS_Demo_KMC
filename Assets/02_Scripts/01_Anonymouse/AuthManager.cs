using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

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

        // 버튼 클릭 이벤트 연결
        signInButton.onClick.AddListener(() =>
        {

        });
    }

    // 익명 로그인 로직
    private async Task SignInAsync()
    {
        try
        {
            // 로직
        }
        catch (AuthenticationException e)
        {
            Debug.Log(e.Message);
        }
    }
}
