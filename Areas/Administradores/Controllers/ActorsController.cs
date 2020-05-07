using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recetario.Areas.Administradores.Servicios;
using Recetario.BaseDatos;
using Recetario.Areas.Administradores.Models;

namespace Recetario.Areas.Administradores.Controllers
{
    [Area("Administradores")]
    public class ActorsController : Controller
    {
        private readonly ContextoBD _context;
        private readonly IActor _serviciosActor;

        public ActorsController(ContextoBD context, IActor serviciosActor)
        {
            _context = context;
            _serviciosActor = serviciosActor;
        }

        // GET: Administradores/Actors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Actor.ToListAsync());
        }

        // GET: Administradores/Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor
                .FirstOrDefaultAsync(m => m.IdActor == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // GET: Administradores/Actors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administradores/Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VActor actor)
        {
            if (ModelState.IsValid)
            {
                //Establecer que es un administrador
                actor.Tipo = 1;
                _serviciosActor.RegistrarActor(actor);
                return View("../Menus/MenuSA");
            }
            return View(actor);
        }

        // GET: Administradores/Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        // POST: Administradores/Actors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdActor,NombreActor,FechaNac,Tipo,Usuario,Contrasena,Email")] Actor actor)
        {
            if (id != actor.IdActor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.IdActor))
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
            return View(actor);
        }

        // GET: Administradores/Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor
                .FirstOrDefaultAsync(m => m.IdActor == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: Administradores/Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actor.FindAsync(id);
            _context.Actor.Remove(actor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
            return _context.Actor.Any(e => e.IdActor == id);
        }
    }
}
