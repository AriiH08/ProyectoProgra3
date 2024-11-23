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
    public class RecetasController : Controller
    {
        private readonly ClinicaMedicaDbContext _context;

        public RecetasController(ClinicaMedicaDbContext context)
        {
            _context = context;
        }

        // GET: Recetas
        public async Task<IActionResult> Index()
        {
            var clinicaMedicaDbContext = _context.Recetas.Include(r => r.Cita).Include(r => r.Medicamento);
            return View(await clinicaMedicaDbContext.ToListAsync());
        }

        // GET: Recetas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Recetas
                .Include(r => r.Cita)
                .Include(r => r.Medicamento)
                .FirstOrDefaultAsync(m => m.RecetaId == id);
            if (receta == null)
            {
                return NotFound();
            }

            return View(receta);
        }

        // GET: Recetas/Create
        public IActionResult Create()
        {
            ViewData["CitaId"] = new SelectList(_context.Citas, "CitaId", "CitaId");
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "MedicamentoId", "MedicamentoId");
            return View();
        }

        // POST: Recetas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecetaId,CitaId,MedicamentoId,Cantidad")] Receta receta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CitaId"] = new SelectList(_context.Citas, "CitaId", "CitaId", receta.CitaId);
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "MedicamentoId", "MedicamentoId", receta.MedicamentoId);
            return View(receta);
        }

        // GET: Recetas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Recetas.FindAsync(id);
            if (receta == null)
            {
                return NotFound();
            }
            ViewData["CitaId"] = new SelectList(_context.Citas, "CitaId", "CitaId", receta.CitaId);
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "MedicamentoId", "MedicamentoId", receta.MedicamentoId);
            return View(receta);
        }

        // POST: Recetas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecetaId,CitaId,MedicamentoId,Cantidad")] Receta receta)
        {
            if (id != receta.RecetaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecetaExists(receta.RecetaId))
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
            ViewData["CitaId"] = new SelectList(_context.Citas, "CitaId", "CitaId", receta.CitaId);
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "MedicamentoId", "MedicamentoId", receta.MedicamentoId);
            return View(receta);
        }

        // GET: Recetas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Recetas
                .Include(r => r.Cita)
                .Include(r => r.Medicamento)
                .FirstOrDefaultAsync(m => m.RecetaId == id);
            if (receta == null)
            {
                return NotFound();
            }

            return View(receta);
        }

        // POST: Recetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receta = await _context.Recetas.FindAsync(id);
            if (receta != null)
            {
                _context.Recetas.Remove(receta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecetaExists(int id)
        {
            return _context.Recetas.Any(e => e.RecetaId == id);
        }
    }
}
