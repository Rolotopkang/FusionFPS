namespace Scripts.Items
{
    public class FirearmsItem : BaseItem
    {
        public enum FirearmsType
        {
            AssultRefile,
            HandGun,
        }

        public FirearmsType CurrentFirearmsType;
        public string ArmsName;
    }
}