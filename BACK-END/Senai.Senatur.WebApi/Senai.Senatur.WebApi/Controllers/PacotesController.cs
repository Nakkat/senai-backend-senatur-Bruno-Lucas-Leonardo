﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senai.Senatur.WebApi.Interfaces;
using Senai.Senatur.WebApi.Repositories;

namespace Senai.Senatur.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class PacotesController : ControllerBase
    {
        /// <summary>
        /// Cria um objeto _pacoteRepository que irá receber todos os métodos definidos na interface
        /// </summary>
        private IPacoteRepository _pacoteRepository;

        /// <summary>
        /// Instancia este objeto para que haja a referência aos métodos no repositório
        /// </summary>
        public PacotesController()
        {
            _pacoteRepository = new PacoteRepository();
        }

        /// <summary>
        /// Lista todos os pacotes
        /// </summary>
        /// <returns>Uma lista de pacotes e um status code 200 - Ok</returns>
        [HttpGet]
        public IActionResult Get()
        {
            // Retora a resposta da requisição fazendo a chamada para o método
            return Ok(_pacoteRepository.ListarPacotes());
        }

        /// <summary>
        /// Busca um pacote através do ID
        /// </summary>
        /// <param name="id">ID do pacote que será buscado</param>
        /// <returns>Um pacote buscado e um status code 200 - Ok</returns>
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // Retora a resposta da requisição fazendo a chamada para o método
            return Ok(_pacoteRepository.BuscarPacotesPorId(id));
        }

        /// <summary>
        /// Cadastra um novo pacote
        /// </summary>
        /// <param name="novoPacote">Objeto novoPacote que será cadastrado</param>
        /// <returns>Um status code 201 - Created</returns>
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPost]
        public IActionResult Post(Pacote novoPacote)
        {
            // Tenta fazer o método
            try
            {
                // Faz a chamada para o método
                _pacoteRepository.CadastrarPacote(novoPacote);

                // Retorna um status code
                return StatusCode(201);
            }
            // Caso contrário retorna uma mensagem de erro de má requisição
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Altera um pacote
        /// </summary>
        /// <param name="id">Id do estúdio que será buscado</param>
        /// <param name="pacoteAtualizado">Objeto pacoteAtualizado que será alterado</param>
        /// <returns>Um Status Code 204 (No Content)</returns>
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, Pacote pacoteAtualizado)
        {
            // Cria um objeto em Pacotes para armazenar IdBuscado
            Pacote pacoteBuscado = _pacoteRepository.BuscarPacotesPorId(id);

            // Se o Id do pacote buscado for nulo :
            if (pacoteBuscado == null)
            {
                return NotFound
                    (
                        new
                        {
                            mensagem = "Pacote não encontrado",
                            erro = true
                        }
                    );
            }

            // Tenta fazer o método
            try
            {
                _pacoteRepository.AtualizarPacote(id, pacoteAtualizado);

                return NoContent();
            }

            // Caso contrário : 
            catch (Exception erro)
            {
                return BadRequest(erro);
            }
        }

        /// <summary>
        /// Deleta um pacote
        /// </summary>
        /// <param name="id">Id do pacote que será deletado</param>
        /// <returns>Um status code 200</returns>
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _pacoteRepository.DeletarPacote(id);

                return Ok("Pacote deletado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Desafios extras

        /// <summary>
        /// Listar pacotes ativos
        /// </summary>
        /// <returns>Retorna a lista e um status code 200</returns>
        [HttpGet("Ativos")]
        public IActionResult GetByAtivo()
        {
            List<Pacote> listaPacotes = _pacoteRepository.ListarPacotesAtivos();
            return StatusCode(200, new { mensagem = "Lista de pacotes ativos", listaPacotes });
        }

        /// <summary>
        /// Listar pacotes inativos
        /// </summary>
        /// <returns>Retorna a lista e um status code 200</returns>
        [HttpGet("Inativos")]
        public IActionResult GetByInativo()
        {
            List<Pacote> listaPacotes = _pacoteRepository.ListarPacotesInativos();
            return StatusCode(200, new { mensagem = "Lista de pacotes inativos", listaPacotes });
        }

        /// <summary>
        /// Listar pacotes de uma cidade específica
        /// </summary>
        /// <param name="cidadeBuscada"></param>
        /// <returns>Retorna a lista e um status code 200</returns>
        [HttpGet("{cidadeBuscada}")]
        public IActionResult GetByCidade(string cidadeBuscada)
        {
            List<Pacote> listaPacotes = _pacoteRepository.ListarPacotesCidades(cidadeBuscada);
            return StatusCode(200, new { mensagem = "Lista de pacotes de uma cidade específica", listaPacotes });
        }

        /// <summary>
        /// Listar pacotes ordenados por preço
        /// </summary>
        /// <param name="ordem"></param>
        /// <returns>Retorna a lista e um status code 200</returns>
        [HttpGet("Preco/{ordem}")]
        public IActionResult GetByPreco(string ordem)
        {
            List<Pacote> listaPacotes = _pacoteRepository.ListarPacotesPorPreco(ordem);
            return StatusCode(200, new { mensagem = "Lista de pacotes orderdenados por preço", listaPacotes });
        }
    }
}