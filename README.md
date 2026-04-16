# Automobile Service Center (ASC)
## ASP.NET Core 8 MVC — Full Lab Solution (Lab 1–4)

---

## Cấu trúc Solution

```
ASC.sln
├── ASC.Model          ← Lab 2: BaseEntity, ServiceRequest, MasterData, Product, ApplicationUser
├── ASC.DataAccess     ← Lab 2: IRepository<T>, Repository<T>, IUnitOfWork, UnitOfWork
├── ASC.Business       ← Lab 2: IServiceRequestOperations, IMasterDataOperations
├── ASC.Utilities      ← Lab 3+4: SessionExtensions, CurrentUser, ClaimsPrincipalExtensions
├── ASC.Web            ← Lab 1+4: MVC App, Identity, Areas, Navigation, Email
│   ├── Areas/
│   │   ├── Identity/          ← Lab 4: Login, Register, ForgotPassword, ResetPassword
│   │   ├── ServiceRequests/   ← Lab 4: Dashboard, ServiceRequest CRUD
│   │   ├── Accounts/          ← Lab 4: Customers, Engineers, Profile
│   │   └── Configuration/     ← Lab 2: MasterData Keys & Values
│   ├── Controllers/           ← Lab 1: HomeController (DI demo), BaseController, AnonymousController
│   ├── Services/              ← Lab 1: TransientLoggerService, ScopedLoggerService, SingletonLoggerService
│   │                            Lab 4: AuthMessageSender (email)
│   ├── Infrastructure/        ← Lab 4: NavigationCacheOperations
│   ├── ViewComponents/        ← Lab 4: LeftNavigationViewComponent
│   ├── Migrations/            ← Lab 2: InitialCreate + AddProduct
│   └── wwwroot/               ← Lab 1: CSS (Materialize), JS, images
└── ASC.Tests          ← Lab 3: xUnit + Moq + FakeSession (30+ test cases)
    ├── HomeControllerTests.cs
    ├── DILifetimeTests.cs      ← chứng minh Transient/Scoped/Singleton
    ├── ServiceRequestTests.cs
    ├── SessionExtensionsTests.cs
    ├── MasterDataTests.cs
    ├── ProductTests.cs         ← Lab 2 step 6
    ├── ApplicationUserTests.cs
    └── Mocks/FakeSession.cs    ← Lab 3 key deliverable
```

---

## Yêu cầu cài đặt

- **Visual Studio 2022** (ASP.NET & Web Development workload)
- **.NET 8 SDK**
- **SQL Server LocalDB** (cài kèm VS 2022)

---

## Hướng dẫn chạy (5 bước)

### Bước 1 — Mở Solution
```
File → Open → Project/Solution → chọn ASC.sln
```

### Bước 2 — Restore NuGet Packages
```
Chuột phải vào Solution → Restore NuGet Packages
```

### Bước 3 — Tạo Database
Vào **Tools → NuGet Package Manager → Package Manager Console**, chọn Default project = `ASC.Web`:
```powershell
Update-Database
```
> Migration đã có sẵn (`InitialCreate` + `AddProduct`), chỉ cần `Update-Database` là tạo xong DB.

### Bước 4 — Chạy ứng dụng
```
Nhấn F5 → chọn ASC.Web
```
Khi chạy lần đầu, app tự động seed tài khoản Admin và Engineer.

### Bước 5 — Chạy Unit Tests
```
Test → Run All Tests   (30+ test cases — Lab 3)
```

---

## Tài khoản mặc định

| Role     | Email                  | Password         |
|----------|------------------------|------------------|
| Admin    | admin@asc.com          | Admin@123456     |
| Engineer | engineer@asc.com       | Engineer@123456  |
| User     | tự đăng ký tại /Identity/Account/Register | — |

---

## Hướng dẫn chứng minh từng Lab

### ✅ Lab 1 — Thiết kế & Khởi tạo + Dependency Injection
1. Chạy app → vào `/` → thấy trang Home với **3 icon block** (Who we are, Competency, Easy to work with)
2. Mở `appsettings.json` → đổi `Title` → reload → navbar thay đổi theo (**IOptions DI hoạt động**)
3. Vào `/Home/DIDemo` → thấy bảng chứng minh **Transient ≠ Scoped = Singleton =**
4. Trong Test Explorer chạy `DILifetimeTests` → tất cả xanh lá

### ✅ Lab 2 — Domain-Driven Architecture + Repository + UnitOfWork
1. Package Manager Console → `Update-Database` → mở **SQL Server Object Explorer**
2. Show DB `ASCDb` đã tạo bảng: `ServiceRequests`, `MasterDataKeys`, `MasterDataValues`, **`Products`**
3. Mở **Solution Explorer** → show đúng 5 project + 1 (ASC.Tests) với đúng project references
4. Show code `IRepository<T>`, `UnitOfWork`, `BaseEntity` → giải thích layered architecture

### ✅ Lab 3 — Test-Driven Architecture (TDD)
1. `Test → Run All Tests` → show **Test Explorer xanh lá** (30+ tests pass)
2. Mở `FakeSession.cs` → giải thích: "đây là fake ISession để test không phụ thuộc infrastructure"
3. Show `HomeControllerTests.cs` → naming convention `ControllerName_Action_Condition_Test`
4. Show `DILifetimeTests.cs` → test dùng `IServiceCollection` thật để prove DI lifetime

### ✅ Lab 4 — ASP.NET Identity + OAuth + Navigation
1. Chạy app → vào `/ServiceRequests/Dashboard/Dashboard` khi **chưa login** → redirect về Login
2. Login `admin@asc.com` → menu trái có **User Administration, Master Data**
3. Login `engineer@asc.com` → menu trái **không có** User Administration (navigation động theo role)
4. Click **Logout** → redirect về Login, session cleared
5. Thử **Forgot Password** → nhập email → show màn hình confirmation

---

## Ghi chú kỹ thuật

- **Materialize CSS** dùng qua CDN (không cần tải về)
- **Email** (ForgotPassword/ResetPassword): cấu hình SMTP trong `appsettings.json` → mục `Smtp`
- **Google OAuth**: có thể bổ sung sau bằng package `Microsoft.AspNetCore.Authentication.Google`
- **CI/CD** (Lab 1 Section 6): đẩy code lên GitHub → có thể tích hợp GitHub Actions thay Travis CI
