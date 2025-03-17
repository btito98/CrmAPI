using CRM.Application.DTOs.Cliente;
using CRM.Application.Exceptions;
using CRM.Application.Interfaces;
using CRM.Domain.Models.Cliente;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ValidationException = CRM.Application.Exceptions.ValidationException;

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
            var cliente = await _clienteService.GetByIdAsync(id);

            if (cliente == null)
                throw new NotFoundException($"Cliente com ID {id} não encontrado");

            return Ok(cliente);
        }

        [ProducesResponseType(typeof(IEnumerable<ClienteResultDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ClienteFilterParams filterParams)
        {
            var (clientes, totalCount) = await _clienteService.GetFilteredAsync(filterParams);

            Response.Headers.Append("X-Total-Count", totalCount.ToString());

            return Ok(clientes);
        }

        [ProducesResponseType(typeof(ClienteResultDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClienteCreateDTO cliente)
        {
            var validationResult = await _clienteCreateValidator.ValidateAsync(cliente);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Erro de validação ao criar cliente {@ClienteRequest} {@ValidationErrors}", cliente, errorMessages);
                throw new ValidationException(errorMessages);
            }

            cliente.InitializeUserCreation(User.FindFirst("name")?.Value ?? "Não identificado");

            var clienteCriado = await _clienteService.AddAsync(cliente);

            return CreatedAtAction(nameof(GetById), new { id = clienteCriado.Id }, clienteCriado);
        }

        [ProducesResponseType(typeof(IEnumerable<ClienteResultDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "CreateCliente")]
        [HttpPost("createrange")]
        public async Task<IActionResult> PostRange([FromBody] IEnumerable<ClienteCreateDTO> clientes)
        {
            foreach (var cliente in clientes)
            {
                var validationResult = await _clienteCreateValidator.ValidateAsync(cliente);
                if (!validationResult.IsValid)
                {
                    var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    _logger.LogWarning("Erro de validação ao criar clientes em lote {@ClienteRequest} {@ValidationErrors}", cliente, errorMessages);
                    throw new ValidationException($"Erro de validação para cliente {cliente.Nome}", errorMessages);
                }
            }

            clientes.ToList().ForEach(c => c.InitializeUserCreation(User.FindFirst("name")?.Value ?? "Não identificado"));

            var clientesCriados = await _clienteService.AddRangeAsync(clientes);

            return CreatedAtAction(nameof(Get), null, clientesCriados);
        }

        [ProducesResponseType(typeof(ClienteResultDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ClienteCreateDTO cliente)
        {
            var validationResult = await _clienteCreateValidator.ValidateAsync(cliente);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Erro de validação ao atualizar cliente {@ClienteRequest} {@ValidationErrors}", cliente, errorMessages);
                throw new ValidationException(errorMessages);
            }

            cliente.UpdateDTO(User.FindFirst("name")?.Value ?? "Não identificado");

            var clienteAtualizado = await _clienteService.UpdateAsync(id, cliente);

            return Ok(clienteAtualizado);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            string usuarioAlteracao = User.FindFirst("name")?.Value ?? "Não identificado";

            await _clienteService.RemoveAsync(id, usuarioAlteracao);

            return Ok();
        }
    }
}