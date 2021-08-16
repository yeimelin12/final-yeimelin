using MediPlus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace MediPlus.Controllers
{
    public class CitasController : Controller
    {
        private readonly CITASMEDICASDBContext _context;

        public CitasController(CITASMEDICASDBContext context)
        {
            _context = context;

        }

        // GET: Citas
        public async Task<IActionResult> Index()
        {
            if (!LocalData.LocalData.IsLogin)
            {
                return RedirectToAction("Login", "Usuarios");
            }

            if (LocalData.LocalData.Usuario.Rol.Nombre == "Secretaria")
            {
                var cITASMEDICASDBContext = _context.Citas
                                .Include(c => c.IdMedicoNavigation)
                                .Include(c => c.IdPacientesNavigation)
                                .Where(m => m.IdMedico == LocalData.LocalData.Usuario.Doctor.Id);
                return View(await cITASMEDICASDBContext.ToListAsync());
            }

            else if (LocalData.LocalData.Usuario.Rol.Nombre == "Medico")
            {
                var cITASMEDICASDBContext = _context.Citas
                                .Include(c => c.IdMedicoNavigation)
                                .Include(c => c.IdPacientesNavigation)
                                .Where(m => m.IdMedico == LocalData.LocalData.Usuario.Id);
                return View(await cITASMEDICASDBContext.ToListAsync());
            }

            else
            {
                var cITASMEDICASDBContext = _context.Citas
                                .Include(c => c.IdMedicoNavigation)
                                .Include(c => c.IdPacientesNavigation);
                return View(await cITASMEDICASDBContext.ToListAsync());
            }


        }

        public IActionResult CitasHoy()
        {
            var result = _context.Citas
                .Where(
                e => e.FechaCita.Day == DateTime.Now.Day &&
                e.FechaCita.Month == DateTime.Now.Month &&
                e.FechaCita.Year == DateTime.Now.Year
                );
            return View(result);
        }

        public IActionResult MisCitasHoy()
        {
            var result = _context.Citas.Include(e => e.IdPacientesNavigation)
                .Where(
                e => e.FechaCita.Day == DateTime.Now.Day &&
                e.FechaCita.Month == DateTime.Now.Month &&
                e.FechaCita.Year == DateTime.Now.Year &&
                e.IdPacientes == LocalData.LocalData.Usuario.Id
                ).ToList();
            return View(result);
        }

        public IActionResult MisCitas()
        {
            var result = _context.Citas.Include(e => e.IdPacientesNavigation)
                .Where(
                e => e.IdPacientes == LocalData.LocalData.Usuario.Id
                ).ToList();
            return View(result);
        }


        // GET: Citas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .Include(c => c.IdMedicoNavigation)
                .Include(c => c.IdPacientesNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // GET: Citas/Create
        public IActionResult Create()
        {
            if (LocalData.LocalData.Usuario.Rol.Nombre == "Medico")
            {
                ViewData["IdMedico"] = new SelectList(_context.Medicos.Where(e => e.Id == LocalData.LocalData.Usuario.Doctor.Id).ToList(), "Id", "Nombre");
            }
            else
            {
                ViewData["IdMedico"] = new SelectList(_context.Medicos.ToList(), "Id", "Nombre");

            }

            ViewData["IdPacientes"] = new SelectList(_context.Pacientes, "Id", "Nombres");
            return View();
        }

        // POST: Citas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdPacientes,IdMedico,MotivoDeConsulta,FechaCita,Comentario,Estado")] Cita cita)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cita);
                var paciente = _context.Pacientes.Include(u => u.IdUsuarioNavigation).FirstOrDefault(e => e.Id == cita.IdPacientes);
                var medico = _context.Medicos.FirstOrDefault(e => e.Id == cita.IdMedico);


                try
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("yeimelin3@gmail.com");
                        mail.To.Add(paciente.IdUsuarioNavigation.Email);
                        mail.Subject = "MediPlus - Cita Medica";
                        mail.Body = "" +
                            "<b> Hola " + paciente.Nombres + " " + paciente.Apellidos + ", se ha registrado su cita medica en MediPlus</b><br><br>" +
                            "<b>Doctor: </b>: " + medico.Nombre + " " + medico.Apellidos + "<br>" +
                            "<b>Oficina: </b>: " + medico.Oficina + "<br>" +
                            "<b>Fecha: </b>: " + cita.FechaCita + "<br>" +
                            "<b>Motivo: </b>: " + cita.MotivoDeConsulta + "<br>" +
                            "<b>Comentarios: </b>: " + cita.Comentario + "<br>" +
                            "<b>Estado: </b>: " + cita.Estado +
                            "";
                        mail.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("yeimelin3@gmail.com", "558293YE");
                            smtp.EnableSsl = true;
                            smtp.Send(mail);

                        }

                        await _context.SaveChangesAsync();
                    }
                }

                catch (SmtpException ex)
                {
                    throw new ApplicationException
                      ("SmtpException has occured: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }



                return RedirectToAction(nameof(Index));
            }
            ViewData["IdMedico"] = new SelectList(_context.Medicos, "Id", "Nombre", cita.IdMedico);
            ViewData["IdPacientes"] = new SelectList(_context.Pacientes, "Id", "Nombres", cita.IdPacientes);
            return View(cita);
        }

        // GET: Citas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }
            ViewData["IdMedico"] = new SelectList(_context.Medicos, "Id", "Nombre", cita.IdMedico);
            ViewData["IdPacientes"] = new SelectList(_context.Pacientes, "Id", "Nombres", cita.IdPacientes);
            return View(cita);
        }

        public async Task<IActionResult> Cancelar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }
            ViewData["IdMedico"] = new SelectList(_context.Medicos, "Id", "Nombre", cita.IdMedico);
            ViewData["IdPacientes"] = new SelectList(_context.Pacientes, "Id", "Nombres", cita.IdPacientes);
            return View(cita);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelar(int id, [Bind("Id,IdPacientes,IdMedico,MotivoDeConsulta,FechaCita,Comentario,Estado")] Cita cita)
        {
            if (id != cita.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    cita.Estado = "Cancelada";
                    _context.Update(cita);

                    var paciente = _context.Pacientes.Include(u => u.IdUsuarioNavigation).FirstOrDefault(e => e.Id == cita.IdPacientes);
                    var medico = _context.Medicos.FirstOrDefault(e => e.Id == cita.IdMedico);


                    try
                    {
                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("yeimelin3@gmail.com");
                            mail.To.Add(paciente.IdUsuarioNavigation.Email);
                            mail.Subject = "MediPlus - Cancelación de Cita Medica";
                            mail.Body = "" +
                                "<b> Hola " + paciente.Nombres + " " + paciente.Apellidos + ", se ha cancelado su cita medica en MediPlus</b><br><br>" +
                                "<b>Doctor: </b> " + medico.Nombre + " " + medico.Apellidos + "<br>" +
                                "<b>Oficina: </b> " + medico.Oficina + "<br>" +
                                "<b>Fecha: </b> " + cita.FechaCita + "<br>" +
                                "<b>Motivo: </b> " + cita.MotivoDeConsulta + "<br>" +
                                "<b>Comentarios </b>: " + cita.Comentario + "<br>" +
                                "<b>Estado: </b> " + cita.Estado +
                                "";
                            mail.IsBodyHtml = true;

                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.UseDefaultCredentials = false;
                                smtp.Credentials = new System.Net.NetworkCredential("yeimelin3@gmail.com", "558293YE");
                                smtp.EnableSsl = true;
                                smtp.Send(mail);

                            }

                            await _context.SaveChangesAsync();
                            //_context.Citas.Remove(cita);
                            //await _context.SaveChangesAsync();

                        }
                    }

                    catch (SmtpException ex)
                    {
                        throw new ApplicationException
                          ("SmtpException has occured: " + ex.Message);
                    }


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaExists(cita.Id))
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
            ViewData["IdMedico"] = new SelectList(_context.Medicos, "Id", "Nombre", cita.IdMedico);
            ViewData["IdPacientes"] = new SelectList(_context.Pacientes, "Id", "Nombres", cita.IdPacientes);
            return View(cita);
        }

        // POST: Citas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdPacientes,IdMedico,MotivoDeConsulta,FechaCita,Comentario,Estado")] Cita cita)
        {
            if (id != cita.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cita);

                    var paciente = _context.Pacientes.Include(u => u.IdUsuarioNavigation).FirstOrDefault(e => e.Id == cita.IdPacientes);
                    var medico = _context.Medicos.FirstOrDefault(e => e.Id == cita.IdMedico);


                    try
                    {
                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("yeimelin3@gmail.com");
                            mail.To.Add(paciente.IdUsuarioNavigation.Email);
                            mail.Subject = "MediPlus - Actualización de Cita Medica";
                            mail.Body = "" +
                                "<b> Hola " + paciente.Nombres + " " + paciente.Apellidos + ", se ha actualizado su cita medica en MediPlus</b><br><br>" +
                                "<b>Doctor: </b> " + medico.Nombre + " " + medico.Apellidos + "<br>" +
                                "<b>Oficina: </b> " + medico.Oficina + "<br>" +
                                "<b>Fecha: </b> " + cita.FechaCita + "<br>" +
                                "<b>Motivo: </b> " + cita.MotivoDeConsulta + "<br>" +
                                "<b>Comentarios </b>: " + cita.Comentario + "<br>" +
                                "<b>Estado: </b> " + cita.Estado +
                                "";
                            mail.IsBodyHtml = true;

                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.UseDefaultCredentials = false;
                                smtp.Credentials = new System.Net.NetworkCredential("yeimelin3@gmail.com", "558293YE");
                                smtp.EnableSsl = true;
                                smtp.Send(mail);

                            }

                            await _context.SaveChangesAsync();
                        }
                    }

                    catch (SmtpException ex)
                    {
                        throw new ApplicationException
                          ("SmtpException has occured: " + ex.Message);
                    }


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaExists(cita.Id))
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
            ViewData["IdMedico"] = new SelectList(_context.Medicos, "Id", "Nombre", cita.IdMedico);
            ViewData["IdPacientes"] = new SelectList(_context.Pacientes, "Id", "Nombres", cita.IdPacientes);
            return View(cita);
        }

        // GET: Citas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .Include(c => c.IdMedicoNavigation)
                .Include(c => c.IdPacientesNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitaExists(int id)
        {
            return _context.Citas.Any(e => e.Id == id);
        }
    }
}
