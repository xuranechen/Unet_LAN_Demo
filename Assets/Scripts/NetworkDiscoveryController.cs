using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkDiscoveryController : NetworkDiscovery
{
    //是否收到广播
    public bool receivedBroadcast;
    //控制广播发送的频率(毫秒)
    private int BroadcastInterval = 1000;
    //发送广播设备的IP地址
    public string ServerIp { get; private set; }
    Ping ping;

    private void Start()
    {
        if (NetworkManager.singleton == null)
        {
            Debug.Log("需要 NetworkManager组件");
            return;
        }
        //初始化NetworkDiscovery
        Initialize();
        //作为客户端监听广播
        StartAsClient();
        //监听计时，超时则作为服务器
        StartCoroutine("WiatForConnect");
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        //收到广播，关闭监听计时
        StopCoroutine("WiatForConnect");
        //从广播中接收ip数据，如果已经接收过了就不需要接收了
        if (receivedBroadcast)
        {
            return;
        }
        receivedBroadcast = true;
        //把广播设备的IP保存
        ServerIp = fromAddress.Substring(fromAddress.LastIndexOf(':') + 1);
        //设置NetworkManager的IP
        NetworkManager.singleton.networkAddress = ServerIp;
        //自己作为一个客户端连接上服务器IP
        NetworkManager.singleton.StartClient();
    }

    //首先做为客户端接收广播数据，若没有接收到则自己作为服务器发起广播
    private IEnumerator WiatForConnect()
    {
        yield return new WaitForSeconds(3f);
        if (!receivedBroadcast)
        {
            StartAsServerBroadcast();
        }
    }

    private void StartAsServerBroadcast()
    {
        //如果接收到了广播，说明已经有设备先广播了并作为了服务器，就接收广播，并连接
        if (receivedBroadcast)
        {
            return;
        }
        //如果没有接收到了广播，说明还没有设备创建服务器
        StopBroadcast();//停止接听/发送广播
                        //开启本机服务器（也可以作为客户端）
        NetworkManager.singleton.StartHost();
        Initialize();
        //开始发送广播
        StartAsServer();
    }
}