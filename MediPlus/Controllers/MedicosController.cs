using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MediPlus.Models;

namespace MediPlus.Controllers
{
    public class MedicosController : Controller
    {
        private readonly CITASMEDICASDBContext _context;

        public MedicosController(CITASMEDICASDBContext context)
        {
            _context = context;
        }

        // GET: Medicos
        public async Task<IActionResult> Index()
        {
            if (!LocalData.LocalData.IsLogin)
            {
                return RedirectToAction("Login", "Usuarios");
            }
            var cITASMEDICASDBContext = _context.Medicos.Include(m => m.EspecialidadNavigation).Include(m => m.IdUsuarioNavigation);
            return View(await cITASMEDICASDBContext.ToListAsync());
        }

        public async Task<IActionResult> MiSecretaria()
        {
            if (!LocalData.LocalData.IsLogin)
            {
                return RedirectToAction("Login", "Usuarios");
            }

            var id = LocalData.LocalData.Usuario.Id;

            if (id == null)
            {
                return NotFound();
            }

            var secretarium = await _context.Secretaria
                .Include(s => s.IdDoctorNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdDoctorNavigation.Id == id);
            if (secretarium == null)
            {
                return NotFound();
            }

            return View(secretarium);
        }


        // GET: Medicos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos
                .Include(m => m.EspecialidadNavigation)
                .Include(m => m.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // GET: Medicos/Create
        public IActionResult Create()
        {
            ViewData["Especialidad"] = new SelectList(_context.Especialidades, "Id", "Nombre");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Email");
            return View();
        }

        // POST: Medicos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellidos,Especialidad,Oficina,Telefono,IdUsuario")] Medico medico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Especialidad"] = new SelectList(_context.Especialidades, "Id", "Nombre", medico.Especialidad);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Email", medico.IdUsuario);
            return View(medico);
        }

        // GET: Medicos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }
            ViewData["Especialidad"] = new SelectList(_context.Especialidades, "Id", "Nombre", medico.Especialidad);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Email", medico.IdUsuario);
            return View(medico);
        }

        // POST: Medicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellidos,Especialidad,Oficina,Telefono,IdUsuario")] Medico medico)
        {
            if (id != medico.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicoExists(medico.Id))
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
            ViewData["Especialidad"] = new SelectList(_context.Especialidades, "Id", "Nombre", medico.Especialidad);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Email", medico.IdUsuario);
            return View(medico);
        }

        // GET: Medicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos
                .Include(m => m.EspecialidadNavigation)
                .Include(m => m.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // POST: Medicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicoExists(int id)
        {
            return _context.Medicos.Any(e => e.Id == id);
        }
    }
}
