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
    public class SecretariumsController : Controller
    {
        private readonly CITASMEDICASDBContext _context;

        public SecretariumsController(CITASMEDICASDBContext context)
        {
            _context = context;
        }

        // GET: Secretariums
        public async Task<IActionResult> Index()
        {
            if (!LocalData.LocalData.IsLogin)
            {
                return RedirectToAction("Login", "Usuarios");
            }
            var cITASMEDICASDBContext = _context.Secretaria.Include(s => s.IdDoctorNavigation).Include(s => s.IdUsuarioNavigation);
            return View(await cITASMEDICASDBContext.ToListAsync());
        }

        // GET: Secretariums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secretarium = await _context.Secretaria
                .Include(s => s.IdDoctorNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (secretarium == null)
            {
                return NotFound();
            }

            return View(secretarium);
        }

        // GET: Secretariums/Create
        public IActionResult Create()
        {
            ViewData["IdDoctor"] = new SelectList(_context.Medicos, "Id", "Nombre");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Email");
            return View();
        }

        // POST: Secretariums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellidos,Oficina,Telefono,IdUsuario,IdDoctor")] Secretarium secretarium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(secretarium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDoctor"] = new SelectList(_context.Medicos, "Id", "Nombre", secretarium.IdDoctor);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Email", secretarium.IdUsuario);
            return View(secretarium);
        }

        // GET: Secretariums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secretarium = await _context.Secretaria.FindAsync(id);
            if (secretarium == null)
            {
                return NotFound();
            }
            ViewData["IdDoctor"] = new SelectList(_context.Medicos, "Id", "Nombre", secretarium.IdDoctor);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Email", secretarium.IdUsuario);
            return View(secretarium);
        }

        // POST: Secretariums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellidos,Oficina,Telefono,IdUsuario,IdDoctor")] Secretarium secretarium)
        {
            if (id != secretarium.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(secretarium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecretariumExists(secretarium.Id))
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
            ViewData["IdDoctor"] = new SelectList(_context.Medicos, "Id", "Nombre", secretarium.IdDoctor);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Email", secretarium.IdUsuario);
            return View(secretarium);
        }

        // GET: Secretariums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secretarium = await _context.Secretaria
                .Include(s => s.IdDoctorNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (secretarium == null)
            {
                return NotFound();
            }

            return View(secretarium);
        }

        // POST: Secretariums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var secretarium = await _context.Secretaria.FindAsync(id);
            _context.Secretaria.Remove(secretarium);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SecretariumExists(int id)
        {
            return _context.Secretaria.Any(e => e.Id == id);
        }
    }
}
