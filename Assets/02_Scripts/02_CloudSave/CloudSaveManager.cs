using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using Auth = Unity.Services.Authentication.AuthenticationService;

[System.Serializable]
public struct PlayerData
{
    public string name;
    public int level;
    public int xp;
    public int gold;

    public List<ItemData> items;
}

[System.Serializable]
public struct ItemData
{
    public string name;
    public int count;
    public string icon;
}

public class CloudSaveManager : MonoBehaviour
{
    public Button singleDataSaveButton;
    public Button multiDataSaveButton;

    public Button singleDataLoadButton;
    public Button multiDataLoadButton;

    public PlayerData playerData;

    private async void Awake()
    {
        // 유니티 서비스 초기화
        await UnityServices.InitializeAsync();

        // 로그인 성공시 이벤트 연결
        Auth.Instance.SignedIn += () => Debug.Log(Auth.Instance.PlayerId);

        // 익명 로그인
        await Auth.Instance.SignInAnonymouslyAsync();

        // 저장 버튼 이벤트 연결
        singleDataSaveButton.onClick.AddListener(async () => await SaveSingleDataAsync());
        multiDataSaveButton.onClick.AddListener(async () =>
        {
            await SaveMultiDataAsync<PlayerData>("player_data", playerData);
        });

        // 로드 버튼 이벤트 연결
        singleDataLoadButton.onClick.AddListener(async () => await LoadData());
        multiDataLoadButton.onClick.AddListener(async () => await LoadData<PlayerData>("player_data"));
    }

    #region 싱글 데이터 저장
    private async Task SaveSingleDataAsync()
    {
        // Dictionary<키, 값>
        // 저장할 데이터 생성
        var data = new Dictionary<string, object>
        {
            {"player_name", "Zack"},
            {"level", 30},
            {"xp", 2000},
            {"gold", 100}
        };

        // 저장 로직
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        Debug.Log("싱글 데이터 저장 완료");
    }
    #endregion

    #region 멀티 데이터 저장
    private async Task SaveMultiDataAsync<T>(string key, T saveData)
    {
        var data = new Dictionary<string, object>
        {
            {key, saveData}
        };

        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        Debug.Log("멀티 데이터 저장 완료");

        // 데이터 삭제
        playerData = new PlayerData();
    }
    #endregion

    /*
        HashSet

        HashSet<int> playerId = new HashSet<int>();
        playerId.Add(1);
        playerId.Add(2);
        playerId.Add(1); 
    */

    private async Task LoadData()
    {
        // 조회하려는 Key 설정
        var keys = new HashSet<string>
        {
            "player_name", "level", "xp", "gold"
        };

        var data = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

        if (data.TryGetValue("player_name", out var playerName))
        {
            Debug.Log("Player Name :" + playerName.Value.GetAs<string>());
        }
        if (data.TryGetValue("level", out var level))
        {
            Debug.Log($"Level : {level.Value.GetAs<int>()}");
        }
    }

    // 멀티 데이터 로드
    private async Task LoadData<T>(string key)
    {
        var loadData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });

        if (loadData.TryGetValue(key, out var data))
        {
            playerData = data.Value.GetAs<PlayerData>();
        }
    }

}
