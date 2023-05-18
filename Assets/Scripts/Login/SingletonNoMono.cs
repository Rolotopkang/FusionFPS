namespace UnityTemplateProjects.Login
{
    public class SingletonNoMono<T> where T : new()
    {
        static T instance;
 
        public static T GetInstance()
        {
            if (instance == null)
            { 
                instance = new T();
            }
            return instance;
        }

        
    }
}