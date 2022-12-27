using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PMS.Models;
using PMS.Models.Context;

namespace PMS.Controllers
{
    [Authorize]
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public RoomsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment hostEnvironment)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this._context = context;
            this.webHostEnvironment = hostEnvironment;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var UserId = signInManager.Context.User.Claims.FirstOrDefault().Value;
            var User = userManager.Users.FirstOrDefault(p=>p.Id.Equals(Convert.ToInt32(UserId)));
            if (User.UserType.Equals("Student"))
            {
                List<RoomViewModel> list = new List<RoomViewModel>();
                var studentRoomIds =  _context.StudentRooms.Where(p => p.StudentId.Equals(Convert.ToInt32(UserId))).Select(p => p.RoomId).ToList();
                var rooms = _context.Rooms.Where(p => studentRoomIds.Contains(p.Id)).ToList();
                rooms.ForEach(p =>
                {
                    RoomViewModel roomVM = new RoomViewModel();
                    roomVM.Id = p.Id;
                    roomVM.ProjectName = p.ProjectName;
                    roomVM.StudentId = Convert.ToInt32(UserId);
                    roomVM.StudentName = User.Name;
                    roomVM.IsCompleted = p.IsCompleted;
                    roomVM.ProjectType = p.ProjectType;
                    roomVM.Year = p.Year;
                    roomVM.CreatedOn = p.CreatedOn;
                    list.Add(roomVM);
                });
                return View(list);
            }
            else
            {
                var Ids = await _context.Rooms.Where(p => p.CreatedBy.Equals(Convert.ToInt32(UserId))).Select(p=>p.Id).ToListAsync();
                var teacherStudents = _context.StudentRooms.Where(p => Ids.Contains(p.RoomId)).ToList();
                List<RoomViewModel> list = new List<RoomViewModel>();
                teacherStudents.ForEach(p =>
                {
                    var room = _context.Rooms.Find(p.RoomId);
                    var student = userManager.Users.FirstOrDefault(pp => pp.Id.Equals(p.StudentId));
                    RoomViewModel roomVM = new RoomViewModel();
                    roomVM.Id = p.RoomId;
                    roomVM.StudentRoomId = p.Id;
                    roomVM.ProjectName = room.ProjectName;
                    roomVM.StudentId = p.StudentId;
                    roomVM.StudentName = student.Name;
                    roomVM.IsCompleted = room.IsCompleted;
                    roomVM.ProjectType = room.ProjectType;
                    roomVM.Year = room.Year;
                    roomVM.CreatedOn = room.CreatedOn;
                    list.Add(roomVM);
                });
                return View(list);
            }
            
        }

        // GET: Rooms/Details/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            var stdIds = _context.StudentRooms.Where(p => p.RoomId.Equals(id)).Select(p => p.StudentId).ToList();
            ViewBag.StudentsList = this.userManager.Users.Where(p => stdIds.Contains(p.Id)).ToList();
            return View(room);
        }

        // GET: Rooms/Create
        [Authorize(Roles ="Teacher")]
        public IActionResult Create()
        {
            Room room = new Room();
            var Students = userManager.Users.Where(p=>p.UserType.Equals("Student")).ToList();
            room.drpStudents = Students.Select(x => new SelectListItem { Text = x.Name+" | "+x.Email, Value = x.Id.ToString() }).ToList();

            return View(room);
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public async Task<IActionResult> Create(Room room)
        {
            var Students = userManager.Users.Where(p => p.UserType.Equals("Student")).ToList();
            room.drpStudents = Students.Select(x => new SelectListItem { Text = x.Name + " | " + x.Email, Value = x.Id.ToString() }).ToList();
            
            if (ModelState.IsValid)
            {
                var UserId = signInManager.Context.User.Claims.FirstOrDefault().Value;
                room.CreatedOn = DateTime.Now;
                room.CreatedBy = Convert.ToInt32(UserId);
                _context.Add(room);
                await _context.SaveChangesAsync();
                foreach(var stdId in room.StudentIds)
                {
                    StudentRoom studentRoom = new StudentRoom();
                    studentRoom.StudentId = Convert.ToInt32(stdId);
                    studentRoom.RoomId = room.Id;
                    _context.Add(studentRoom);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectName,Year,ProjectType,IsCompleted,CreatedOn")] Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
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
            return View(room);
        }

        // GET: Rooms/Delete/5
        [Authorize(Roles = "Teacher")]
        public IActionResult Delete(int? id)
        {
            ViewBag.Id = id;
            return View();
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.StudentRooms.FindAsync(id);
            _context.StudentRooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
        public ActionResult RoomDetail(int RoomId,int StudentId,bool isSave=false)
        {
            var UserId = signInManager.Context.User.Claims.FirstOrDefault().Value;
            var user = userManager.Users.FirstOrDefault(p => p.Id.Equals(Convert.ToInt32(UserId)));
            ViewBag.UserId = user.Name;
            ViewBag.RoomId = RoomId;
            ViewBag.StudentId = StudentId;
            ViewBag.Message = "";
            if (isSave)
            {
                ViewBag.Message = "File(s) data updated Successfully!";
            }
            ViewBag.RoomFiles = _context.RoomFiles.Where(p => p.RoomId.Equals(RoomId) && p.StudentId.Equals(StudentId)).ToList();
            ViewBag.RoomTasks=_context.ProjectTasks.Where(p => p.RoomId.Equals(RoomId) && p.StudentId.Equals(StudentId)).ToList();
            return View();
        }
        public ActionResult UploadFiles(int RoomId,int StudentId, List<IFormFile> files)
        {
            var room = _context.Rooms.Find(RoomId);
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot\\RoomFiles");

                    if (!System.IO.Directory.Exists(filePath))
                    {
                        System.IO.Directory.CreateDirectory(filePath); //Create directory if it doesn't exist
                    }

                    var roomFolder = filePath + "\\"+StudentId+"\\" + room.Id + "-" + room.ProjectName;
                    if (!System.IO.Directory.Exists(roomFolder))
                    {
                        System.IO.Directory.CreateDirectory(roomFolder); //Create directory if it doesn't exist
                    }
                    var fileNameWithPath = string.Concat(roomFolder, "\\", formFile.FileName);
                    if (!System.IO.File.Exists(fileNameWithPath))
                    {
                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            formFile.CopyTo(stream);
                        }
                        var ind = fileNameWithPath.LastIndexOf("RoomFiles");
                        var path = fileNameWithPath.Substring(ind);
                        RoomFile roomFile = new RoomFile();
                        roomFile.FileName = formFile.FileName;
                        roomFile.RoomId = RoomId;
                        roomFile.StudentId = StudentId;
                        roomFile.FilePth = path;
                        _context.Add(roomFile);
                        _context.SaveChanges();
                    }
                }
            }
            return RedirectToAction("RoomDetail",new { RoomId = RoomId,StudentId=StudentId, isSave=true});
        }
        [HttpPost]
        public ActionResult DeleteFile(int FileId,int RoomId,int StudentId)
        {
            var file = _context.RoomFiles.Find(FileId);
            _context.RoomFiles.Remove(file);
            _context.SaveChanges();
            return RedirectToAction("RoomDetail", new { RoomId = RoomId, StudentId = StudentId, isSave = true });
        }
        [HttpPost]
        public ActionResult CreateTask(ProjectTask model)
        {
            _context.Add(model);
            _context.SaveChanges();
            return RedirectToAction("RoomDetail", new
            {
                RoomId = model.RoomId,
                StudentId = model.StudentId
            });
        }
        [HttpGet]
        public ActionResult EditTask(int id)
        {
            var task = _context.ProjectTasks.Find(id);
            return View(task);
        }
        [HttpPost]
        public ActionResult EditTask(ProjectTask model)
        {
            _context.Update(model);
            _context.SaveChanges();
            return RedirectToAction("RoomDetail", new { RoomId = model.RoomId, StudentId = model.StudentId });
        }
    }
}
