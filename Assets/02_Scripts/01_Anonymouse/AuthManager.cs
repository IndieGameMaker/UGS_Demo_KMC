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
    }
}
