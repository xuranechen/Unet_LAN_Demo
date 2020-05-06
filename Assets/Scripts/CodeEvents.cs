using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeEvents : MonoBehaviour
{
    [SerializeField]
    private GameObject box;

    private static CodeEvents Inst;
    public static CodeEvents Instance
    {
        get { return Inst; }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Inst == null)
        {
            Inst = this;
        }
    }

    public void SendMessage(string code)
    {
        MessageManager.Instance.CmdSendMessage(code);
    }

    public void ReceiveMessage(string code)
    {
        if (code == "画面描述")
        {
            box.SetActive(false);
        }
    }
}
