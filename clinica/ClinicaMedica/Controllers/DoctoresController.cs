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
    public class DoctoresController : Controller
    {
        private readonly ClinicaMedicaDbContext _context;

        public DoctoresController(ClinicaMedicaDbContext context)
        {
            _context = context;
        }

        // GET: Doctores
        public async Task<IActionResult> Index()
        {
           
            return View(await _context.Doctores.ToListAsync());
        }

        // GET: Doctores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctore = await _context.Doctores
                .FirstOrDefaultAsync(m => m.DoctorId == id);
            if (doctore == null)
            {
                return NotFound();
            }

            return View(doctore);
        }

        // GET: Doctores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorId,Nombre,Especialidad,Telefono,Correo")] Doctore doctore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctore);
        }

        // GET: Doctores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctore = await _context.Doctores.FindAsync(id);
            if (doctore == null)
            {
                return NotFound();
            }
            return View(doctore);
        }

        // POST: Doctores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorId,Nombre,Especialidad,Telefono,Correo")] Doctore doctore)
        {
            if (id != doctore.DoctorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctoreExists(doctore.DoctorId))
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
            return View(doctore);
        }

        // GET: Doctores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctore = await _context.Doctores
                .FirstOrDefaultAsync(m => m.DoctorId == id);
            if (doctore == null)
            {
                return NotFound();
            }

            return View(doctore);
        }

        // POST: Doctores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctore = await _context.Doctores.FindAsync(id);
            if (doctore != null)
            {
                _context.Doctores.Remove(doctore);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctoreExists(int id)
        {
            return _context.Doctores.Any(e => e.DoctorId == id);
        }
    }
}
