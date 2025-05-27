**Cara Menjalankan Aplikasi Website - .NET Core 8**

**1. Clone dan Buka Project**
   Clone repository ini ke komputer Anda, lalu buka project menggunakan Visual Studio.
   **Cara membuka project:** Buka Visual Studio -> Pilih menu File > Open -> Arahkan ke folder project dan pilih file dengan ekstensi .csproj

**2. Restore Dependencies**
   Pastikan semua dependency terinstal.
   **Cara:** Buka menu Tools > Command Line > Developer Command Prompt -> Jalankan perintah "dotnet restore"

**3. Konfigurasi Database (SQL Server)**
   Pastikan SQL Server sudah terinstall dan berjalan. Lalu ubah koneksi database di file appsettings.json sesuai konfigurasi SQL Server Anda:
   "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=nama_database;User Id=nama_user;Password=password_anda;"
   }

**4. Lakukan Migration dan Update Database**
   Pastikan Anda telah menginstall EF Core CLI. Jika belum, jalankan "dotnet tool install --global dotnet-ef". Setelah itu, buka terminal atau command prompt di direktori project Anda, lalu jalankan perintah berikut untuk membuat migration dan memperbarui database: "dotnet ef migrations add InitialCreate" lalu "dotnet ef database update"
   
**5. Jalankan Aplikasi**
   Klik tombol Run (ikon play) di bagian atas Visual Studio untuk menjalankan aplikasi
