using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.CloudSave;
using System;

public class AuthManager : MonoBehaviour
{
    private static AuthManager _instance;

    public static Action<int, string> OnSigninFailed;
    public static Action OnSigninSuccess;
    public static AuthManager Instance
    {
        get
        {
            return _instance;
        }
    }

    internal async Task Awake()
    {
        if (_instance == null || _instance != this)
            _instance = this;
        await UnityServices.InitializeAsync();
        await SignInAnonymously();
    }

    private async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            var playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log("PLAYER ID :: " + playerId);
            OnSigninSuccess?.Invoke();
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            Debug.Log("Signin failed : " + s.Message);
            OnSigninFailed?.Invoke(s.HResult, s.Message);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}
