namespace GlassStoreCore.Helpers
{
    public sealed class Singleton
    {
        public string JwtToken { get; set; }
        private static Singleton instance = null;
        public static Singleton GetInstance
        {
            get
            {
                if (instance == null)
                    instance = new Singleton();
                return instance;
            }
        }

        private Singleton()
        {
        }

    }
}
