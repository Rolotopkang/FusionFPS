namespace UnityTemplateProjects.DBServer.NetMsg
{
    public enum NetCode : int
    {
        None = 0,
        UserLogin = 1001,
        UserRegister = 1002,
        VersionControl = 1003,
        UserMoneyGet = 1051,
        UserMoneyAdd = 1052,
        AnnouncementGet = 1100
    }
}