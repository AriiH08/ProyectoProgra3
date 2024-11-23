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
    public class PersonalAdministrativoesController : Controller
    {
        private readonly ClinicaMedicaDbContext _context;

        public PersonalAdministrativoesController(ClinicaMedicaDbContext context)
        {
            _context = context;
        }

        // GET: PersonalAdministrativoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PersonalAdministrativos.ToListAsync());
        }

        // GET: PersonalAdministrativoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalAdministrativo = await _context.PersonalAdministrativos
                .FirstOrDefaultAsync(m => m.PersonalId == id);
            if (personalAdministrativo == null)
            {
                return NotFound();
            }

            return View(personalAdministrativo);
        }

        // GET: PersonalAdministrativoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PersonalAdministrativoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonalId,Nombre,Cargo,Telefono,Correo")] PersonalAdministrativo personalAdministrativo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personalAdministrativo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(personalAdministrativo);
        }

        // GET: PersonalAdministrativoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalAdministrativo = await _context.PersonalAdministrativos.FindAsync(id);
            if (personalAdministrativo == null)
            {
                return NotFound();
            }
            return View(personalAdministrativo);
        }

        // POST: PersonalAdministrativoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonalId,Nombre,Cargo,Telefono,Correo")] PersonalAdministrativo personalAdministrativo)
        {
            if (id != personalAdministrativo.PersonalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personalAdministrativo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonalAdministrativoExists(personalAdministrativo.PersonalId))
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
            return View(personalAdministrativo);
        }

        // GET: PersonalAdministrativoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalAdministrativo = await _context.PersonalAdministrativos
                .FirstOrDefaultAsync(m => m.PersonalId == id);
            if (personalAdministrativo == null)
            {
                return NotFound();
            }

            return View(personalAdministrativo);
        }

        // POST: PersonalAdministrativoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personalAdministrativo = await _context.PersonalAdministrativos.FindAsync(id);
            if (personalAdministrativo != null)
            {
                _context.PersonalAdministrativos.Remove(personalAdministrativo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonalAdministrativoExists(int id)
        {
            return _context.PersonalAdministrativos.Any(e => e.PersonalId == id);
        }
    }
}
