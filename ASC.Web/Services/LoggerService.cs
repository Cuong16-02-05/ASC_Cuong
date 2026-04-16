namespace ASC.Web.Services
{
    // ── Lab 1 - Section 10: Dependency Injection demo ──────────────────
    // Ba lifetime khác nhau để minh hoạ Transient / Scoped / Singleton

    public interface ILoggerService
    {
        string GetOperationId();
        string GetLifetime();
    }

    /// <summary>
    /// Transient: tạo instance mới MỖI LẦN được inject.
    /// Dùng cho: stateless services, nhẹ, không chia sẻ state.
    /// </summary>
    public class TransientLoggerService : ILoggerService
    {
        private readonly string _id = Guid.NewGuid().ToString("N")[..8];
        public string GetOperationId() => _id;
        public string GetLifetime() => "Transient";
    }

    /// <summary>
    /// Scoped: tạo 1 instance DUY NHẤT per HTTP request.
    /// Dùng cho: DbContext, Unit of Work - mỗi request có 1 context riêng.
    /// </summary>
    public class ScopedLoggerService : ILoggerService
    {
        private readonly string _id = Guid.NewGuid().ToString("N")[..8];
        public string GetOperationId() => _id;
        public string GetLifetime() => "Scoped";
    }

    /// <summary>
    /// Singleton: tạo 1 instance DUY NHẤT cho toàn bộ vòng đời app.
    /// Dùng cho: caching, config, NavigationCacheOperations.
    /// </summary>
    public class SingletonLoggerService : ILoggerService
    {
        private readonly string _id = Guid.NewGuid().ToString("N")[..8];
        public string GetOperationId() => _id;
        public string GetLifetime() => "Singleton";
    }
}
