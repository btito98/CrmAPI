using CRM.API.Helpers;
using CRM.Application.DTOs.Cliente;
using CRM.Application.Exceptions;
using CRM.Application.Interfaces;
using CRM.Domain.Models.Cliente;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{

    [ApiController]
    [Route("api/clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly IValidator<ClienteCreateDTO> _clienteCreateValidator;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(
            IClienteService clienteService,
            IValidator<ClienteCreateDTO> clienteCreateValidator,
            ILogger<ClienteController> logger)
        {
            _clienteService = clienteService;
            _clienteCreateValidator = clienteCreateValidator;
            _logger = logger;
        }

        [ProducesResponseType(typeof(ClienteResultDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "ReadCliente")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var cliente = await _clienteService.GetByIdAsync(id);

                if (cliente == null) return NotFound(new ErroResponse(404, "Cliente não encontrado"));

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cliente por id {id}", id);
                return BadRequest(new ErroResponse(400, ex.Message));
            }
        }

        [ProducesResponseType(typeof(IEnumerable<ClienteResultDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ClienteFilterParams filterParams)
        {
            try
            {
                var (clientes, totalCount) = await _clienteService.GetFilteredAsync(filterParams);

                Response.Headers.Append("X-Total-Count", totalCount.ToString());

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar clientes");
                return BadRequest(new ErroResponse(400, ex.Message));
            }
        }

        [ProducesResponseType(typeof(ClienteResultDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClienteCreateDTO cliente)
        {
            try
            {
                var validationResult = await _clienteCreateValidator.ValidateAsync(cliente);

                if (!validationResult.IsValid)
                {
                    var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    _logger.LogWarning("Erro de validação ao criar cliente {@ClienteRequest} {@ValidationErrors}", cliente, errorMessages);
                    return BadRequest(new ErroResponse(400, errors: errorMessages));
                }

                cliente.InitializeUserCreation(User.FindFirst("name")?.Value ?? "Não identificado");

                var clienteCriado = await _clienteService.AddAsync(cliente);

                return CreatedAtAction(nameof(GetById), new { id = clienteCriado.Id }, clienteCriado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar cliente {@ClienteRequest}", cliente);
                return BadRequest(new ErroResponse(400, ex.Message));
            }
        }

        [ProducesResponseType(typeof(IEnumerable<ClienteResultDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "CreateCliente")]
        [HttpPost("createrange")]
        public async Task<IActionResult> PostRange([FromBody] IEnumerable<ClienteCreateDTO> clientes)
        {
            try
            {
                clientes.ToList().ForEach(c => c.InitializeUserCreation(User.FindFirst("name")?.Value ?? "Não identificado"));

                var clientesCriados = await _clienteService.AddRangeAsync(clientes);

                return CreatedAtAction(nameof(Get), null, clientesCriados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar clientes {@ClientesRequest}", clientes);
                return BadRequest(new ErroResponse(400, ex.Message));
            }
        }

        [ProducesResponseType(typeof(ClienteResultDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, ClienteCreateDTO cliente)
        {
            try
            {
                cliente.UpdateDTO(User.FindFirst("name")?.Value ?? "Não identificado");

                var clienteAtualizado = await _clienteService.UpdateAsync(id, cliente);

                return Ok(clienteAtualizado);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Cliente não encontrado para atualizar por id {id}", id);
                return NotFound(new ErroResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar cliente {@ClienteRequest}", cliente);
                return BadRequest(new ErroResponse(400, ex.Message));
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                string usuarioAlteracao = User.FindFirst("name")?.Value ?? "Não identificado";

                await _clienteService.RemoveAsync(id, usuarioAlteracao);

                return Ok();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Cliente não encontrado para deletar por id {id}", id);
                return NotFound(new ErroResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar cliente por id {id}", id);
                return BadRequest(new ErroResponse(400, ex.Message));
            }
        }
    }
}
