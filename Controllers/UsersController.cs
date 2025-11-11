using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STTechUserManagement.Data; 
using STTechUserManagement.Models; 
using X.PagedList; 

namespace STTechUserManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users (Hiển thị danh sách CÓ PHÂN TRANG)
        public async Task<IActionResult> Index(int? page)
        {
            int pageNumber = page ?? 1; 
            int pageSize = 10; 

            var users = await _context.Users
                .OrderBy(u => u.Ma)
                .ToPagedListAsync(pageNumber, pageSize);
            
            return View(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Ma == id);
            
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create (ĐÃ SỬA: Hiển thị form VỚI MÃ GỢI Ý)
        public async Task<IActionResult> Create()
        {
            string newMa = "NSST0001"; // Mã mặc định

            // Logic: Chỉ tìm mã "NSST" cuối cùng để gợi ý
            var lastNsstUser = await _context.Users
                .Where(u => u.Ma.StartsWith("NSST")) // Chỉ tìm các mã bắt đầu bằng "NSST"
                .OrderByDescending(u => u.Ma)       // Sắp xếp chúng
                .FirstOrDefaultAsync();             // Lấy cái cuối cùng
            
            if (lastNsstUser != null)
            {
                // lastNsstUser.Ma bây giờ chắc chắn là "NSSTxxxx"
                if (int.TryParse(lastNsstUser.Ma.AsSpan(4), out int num))
                {
                    num++; // Tăng lên 1
                    newMa = "NSST" + num.ToString("D4"); // Format "0002", "0003"...
                }
            }

            var newUser = new User { Ma = newMa }; 
            return View(newUser); // Trả về View với mã gợi ý đúng
        }

        // POST: Users/Create (ĐÃ SỬA: Xử lý mã do người dùng nhập)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ma,HoTen,NgayThangNamSinh,GioiTinh,Email,DienThoai,DiaChi")] User user)
        {
            // Thêm 1 bước kiểm tra: Mã này đã tồn tại chưa?
            if (await _context.Users.AnyAsync(u => u.Ma == user.Ma))
            {
                // Nếu tồn tại, trả về lỗi
                ModelState.AddModelError("Ma", "Mã người dùng này đã tồn tại. Vui lòng chọn mã khác.");
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            // Nếu có lỗi (trùng mã, thiếu tên...), trả lại form cho người dùng sửa
            return View(user);
        }

        // GET: Users/Edit/5 (Hiển thị form sửa)
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5 (Xử lý sửa)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Ma,HoTen,NgayThangNamSinh,GioiTinh,Email,DienThoai,DiaChi")] User user)
        {
            if (id != user.Ma)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Ma)) 
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5 (Hiển thị form xác nhận xóa)
        public async Task<IActionResult> Delete(string id)