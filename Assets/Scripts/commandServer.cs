using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class commandServer : MonoBehaviour {

    private SocketIOComponent _socket;
    public Transform _cube;
    private Vector3 cubeVelocity;
    Dictionary<string, string> data = new Dictionary<string, string>();
    JSONObject jdata;

    private float vy = 0f;

    void Start()
    {
        _socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
        Debug.Log(" [-] socket game object is found.");

        _cube.position = new Vector3(0.0f, 5f, 0.0f);

        _socket.On("connect", (SocketIOEvent obj) =>
        {
            Debug.Log("hello socketio~");
        });

        
    }

    private void FixedUpdate()
    {
        //_cube.position = _cube.position + new Vector3(0f, Random.Range(-0.1f, 0.1f), 0f);
        Debug.Log("elapsed time = " + Time.deltaTime);

        data["y"] = _cube.position[1].ToString("N4");
        jdata = new JSONObject(data);

        _socket.Emit("test", jdata);
        _socket.On("speed", getSpeed);
        _cube.position = _cube.position + new Vector3(0f, vy, 0f) + new Vector3(0f, Random.Range(-0.1f, 0.1f), 0f);
    }

    void getSpeed(SocketIOEvent obj)
    {
        JSONObject jspeed = obj.data;
        vy = float.Parse(jspeed.GetField("vy").str) * 0.02f;
    }
}
