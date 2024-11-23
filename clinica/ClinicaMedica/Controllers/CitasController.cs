using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicaMedica.Data.Models;
using ClinicaMedica.Permisos;

namespace ClinicaMedica.Controllers
{
    [ValidarSesion(2)] // Solo usuarios con RolId = 2 (Clientes)
    public class CitasController : Controller
    {
        private readonly ClinicaMedicaDbContext _context;

        public CitasController(ClinicaMedicaDbContext context)
        {
            _context = context;
        }

        // GET: Citas
        public async Task<IActionResult> Index()
        {
            // Obtener el UsuarioId desde la sesión
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            // Filtrar citas solo del usuario autenticado
            var citas = await _context.Citas
                .Where(c => c.UsuarioId == usuarioId)
                .Include(c => c.Doctor)
                .Include(c => c.Paciente)
                .Include(c => c.Personal)
                .ToListAsync();

            // Si no hay citas, mostrar un mensaje en la vista
            ViewData["Mensaje"] = citas.Any() ? null : "No tienes citas agendadas.";

            return View(citas);
        }

        // GET: Citas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest("El ID de la cita no puede ser nulo.");
            }

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            // Buscar la cita solo si pertenece al usuario autenticado
            var cita = await _context.Citas
                .Include(c => c.Doctor)
                .Include(c => c.Paciente)
                .Include(c => c.Personal)
                .FirstOrDefaultAsync(m => m.CitaId == id && m.UsuarioId == usuarioId);

            if (cita == null)
            {
                return NotFound("No se encontró la cita especificada.");
            }

            return View(cita);
        }

        // GET: Citas/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Citas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PacienteId,DoctorId,PersonalId,FechaCita,Estado,MotivoConsulta")] Cita cita)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            // Asociar la cita al usuario autenticado
            cita.UsuarioId = usuarioId.Value;

            if (ModelState.IsValid)
            {
                _context.Add(cita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(cita);
            return View(cita);
        }

        // GET: Citas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest("El ID de la cita no puede ser nulo.");
            }

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            // Buscar la cita solo si pertenece al usuario autenticado
            var cita = await _context.Citas
                .FirstOrDefaultAsync(c => c.CitaId == id && c.UsuarioId == usuarioId);

            if (cita == null)
            {
                return NotFound("No se encontró la cita especificada.");
            }

            PopulateDropdowns(cita);
            return View(cita);
        }

        // POST: Citas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CitaId,PacienteId,DoctorId,PersonalId,FechaCita,Estado,MotivoConsulta")] Cita cita)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            // Validar que la cita pertenece al usuario autenticado
            var citaDb = await _context.Citas
                .FirstOrDefaultAsync(c => c.CitaId == id && c.UsuarioId == usuarioId);

            if (citaDb == null)
            {
                return NotFound("No se encontró la cita especificada para editar.");
            }

            if (ModelState.IsValid)
            {
                citaDb.PacienteId = cita.PacienteId;
                citaDb.DoctorId = cita.DoctorId;
                citaDb.PersonalId = cita.PersonalId;
                citaDb.FechaCita = cita.FechaCita;
                citaDb.Estado = cita.Estado;
                citaDb.MotivoConsulta = cita.MotivoConsulta;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(cita);
            return View(cita);
        }

        // GET: Citas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("El ID de la cita no puede ser nulo.");
            }

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            var cita = await _context.Citas
                .FirstOrDefaultAsync(c => c.CitaId == id && c.UsuarioId == usuarioId);

            if (cita == null)
            {
                return NotFound("No se encontró la cita especificada para eliminar.");
            }

            return View(cita);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            var cita = await _context.Citas
                .FirstOrDefaultAsync(c => c.CitaId == id && c.UsuarioId == usuarioId);

            if (cita != null)
            {
                _context.Citas.Remove(cita);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CitaExists(int id)
        {
            return _context.Citas.Any(e => e.CitaId == id);
        }

        private void PopulateDropdowns(Cita? cita = null)
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctores, "DoctorId", "Nombre", cita?.DoctorId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nombre", cita?.PacienteId);
            ViewData["PersonalId"] = new SelectList(_context.PersonalAdministrativos, "PersonalId", "Nombre", cita?.PersonalId);
        }
    }
}
