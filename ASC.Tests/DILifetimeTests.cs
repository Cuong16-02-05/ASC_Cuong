using ASC.Web.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ASC.Tests
{
    /// <summary>
    /// Unit tests chứng minh Transient / Scoped / Singleton - Lab 1 Section 10
    /// </summary>
    public class DILifetimeTests
    {
        private ServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();
            services.AddTransient<TransientLoggerService>();
            services.AddScoped<ScopedLoggerService>();
            services.AddSingleton<SingletonLoggerService>();
            return services.BuildServiceProvider();
        }

        [Fact]
        public void Transient_Creates_DifferentInstance_EachTime_Test()
        {
            var provider = BuildProvider();
            var t1 = provider.GetRequiredService<TransientLoggerService>();
            var t2 = provider.GetRequiredService<TransientLoggerService>();
            // Transient → mỗi lần resolve là 1 instance mới → ID khác nhau
            Assert.NotEqual(t1.GetOperationId(), t2.GetOperationId());
        }

        [Fact]
        public void Scoped_Returns_SameInstance_WithinScope_Test()
        {
            var provider = BuildProvider();
            using var scope = provider.CreateScope();
            var s1 = scope.ServiceProvider.GetRequiredService<ScopedLoggerService>();
            var s2 = scope.ServiceProvider.GetRequiredService<ScopedLoggerService>();
            // Scoped → cùng scope → cùng instance → ID giống nhau
            Assert.Equal(s1.GetOperationId(), s2.GetOperationId());
        }

        [Fact]
        public void Scoped_Creates_DifferentInstance_AcrossScopes_Test()
        {
            var provider = BuildProvider();
            string id1, id2;
            using (var scope1 = provider.CreateScope())
                id1 = scope1.ServiceProvider.GetRequiredService<ScopedLoggerService>().GetOperationId();
            using (var scope2 = provider.CreateScope())
                id2 = scope2.ServiceProvider.GetRequiredService<ScopedLoggerService>().GetOperationId();
            // Scoped → khác scope → khác instance
            Assert.NotEqual(id1, id2);
        }

        [Fact]
        public void Singleton_Returns_SameInstance_Always_Test()
        {
            var provider = BuildProvider();
            var single1 = provider.GetRequiredService<SingletonLoggerService>();
            string id2;
            using (var scope = provider.CreateScope())
                id2 = scope.ServiceProvider.GetRequiredService<SingletonLoggerService>().GetOperationId();
            // Singleton → luôn cùng 1 instance dù qua scope khác nhau
            Assert.Equal(single1.GetOperationId(), id2);
        }

        [Fact]
        public void Transient_Lifetime_Returns_Correct_Name_Test()
        {
            var t = new TransientLoggerService();
            Assert.Equal("Transient", t.GetLifetime());
        }

        [Fact]
        public void Scoped_Lifetime_Returns_Correct_Name_Test()
        {
            var s = new ScopedLoggerService();
            Assert.Equal("Scoped", s.GetLifetime());
        }

        [Fact]
        public void Singleton_Lifetime_Returns_Correct_Name_Test()
        {
            var s = new SingletonLoggerService();
            Assert.Equal("Singleton", s.GetLifetime());
        }
    }
}
