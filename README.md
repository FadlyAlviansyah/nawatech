# NawatechApp - ASP.NET Core Web App

Aplikasi web berbasis ASP.NET Core MVC yang menyediakan sistem autentikasi, profil pengguna, serta dashboard dengan statistik produk dan kategori.

## 🧩 Fitur Aplikasi

- 🔐 **Autentikasi Login/Logout**
- 📝 **Registrasi Pengguna Baru**
- 🧑 **Manajemen Profil** (nama, email, password, foto profil)
- 📊 **Dashboard**: Menampilkan total kategori dan total produk (yang tidak dihapus)
- 📁 **Upload Gambar Profil**
- 🧮 **Password Hashing**
- 🧰 **Clean MVC Architecture**

## 🛠️ Teknologi yang Digunakan

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- HTML, CSS (Bootstrap)
- C#

## 📁 Struktur Proyek

```
NawatechApp/
│
├── Controllers/
│   ├── AuthController.cs
│   └── DashboardController.cs
│   └── HomeController.cs
│   └── ProductCategoryController.cs
│   └── ProductController.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Models/
│   ├── ErrorViewModel.cs
│   └── Product.cs
│   └── ProductCategory.cs
│   └── User.cs
│
├── Views/
│   ├── Auth/
│   │   ├── Login.cshtml
│   │   ├── Register.cshtml
│   │   └── Profile.cshtml
│   ├── Dashboard/
│   │   └── Index.cshtml
│   ├── Home/
│   │   └── Privacy.cshtml
│   ├── Product/
│   │   └── Create.cshtml
│   │   └── Edit.cshtml
│   │   └── Index.cshtml
│   ├── ProductCategory/
│   │   └── Create.cshtml
│   │   └── Edit.cshtml
│   │   └── Index.cshtml
│   └── Shared/
│       └── _Layout.cshtml
│
├── wwwroot/
│   └── css/
│   │   └── site.css
│   ├── images/
│   │   └── products/
│   │   └── users/
│   ├── js/
│   │   └── site.js
│
└── Program.cs
```

## ▶️ Cara Menjalankan

1. **Clone Repository**
   ```bash
   git clone https://github.com/username/nawatech.git
   cd nawatech
   ```

2. **Restore & Build**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **Atur Database & Jalankan Migrasi**
   ```bash
   dotnet ef database update
   ```

4. **Jalankan Aplikasi**
   ```bash
   dotnet run
   ```

5. **Buka di Browser**
   ```
   http://localhost:5000
   ```

## 🤝 Kontribusi

Pull request, masukan, atau perbaikan bug sangat diterima.

