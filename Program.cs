// --- 1. Thêm các thư viện (using) ---
using Microsoft.EntityFrameworkCore;
using STTechUserManagement.Data; 

// --- 2. Khởi tạo WebApplicationBuilder ---
var builder = WebApplication.CreateBuilder(args);

// --- 3. Đăng ký Dịch vụ (Service Configuration) ---

// Thêm dịch vụ DbContext (để kết nối SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Thêm dịch vụ MVC (Controllers và Views)
builder.Services.AddControllersWithViews();

// --- 4. Build ứng dụng ---
var app = builder.Build();

// --- 5. Tự động tạo Database (Chỉ chạy 1 lần khi khởi động) ---
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Lệnh này sẽ tự tạo database và bảng nếu chưa có
    dbContext.Database.EnsureCreated();
}

// --- 6. Cấu hình HTTP Request Pipeline ---

// Cấu hình trang báo lỗi (Rất quan trọng cho lúc lập trình)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days.
    app.UseHsts();
}
else
{
    // Bật trang báo lỗi chi tiết (trang màu đỏ) khi đang phát triển
    app.UseDeveloperExceptionPage(); 
}

// Tự động chuyển hướng sang HTTPS (nếu có)
app.UseHttpsRedirection();

// Cho phép phục vụ các file tĩnh (CSS, JS, hình ảnh...)
app.UseStaticFiles();

// Bật tính năng định tuyến (Routing)
app.UseRouting();

// Bật tính năng xác thực/ủy quyền (nếu có)
app.UseAuthorization();

// --- 7. Định tuyến URL và Chạy ứng dụng ---

// Cấu hình URL mặc định (ví dụ: /Home/Index)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Chạy app
app.Run();