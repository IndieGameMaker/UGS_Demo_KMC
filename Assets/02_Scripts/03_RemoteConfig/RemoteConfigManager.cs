using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class RemoteConfigManager : MonoBehaviour
{
    private async void Awake()
    {
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        // RemoteConfig 로드 완료 콜백
        RemoteConfigService.Instance.FetchCompleted += (response) =>
        {
            Debug.Log("로드 완료");
            float moveSpeed = RemoteConfigService.Instance.appConfig.GetFloat("move_speed");
            int bodyScale = RemoteConfigService.Instance.appConfig.GetInt("body_scale");

            // Mummy 크기를 변경
            GameObject.Find("Mummy").transform.localScale = Vector3.one * bodyScale;
        };
        await GetRemoteConfigAsync();
    }

    private struct userAttributes { };
    private struct appAttributes { };

    // Remote Config 데이터 로드
    private async Task GetRemoteConfigAsync()
    {
        await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
    }
}
