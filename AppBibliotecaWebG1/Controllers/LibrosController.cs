using Microsoft.AspNetCore.Mvc;
//Se referencia el  ORM
using AppBibliotecaWebG1.Models;
using Microsoft.EntityFrameworkCore;

namespace AppBibliotecaWebG1.Controllers
{
    public class LibrosController : Controller
    {
        //Variable que permite manejar la referencia del contexto
        private readonly DbContextBiblioteca _context = null;

        /// <summary>
        /// Constructor con parámetros recibe la referencia del ORM
        /// </summary>
        /// <param name="context"></param>
        public LibrosController(DbContextBiblioteca context)
        {
            _context = context; //se asigna la referencia del contexto 
        }


        public async Task<IActionResult> Index()
        {
            //se declara la variable lista
            //por medio del ORM se lee la información de todos los libros en la db
            var listado = await _context.Libros.ToListAsync();

            return View(listado);//Se envia el listado al front-end
        }

        //Métodos encargado para almacenar un libro
        [HttpGet]
        public IActionResult Create() //Método encargado de mostrar el front-end para crear un libro
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind]Libro libro)
        {
            if(libro == null) //se valida que el objeto libro tenga datos
            {
                return View(); //Como no hay datos dejamos al usuario dentro del formulario create
            }
            else
            {  //si hay datos almacenamos el libro
                _context.Libros.Add(libro);

                //se aplican los cambios en la db
                await _context.SaveChangesAsync();

                //ubicamos al usuario dentro del listado libros
                return RedirectToAction("Index");
                
            }
        }


        //Métodos para el proceso de eliminar un libro
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //buscar e libro  a eliminar por medio del  ORM
            var temp  = await _context.Libros.FirstOrDefaultAsync(r => r.ISBN == id);

            //se envian los datos del libro al front-end
            return View(temp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Delete(int? id)
        {
            //se busca por el ISBN el libro  a eliminar
            var temp = await _context.Libros.FirstOrDefaultAsync(r => r.ISBN == id);

            if(temp != null) //se valida si tiene datos
            {
                _context.Libros.Remove(temp); //se elimina el libro
                await _context.SaveChangesAsync();  //se aplican los cambios
                return RedirectToAction("Index");//se muestra la lista de libros
            }
            else
            {
                return NotFound(); //Error 404 recurso no disponible
            }
        }


        //Método encargado de mostrar los datos para un libro especifico
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            //se busca por ISBN el libro a mostrar
            var temp = await _context.Libros.FirstOrDefaultAsync(r => r.ISBN == id);

            //se envian los datos al front-end
            return View(temp);
        }


        //Métodos encargados de realizar el proceso de editar los datos a un libro
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //se busca por el ISBN  los datos del a editar
            var temp  = await _context.Libros.FirstOrDefaultAsync(r =>r.ISBN == id);

            //se envia los datos del libro al front-end
            return View(temp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Edit(int id, [Bind]Libro plibro)
        {
            //validacion del ID
            if(id == plibro.ISBN)
            {
                //se busca el libro anterior con sus datos
                var  temp = await _context.Libros.FirstOrDefaultAsync( r => r.ISBN == id);

                _context.Libros.Remove(temp); //se elimina el libro
                _context.Libros.Add(plibro); //se agrega el libro
                await _context.SaveChangesAsync(); //se aplican los cambios

                //se ubica al usuario dentro del listado de libros
                return RedirectToAction("Index");

            }
            else
            {
                return NotFound();//Recurso no disponible
            }
        }



    } //cierre controller
} //cierre namepaces
