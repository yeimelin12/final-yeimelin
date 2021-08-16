using MediPlus.Models;
using MediPlus.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MediPlus.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly CITASMEDICASDBContext _context;

        public UsuariosController(CITASMEDICASDBContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostLogin([Bind("Email,Pass")] LoginVM lg)
        {
            {
                var user = await _context.Usuarios
                    .Where(u => u.Email.ToLower().Equals(lg.Email.ToLower())
                    && u.Clave.ToLower().Equals(lg.Pass.ToLower()))
                    .Include(u => u.Rol)
                    .Include(u => u.Secretaria)
                    .Include(u => u.Medicos)
                    .Include(u => u.Pacientes)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    LocalData.LocalData.Usuario = new UsuarioVM();
                    LocalData.LocalData.Usuario.Rol = new RolVM();
                    LocalData.LocalData.Usuario.Doctor = new DoctorVM();
                    LocalData.LocalData.Usuario.Id = user.Id;


                    if (user.Rol.Nombre == "Medico")
                    {
                        var medico = user.Medicos.FirstOrDefault();
                        LocalData.LocalData.Usuario.Nombre = medico.Nombre;
                        LocalData.LocalData.Usuario.Apellido = medico.Apellidos;
                        LocalData.LocalData.Usuario.Rol.Nombre = user.Rol.Nombre;
                        LocalData.LocalData.Usuario.Doctor.Nombre = medico.Nombre;
                        LocalData.LocalData.Usuario.Doctor.Id = medico.Id;

                    }

                    if (user.Rol.Nombre == "Secretaria")
                    {
                        var secretaria = user.Secretaria.FirstOrDefault();
                        var medico = _context.Medicos.FirstOrDefault(e => e.Id == secretaria.IdDoctor);
                        LocalData.LocalData.Usuario.Nombre = secretaria.Nombre;
                        LocalData.LocalData.Usuario.Apellido = secretaria.Apellidos;
                        LocalData.LocalData.Usuario.Rol.Nombre = user.Rol.Nombre;
                        LocalData.LocalData.Usuario.Doctor.Nombre = medico.Nombre;
                        LocalData.LocalData.Usuario.Doctor.Id = medico.Id;


                    }

                    if (user.Rol.Nombre == "Administrador")
                    {
                        LocalData.LocalData.Usuario.Nombre = user.Email;
                        LocalData.LocalData.Usuario.Rol.Nombre = user.Rol.Nombre;
                        LocalData.LocalData.Usuario.Id = user.Id;
                    }

                    if (user.Rol.Nombre == "Paciente")
                    {
                        var paciente = _context.Pacientes.Include(p=> p.IdUsuarioNavigation).FirstOrDefault(e => e.IdUsuarioNavigation.Email == user.Email);
                        LocalData.LocalData.Usuario.Nombre = paciente.Nombres;
                        LocalData.LocalData.Usuario.Apellido = paciente.Apellidos;
                        LocalData.LocalData.Usuario.Rol.Nombre = user.Rol.Nombre;
                        LocalData.LocalData.Usuario.Id = user.Id;
                    }

                    LocalData.LocalData.IsLogin = true;

                    return RedirectToAction("Index", "Home", LocalData.LocalData.Usuario);

                    //ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
                }
                else
                {
                    return RedirectToAction("Index", "Pacientes");
                }
            }
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var cITASMEDICASDBContext = _context.Usuarios.Include(u => u.Rol);
            return View(await cITASMEDICASDBContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            if (LocalData.LocalData.Usuario.Rol.Nombre == "Secretaria")
            {
                ViewData["RolId"] = new SelectList(_context.Roles.Where(e=> e.Nombre == "Paciente"), "Id", "Nombre");
            }
            else
            {
                ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre");
            }
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Clave,RolId,Confirmado")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Clave,RolId,Confirmado")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
