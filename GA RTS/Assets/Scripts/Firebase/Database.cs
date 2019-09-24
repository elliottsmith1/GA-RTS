using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Extensions;

public class Database : MonoBehaviour
{
    private DatabaseReference database;
    private Firebase.Auth.FirebaseUser userInfo;
    private FirebaseApp app;
    private Firebase.Auth.FirebaseAuth auth;

    private bool signedIn = false;
    private long gameNum = 0;    

    // Start is called before the first frame update
    void Start()
    {
        app = FirebaseApp.DefaultInstance;

        app.SetEditorDatabaseUrl("https://rts-flow.firebaseio.com/");
        if (app.Options.DatabaseUrl != null)
            app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);

        database = FirebaseDatabase.DefaultInstance.RootReference;

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;        

        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            signedIn = true;
            userInfo = task.Result;

            FirebaseDatabase.DefaultInstance.GetReference(userInfo.UserId).Child("Games played").GetValueAsync().ContinueWith(task2 =>
            {
                if (task2.IsFaulted)
                {
                    Debug.Log("Error retrieving games played");
                }
                else if (task2.IsCompleted)
                {
                    if (task2.Result.Exists)
                    {
                        gameNum = (long)task2.Result.GetValue(true);
                        gameNum++;
                    }

                    database.Child(userInfo.UserId).Child("Games played").SetValueAsync(gameNum);
                }
            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NewFeedbackData(Random.Range(10.0f, 200.0f), Random.Range(0.0f, 200.0f), Random.Range(0.0f, 10.0f));
        }
    }

    public void NewFeedbackData(float _difficulty, float _skill, float _flow)
    {
        if (!signedIn)
            return;       

        string time = Time.frameCount.ToString();
        string game = gameNum.ToString();

        database.Child(userInfo.UserId).Child(game).Child(time).Child("Difficulty").SetValueAsync(_difficulty).ContinueWith
            (task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Data save encountered an error: " + task.Exception);
                }
            });
        database.Child(userInfo.UserId).Child(game).Child(time).Child("Skill").SetValueAsync(_skill);
        database.Child(userInfo.UserId).Child(game).Child(time).Child("Flow").SetValueAsync(_flow);
    }
}
