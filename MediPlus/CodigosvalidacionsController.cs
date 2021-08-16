using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MediPlus.Models;

namespace MediPlus
{
    public class CodigosvalidacionsController : Controller
    {
        private readonly CITASMEDICASDBContext _context;

        public CodigosvalidacionsController(CITASMEDICASDBContext context)
        {
            _context = context;
        }

        // GET: Codigosvalidacions
        public async Task<IActionResult> Index()
        {
            var cITASMEDICASDBContext = _context.Codigosvalidacions.Include(c => c.Usuario);
            return View(await cITASMEDICASDBContext.ToListAsync());
        }

        // GET: Codigosvalidacions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codigosvalidacion = await _context.Codigosvalidacions
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codigosvalidacion == null)
            {
                return NotFound();
            }

            return View(codigosvalidacion);
        }

        // GET: Codigosvalidacions/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Clave");
            return View();
        }

        // POST: Codigosvalidacions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,UsuarioId,Enviado,Fecha")] Codigosvalidacion codigosvalidacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(codigosvalidacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Clave", codigosvalidacion.UsuarioId);
            return View(codigosvalidacion);
        }

        // GET: Codigosvalidacions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codigosvalidacion = await _context.Codigosvalidacions.FindAsync(id);
            if (codigosvalidacion == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Clave", codigosvalidacion.UsuarioId);
            return View(codigosvalidacion);
        }

        // POST: Codigosvalidacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,UsuarioId,Enviado,Fecha")] Codigosvalidacion codigosvalidacion)
        {
            if (id != codigosvalidacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(codigosvalidacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CodigosvalidacionExists(codigosvalidacion.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Clave", codigosvalidacion.UsuarioId);
            return View(codigosvalidacion);
        }

        // GET: Codigosvalidacions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codigosvalidacion = await _context.Codigosvalidacions
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codigosvalidacion == null)
            {
                return NotFound();
            }

            return View(codigosvalidacion);
        }

        // POST: Codigosvalidacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var codigosvalidacion = await _context.Codigosvalidacions.FindAsync(id);
            _context.Codigosvalidacions.Remove(codigosvalidacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CodigosvalidacionExists(int id)
        {
            return _context.Codigosvalidacions.Any(e => e.Id == id);
        }
    }
}
