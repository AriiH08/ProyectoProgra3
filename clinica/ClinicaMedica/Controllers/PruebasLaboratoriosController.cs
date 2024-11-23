using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicaMedica.Data.Models;
using ClinicaMedica.Permisos;

namespace ClinicaMedica.Controllers
{
    [ValidarSesion]
    public class PruebasLaboratoriosController : Controller
    {
        private readonly ClinicaMedicaDbContext _context;

        public PruebasLaboratoriosController(ClinicaMedicaDbContext context)
        {
            _context = context;
        }

        // GET: PruebasLaboratorios
        public async Task<IActionResult> Index()
        {
            var clinicaMedicaDbContext = _context.PruebasLaboratorios.Include(p => p.Laboratorio).Include(p => p.Paciente);
            return View(await clinicaMedicaDbContext.ToListAsync());
        }

        // GET: PruebasLaboratorios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pruebasLaboratorio = await _context.PruebasLaboratorios
                .Include(p => p.Laboratorio)
                .Include(p => p.Paciente)
                .FirstOrDefaultAsync(m => m.PruebaId == id);
            if (pruebasLaboratorio == null)
            {
                return NotFound();
            }

            return View(pruebasLaboratorio);
        }

        // GET: PruebasLaboratorios/Create
        public IActionResult Create()
        {
            ViewData["LaboratorioId"] = new SelectList(_context.Laboratorios, "LaboratorioId", "LaboratorioId");
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "PacienteId");
            return View();
        }

        // POST: PruebasLaboratorios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PruebaId,PacienteId,LaboratorioId,TipoPrueba,FechaPrueba,Resultado")] PruebasLaboratorio pruebasLaboratorio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pruebasLaboratorio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LaboratorioId"] = new SelectList(_context.Laboratorios, "LaboratorioId", "LaboratorioId", pruebasLaboratorio.LaboratorioId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "PacienteId", pruebasLaboratorio.PacienteId);
            return View(pruebasLaboratorio);
        }

        // GET: PruebasLaboratorios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pruebasLaboratorio = await _context.PruebasLaboratorios.FindAsync(id);
            if (pruebasLaboratorio == null)
            {
                return NotFound();
            }
            ViewData["LaboratorioId"] = new SelectList(_context.Laboratorios, "LaboratorioId", "LaboratorioId", pruebasLaboratorio.LaboratorioId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "PacienteId", pruebasLaboratorio.PacienteId);
            return View(pruebasLaboratorio);
        }

        // POST: PruebasLaboratorios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PruebaId,PacienteId,LaboratorioId,TipoPrueba,FechaPrueba,Resultado")] PruebasLaboratorio pruebasLaboratorio)
        {
            if (id != pruebasLaboratorio.PruebaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pruebasLaboratorio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PruebasLaboratorioExists(pruebasLaboratorio.PruebaId))
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
            ViewData["LaboratorioId"] = new SelectList(_context.Laboratorios, "LaboratorioId", "LaboratorioId", pruebasLaboratorio.LaboratorioId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "PacienteId", pruebasLaboratorio.PacienteId);
            return View(pruebasLaboratorio);
        }

        // GET: PruebasLaboratorios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pruebasLaboratorio = await _context.PruebasLaboratorios
                .Include(p => p.Laboratorio)
                .Include(p => p.Paciente)
                .FirstOrDefaultAsync(m => m.PruebaId == id);
            if (pruebasLaboratorio == null)
            {
                return NotFound();
            }

            return View(pruebasLaboratorio);
        }

        // POST: PruebasLaboratorios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pruebasLaboratorio = await _context.PruebasLaboratorios.FindAsync(id);
            if (pruebasLaboratorio != null)
            {
                _context.PruebasLaboratorios.Remove(pruebasLaboratorio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PruebasLaboratorioExists(int id)
        {
            return _context.PruebasLaboratorios.Any(e => e.PruebaId == id);
        }


        private void PopulateDropdowns(PruebasLaboratorio? pruebas = null)
        {
            ViewData["PruebaId"] = new SelectList(_context.PruebasLaboratorios, "PruebaId", "TipoPrueba", pruebas?.PruebaId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nombre", pruebas?.PacienteId);
            ViewData["LaboratorioId"] = new SelectList(_context.Laboratorios, "LaboratorioId", "Nombre", pruebas?.LaboratorioId);
        }

    }
}
