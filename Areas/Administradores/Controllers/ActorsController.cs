﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recetario.Areas.Administradores.Servicios;
using Recetario.Areas.Administradores.Models;

// TODO: Cambiar los nombres de las páginas a español xD
// TODO: Agregar input para confirma contraseña
/*
 1/2   Pantalla principal de Super Administrador
-    Modificar Administrador
1/2    Mostrar Información de Administradores  (Tabla)
-    Registro de Administradores
-    Ver información de Administrador
Eliminar
 */

namespace Recetario.Areas.Administradores.Controllers
{
    [Area("Administradores")]
    public class ActorsController : Controller
    {
        private readonly IActor _serviciosActor;

        public ActorsController(IActor serviciosActor)
        {
            _serviciosActor = serviciosActor;
        }

        // GET: Administradores/Actors
        public IActionResult Index()
        {
            ICollection<VActor> actores = _serviciosActor.Obtener();
            return View(actores);
            //return View(await _context.Actor.ToListAsync());
        }

        
        public IActionResult Detalles(int? id)
        {
            if (id == null)
            {
                // TODO: En sí qué hace NotFound? 
                return NotFound();
            }

            var actor = _serviciosActor.Obtener(id);
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
                //0.-root, 1.-Administrador, 2.-Usuario
                actor.Tipo = 1;
                _serviciosActor.Registrar(actor);
                //Regresar al menú principal
                // TODO: En cuanto haya sesión, tendrá que redireccionar a una acción.
                return View("../Menus/MenuSA");
            }
            return View(actor);
        }

        // GET: Administradores/Actors/Edit/5
        public IActionResult Editar(int? id)
        {
            //Comprobar que en la URL se pase un ID
            if (id == null)
            {
                return NotFound();
            }
            var actor = _serviciosActor.Obtener(id);
            //En caso de que el Actor no exista en la BD
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
        public IActionResult Editar(int id, VActor actor)
        {
            if (id != actor.IdActor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _serviciosActor.Actualizar(actor);
                }
                catch (Exception)
                {
                    return NotFound();
                }
                // TODO: Arreglar lo de las excepciones xD
                /*
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
                */
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        public IActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            VActor actor = _serviciosActor.Obtener(id);
            // TODO: Confrimar que se puede regresar el null
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }
        
        // Se debe especificar el nombre de la acción por que el nombre la firma del anterior método es el mismo
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarConfirmado(int id)
        {
            _serviciosActor.Eliminar(id);
            //Regresar al menú principal
            // TODO: En cuanto haya sesión, tendrá que redireccionar a una acción x2.
            return View("../Menus/MenuSA");
            //return RedirectToAction(nameof(Index));
        }
        
        /*
        private bool ActorExists(int id)
        {
            return _context.Actor.Any(e => e.IdActor == id);
        }
        */
    }
}
