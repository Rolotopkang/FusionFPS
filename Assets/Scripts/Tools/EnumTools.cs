using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumTools
{
    public enum LoginState
    {
        Success,
        SearchNoUser,
        WrongPassword,
        Error
    }

    public enum RegisterState
    {
        Success,
        HasNoInput,
        RepeatName,
        Error
    }
    
    public enum HitKinds
    {
        normal,
        headShot,
        killShot
    }
}
