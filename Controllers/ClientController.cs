using Microsoft.AspNetCore.Mvc;
using CajaMoreliaApi.Models;
using CajaMoreliaApi.Database;
using Microsoft.EntityFrameworkCore;

namespace CajaMoreliaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ConnectorDatabase _context;

        public ClientController(ConnectorDatabase context)
        {
            _context = context;
        }

        // GET: api/clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clients>>> GetClientes()
        {
            
            var clients = await _context.Clients.ToListAsync();

            var response = new ApiResponse<IEnumerable<object>>("success", clients, "Clientes encontrados "+clients.Count);
            return Ok(response);
        }

        // GET: api/clientes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Clients>> GetCliente(int id)
        {
            var cliente = await _context.Clients.FindAsync(id);
            
            if (cliente == null)
            {
                return Ok(new ApiResponse<object>("error", new {}, "No se encontro ningun cliente"));
            }

            return Ok(new ApiResponse<object>("success", cliente, "Clientre encontrado"));
        }

        // POST: api/clientes
        [HttpPost]
        public async Task<ActionResult<Clients>> PostCliente(Clients cliente)
        {

            var exist = await _context.Clients.FirstOrDefaultAsync(w => w.Correo == cliente.Correo.Trim());
            
            if (exist != null)
            {
                return Ok(new ApiResponse<object>("error", new {}, "El Cliente ya existe"));
            }


            _context.Clients.Add(cliente);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>("success", new {
                id = cliente.Id
            }, "Cliente agregado"));
        }

        // PUT: api/clientes/{id}
        [HttpPut]
        public async Task<IActionResult> PutCliente(Clients cliente)
        {
            if (cliente.Id == null)
            {
                return Ok(new ApiResponse<object>("error", new {}, "El Id del cliente es obligatorio"));
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new ApiResponse<object>("success", new {id = cliente.Id}, "Cliente actualizado"));
            }
            catch (DbUpdateConcurrencyException)
            {
                return Ok(new ApiResponse<object>("error", new {}, "No se encontro ningun cliente"));
            }

            
        }

        // DELETE: api/clientes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clients.FindAsync(id);
            if (cliente == null)
            {
                return Ok(new ApiResponse<object>("error", new {}, "No se encontro ningun cliente"));
            }

            _context.Clients.Remove(cliente);
            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<object>("success", new {id = cliente.Id}, "Cliente Eliminado"));
        }
    }
}