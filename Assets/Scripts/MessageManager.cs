using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageManager : NetworkBehaviour
{
    private static MessageManager Inst;
    public static MessageManager Instance
    {
        get { return Inst; }
    }
    string code_Str;
    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            Inst = this;
        }
    }
    [Command]
    public void CmdSendMessage(string code)
    {
        code_Str = code;
        RpcReceiveMessage(code_Str);
    }
    [ClientRpc]
    public void RpcReceiveMessage(string code)
    {
        CodeEvents.Instance.ReceiveMessage(code);
    }
    
}
