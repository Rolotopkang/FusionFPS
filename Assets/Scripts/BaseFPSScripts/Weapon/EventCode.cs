namespace Scripts.Weapon
{
    public enum EventCode : byte
    {
        HitObject = 1,
        HitPlayer = 2,
        KillPlayer = 3,
        
        
        
        //ConquestEvent
        ConquestPointOccupied = 101,
        
        
        //GameLoop
        GameBegin,
        GameRestart,
        GameEnd,
    }
}