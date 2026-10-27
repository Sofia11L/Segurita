using System.Threading.Tasks;
using System.Web.Http;

namespace PL.Controllers
{
    public class ClienteApiController : ApiController
    {
        [HttpGet]
        [Route("api/clientes")]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await BL.Cliente.GetAll();

            if (result.Correct)
            {
                return Ok(result.Objects);
            }
            else
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/clientes/{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var result = await BL.Cliente.GetById(id);

            if (result.Correct)
            {
                return Ok(result.Object);
            }
            else
            {
                return BadRequest("No se pudo consultar el cliente.");
            }
        }

        [HttpPost]
        [Route("api/clientes")]
        public async Task<IHttpActionResult> Add([FromBody] ML.Cliente cliente)
        {
            if (cliente == null || string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                return BadRequest("Ingresa el nombre del cliente.");
            }

            cliente.Nombre = cliente.Nombre.Trim();

            if (cliente.Nombre.Length > 100)
            {
                return BadRequest("El nombre debe tener máximo 100 caracteres.");
            }

            var result = await BL.Cliente.Add(cliente);

            if (result.Correct)
            {
                return Ok("Cliente agregado correctamente.");
            }
            else
            {
                return BadRequest("No se pudo agregar el cliente. Revisa que el nombre no esté repetido.");
            }
        }

        [HttpPut]
        [Route("api/clientes/{id:int}")]
        public async Task<IHttpActionResult> Update(int id, [FromBody] ML.Cliente cliente)
        {
            if (cliente == null || string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                return BadRequest("Ingresa el nombre del cliente.");
            }

            cliente.Nombre = cliente.Nombre.Trim();

            if (cliente.Nombre.Length > 100)
            {
                return BadRequest("El nombre debe tener máximo 100 caracteres.");
            }

            cliente.Id = id;

            var result = await BL.Cliente.Update(cliente);

            if (result.Correct)
            {
                return Ok("Cliente modificado correctamente.");
            }
            else
            {
                return BadRequest("No se pudo modificar el cliente. Revisa el Id y que el nombre no esté repetido.");
            }
        }

        [HttpDelete]
        [Route("api/clientes/{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var result = await BL.Cliente.Delete(id);

            if (result.Correct)
            {
                return Ok("Cliente eliminado correctamente.");
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
    }
}