﻿using CRM.Application.DTOs.Cliente;
using CRM.Application.Exceptions;
using CRM.Application.Interfaces;
using CRM.Domain.Models.Cliente;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [ProducesResponseType(typeof(ClienteResultDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var cliente = await _clienteService.GetByIdAsync(id);

                if (cliente == null) return NotFound(new { details = "Cliente não encontrado" });

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(new { details = ex.Message });
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
                return BadRequest(new { details = ex.Message });
            }
        }

        [ProducesResponseType(typeof(ClienteResultDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClienteCreateDTO cliente)
        {
            try
            {
                var clienteCriado = await _clienteService.AddAsync(cliente);

                return CreatedAtAction(nameof(GetById), new { id = clienteCriado.Id }, clienteCriado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { details = ex.Message });
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
                string usuarioAlteracao = "Bruno Tits";
                // TODO: Implementar captura do nome do usuario baseado no token

                var clienteAtualizado = await _clienteService.UpdateAsync(id, cliente, usuarioAlteracao);

                return Ok(clienteAtualizado);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { details = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { details = ex.Message });
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
                string usuarioAlteracao = "Bruno Tits";
                // TODO: Implementar captura do nome do usuario baseado no token

                await _clienteService.RemoveAsync(id, usuarioAlteracao);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { details = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { details = ex.Message });
            }
        }

    }
}
