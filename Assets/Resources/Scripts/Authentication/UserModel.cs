using System;
using UnityEngine;

[System.Serializable]
public class UserModel : MonoBehaviour
{
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
}