# SMK Restaurant

Sistem manajemen restoran SMK: aplikasi **WinForms** (Admin & Cashier) + **REST API** (menu), dengan koneksi SQL Server yang terpusat.

---

## Prasyarat

| Komponen | Keterangan |
|----------|------------|
| .NET SDK | **.NET 10** |
| SQL Server | `RUCKIYE\SQLEXPRESS` (Windows Authentication) |
| Database | `SMKRestaurant` |
| IDE (opsional) | Visual Studio 2022 / VS Code / Cursor |

Pastikan SQL Server Express berjalan dan database `SMKRestaurant` sudah dibuat beserta tabelnya (`MsEmployee`, `MsMenu`, `MsMember`, `OrderHeader`, `OrderDetail`, dll.).

---

## Cara Run

### 1. Restore & Build (semua project)

Dari root repo:

```powershell
cd d:\TugasPaDwi\Tugas_Project_PaDwi
dotnet restore SMKRestaurant.slnx
dotnet build SMKRestaurant.slnx
```

### 2. Jalankan WinForms (desktop)

```powershell
dotnet run --project SMKRestaurant\SMKRestaurant.csproj
```

Atau buka `SMKRestaurant.slnx` di Visual Studio → set startup project **SMKRestaurant** → F5.

Aplikasi mulai dari **Login** (`FrmLogin`).

### 3. Jalankan API

```powershell
dotnet run --project SMKRestaurantAPI\SMKRestaurantAPI.csproj
```

API listen di:

```text
http://0.0.0.0:5023
```

Contoh lokal: `http://localhost:5023`

### 4. Uji endpoint Menu

```http
GET http://localhost:5023/menu
Authorization: Bearer test
```

atau:

```http
GET http://localhost:5023/api/menu
Authorization: Bearer test
```

> Header `Authorization` **wajib ada** (isi apa saja). Tanpa header → HTTP **401**.

Contoh dengan curl:

```powershell
curl -H "Authorization: Bearer test" http://localhost:5023/menu
```

---

## Struktur Project

```text
Tugas_Project_PaDwi/
├── README.md
├── SMKRestaurant.slnx              ← Solution (buka ini di IDE)
│
├── SMKRestaurant.Shared/           ← Shared library (DB config)
│   └── Data/
│       ├── DatabaseConfig.cs       ★ SSOT connection string
│       └── DbConnectionFactory.cs
│
├── SMKRestaurant/                  ← WinForms desktop
│   ├── Program.cs
│   ├── Helpers/
│   │   ├── Database.cs             ← facade ke Shared
│   │   └── GlobalExceptionHandler.cs
│   ├── Data/                       ← Repository (SQL)
│   │   ├── EmployeeRepository.cs
│   │   ├── MenuRepository.cs
│   │   ├── MemberRepository.cs
│   │   └── OrderRepository.cs
│   ├── Models/
│   └── Forms/                      ← UI
│       ├── FrmLogin.cs
│       ├── FrmAdmin.cs / FrmCashier.cs
│       ├── FrmMenu.cs / FrmMember.cs
│       └── FrmOrder.cs / FrmPayment.cs
│
└── SMKRestaurantAPI/               ← ASP.NET Core Web API
    ├── Program.cs                  ← DI + middleware
    ├── Controllers/MenuController.cs
    ├── Services/MenuService.cs
    ├── Data/MenuRepository.cs
    ├── Models/                     ← ApiResponse, MenuDto, dll.
    ├── Middleware/ExceptionHandlingMiddleware.cs
    ├── appsettings.json
    └── Properties/launchSettings.json
```

---

## File Penting yang Sering Diubah

### 1. Ganti server / database (paling penting)

**File utama (Single Source of Truth):**

`SMKRestaurant.Shared/Data/DatabaseConfig.cs`

```csharp
public const string Server = @"RUCKIYE\SQLEXPRESS";
public const string DatabaseName = "SMKRestaurant";
public const bool IntegratedSecurity = true;
public const bool Encrypt = true;
public const bool TrustServerCertificate = true;
```

Ubah `Server` / `DatabaseName` di sini. WinForms & API ikut memakai konfigurasi ini lewat `DbConnectionFactory`.

**Disarankan juga selaraskan** (dokumentasi / fallback):

`SMKRestaurantAPI/appsettings.json` → `ConnectionStrings:DefaultConnection`

---

### 2. Port API

`SMKRestaurantAPI/Properties/launchSettings.json`

```json
"applicationUrl": "http://0.0.0.0:5023"
```

Ubah `5023` jika port bentrok.

---

