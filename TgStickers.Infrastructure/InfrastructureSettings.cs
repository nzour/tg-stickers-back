namespace TgStickers.Infrastructure
{
    public class InfrastructureSettings
    {
        public NHibernateSettings NHibernateSettings { get; set; } = new NHibernateSettings();
        public JwtSettings JwtSettings { get; set; } = new JwtSettings();
        public TelegramSettings TelegramSettings { get; set; } = new TelegramSettings();
    }

    public class NHibernateSettings
    {
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; } = 0;
        public string Database { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string ConnectionString => $"Server={Server};Port={Port};Database={Database};User Id={User};Password={Password}";
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public uint TokenTtl { get; set; } = 0;
    }

    public class TelegramSettings
    {
        public string BotToken { get; set; } = string.Empty;
        public string DirectoryToSaveImages { get; set; } = string.Empty;
    }
}
