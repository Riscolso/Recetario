using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recetario.Areas.Usuarios.Models;
using Recetario.BaseDatos;

namespace Recetario.Areas.Usuarios
{
    [Area("Usuarios")]
    public class RecetasController : Controller
    {
        private readonly ContextoBD _context;

        public RecetasController(ContextoBD context)
        {
            _context = context;
        }

        // GET: Usuarios/Recetas
        public async Task<IActionResult> Index()
        {
            var contextoBD = _context.Receta.Include(r => r.ActorIdActorNavigation);
            return View(await contextoBD.ToListAsync());
        }

        // GET: Usuarios/Recetas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Receta
                .Include(r => r.ActorIdActorNavigation)
                .FirstOrDefaultAsync(m => m.IdReceta == id);
            if (receta == null)
            {
                return NotFound();
            }

            return View(receta);
        }

        
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Usuarios/Recetas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(RecetaDTO receta)
        {
            if (ModelState.IsValid)
            {
                

            }
            //ViewData["ActorIdActor"] = new SelectList(_context.Actor, "Id", "NombreActor", receta.ActorIdActor);
            return View(receta);
        }

        // GET: Usuarios/Recetas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Receta.FindAsync(id);
            if (receta == null)
            {
                return NotFound();
            }
            ViewData["ActorIdActor"] = new SelectList(_context.Actor, "Id", "NombreActor", receta.ActorIdActor);
            return View(receta);
        }

        // POST: Usuarios/Recetas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReceta,Nombre,ProcentajePromedio,TiempoPrep,ActorIdActor")] Receta receta)
        {
            if (id != receta.IdReceta)
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
                    if (!RecetaExists(receta.IdReceta))
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
            ViewData["ActorIdActor"] = new SelectList(_context.Actor, "Id", "NombreActor", receta.ActorIdActor);
            return View(receta);
        }

        // GET: Usuarios/Recetas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Receta
                .Include(r => r.ActorIdActorNavigation)
                .FirstOrDefaultAsync(m => m.IdReceta == id);
            if (receta == null)
            {
                return NotFound();
            }

            return View(receta);
        }

        // POST: Usuarios/Recetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receta = await _context.Receta.FindAsync(id);
            _context.Receta.Remove(receta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecetaExists(int id)
        {
            return _context.Receta.Any(e => e.IdReceta == id);
        }
    }
}