### 3. Logic bisnis / SQL WinForms

| Kebutuhan | File |
|-----------|------|
| Login pegawai | `SMKRestaurant/Data/EmployeeRepository.cs` |
| CRUD Menu | `SMKRestaurant/Data/MenuRepository.cs` |
| CRUD Member | `SMKRestaurant/Data/MemberRepository.cs` |
| Order & Payment | `SMKRestaurant/Data/OrderRepository.cs` |
| UI Login / Order / dll. | `SMKRestaurant/Forms/*.cs` |

> Jangan tulis SQL baru di Form. Taruh di `Data/*Repository.cs`.

---

### 4. Logic / endpoint API

| Kebutuhan | File |
|-----------|------|
| Routing & HTTP | `SMKRestaurantAPI/Controllers/MenuController.cs` |
| Business logic | `SMKRestaurantAPI/Services/MenuService.cs` |
| SQL API | `SMKRestaurantAPI/Data/MenuRepository.cs` |
| Format response sukses | `SMKRestaurantAPI/Models/ApiResponse.cs` |
| Format response error | `SMKRestaurantAPI/Models/ApiErrorResponse.cs` |
| Global error API | `SMKRestaurantAPI/Middleware/ExceptionHandlingMiddleware.cs` |
| Registrasi DI | `SMKRestaurantAPI/Program.cs` |

---

### 5. Error handling WinForms

`SMKRestaurant/Helpers/GlobalExceptionHandler.cs`  
`SMKRestaurant/Program.cs` → `GlobalExceptionHandler.Register()`

Log error tersimpan di:

```text
%LocalAppData%\SMKRestaurant\logs\error-YYYYMMDD.log
```

---

## Alur Aplikasi WinForms

```text
FrmLogin
  ├─ admin   → FrmAdmin → FrmMenu / FrmMember
  └─ cashier → FrmCashier → FrmOrder / FrmPayment
```

Login memakai tabel `MsEmployee` (Email + Password + Position).

---

## Format Response API

### Sukses (`GET /menu`)

```json
{
  "success": true,
  "message": "Menu berhasil diambil.",
  "data": [
    {
      "name": "Nasi Goreng",
      "description": "Carbo : 40, Protein 10",
      "price": 15000
    }
  ],
  "count": 1
}
```

### Error (contoh tanpa Authorization)

```json
{
  "success": false,
  "message": "Authorization required",
  "detail": null,
  "traceId": null
}
```

---

## Arsitektur Singkat

```text
┌─────────────────┐     ┌──────────────────┐
│  SMKRestaurant  │     │ SMKRestaurantAPI │
│    (WinForms)   │     │   (Web API)      │
└────────┬────────┘     └────────┬─────────┘
         │                       │
         │    Forms/Controllers  │
         │           ↓           │
         │    Data / Services    │
         │           ↓           │
         └───────────┼───────────┘
                     ↓
         SMKRestaurant.Shared
         (DatabaseConfig + Factory)
                     ↓
              SQL Server
         RUCKIYE\SQLEXPRESS
           DB: SMKRestaurant
```

---

## Troubleshooting

| Masalah | Solusi |
|---------|--------|
| Gagal konek DB | Cek SQL Server running; pastikan instance `RUCKIYE\SQLEXPRESS`; sesuaikan `DatabaseConfig.cs` |
| Login gagal | Cek data di `MsEmployee` (Email, Password, Position = `admin` / `cashier`) |
| Port 5023 dipakai | Ubah di `launchSettings.json` |
| Build gagal “file locked” | Tutup app WinForms/API yang masih jalan, lalu build ulang |
| API 401 | Tambahkan header `Authorization` |
| API 503 / error DB | Middleware menandai gagal SQL; cek connection string & SQL Server |

---

## Perintah Cepat

```powershell
# Build semua
dotnet build SMKRestaurant.slnx

# Run desktop
dotnet run --project SMKRestaurant\SMKRestaurant.csproj

# Run API
dotnet run --project SMKRestaurantAPI\SMKRestaurantAPI.csproj
```

---

## Catatan Developer

1. **Connection string** hanya diubah di `SMKRestaurant.Shared/Data/DatabaseConfig.cs` (lalu sync `appsettings.json` bila perlu).
2. **Form = UI saja**; SQL ada di folder `Data/`.
3. **API** mengikuti pola: Controller → Service → Repository.
4. Error tidak tertangani di Form/Controller tetap ditangkap **global handler / middleware**.
5. Order ID digenerate di dalam transaksi (`UPDLOCK`) agar aman concurrent — lihat `OrderRepository.SaveOrder`.
