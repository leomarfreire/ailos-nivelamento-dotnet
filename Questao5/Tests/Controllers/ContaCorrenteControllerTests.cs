using System.Net;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Infrastructure.Services.Controllers;
using Xunit;

namespace Questao5.Tests.Controllers
{
	public class ContaCorrenteControllerTests
	{
		private readonly Mock<ILogger<ContaCorrenteController>> _loggerMock;
		private readonly Mock<IMediator> _mediatorMock;
		private readonly ContaCorrenteController _controller;

		public ContaCorrenteControllerTests()
		{
			_loggerMock = new Mock<ILogger<ContaCorrenteController>>();
			_mediatorMock = new Mock<IMediator>();
			_controller = new ContaCorrenteController(_loggerMock.Object, _mediatorMock.Object);
		}

		[Fact]
		public async Task PostMovimentacoes_ValidRequest_ShouldReturnOk()
		{
			var request = new MovimentacaoRequest
			{
				IdentificacaoContaCorrente = "123",
				IdentificacaoRequisicao = "req-001",
				Valor = 100,
				TipoMovimento = "C"
			};
			var expectedIdMovimento = "mov-001";
			_mediatorMock.Setup(m => m.Send(request, default)).ReturnsAsync(expectedIdMovimento);

			var result = await _controller.PostMovimentacoes(request);

			var okResult = result.Result as OkObjectResult;
			okResult.Should().NotBeNull();
			okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

			var response = okResult.Value as MovimentacaoResponse;
			response.Should().NotBeNull();
			response.IdMovimento.Should().Be(expectedIdMovimento);
		}

		[Fact]
		public async Task PostMovimentacoes_InvalidRequest_ShouldReturnBadRequest()
		{
			var request = new MovimentacaoRequest
			{
				IdentificacaoContaCorrente = null,
				IdentificacaoRequisicao = "req-002",
				Valor = 100,
				TipoMovimento = "C"
			};
			_controller.ModelState.AddModelError("IdentificacaoContaCorrente", "O campo IdentificacaoContaCorrente é obrigatório.");

			var result = await _controller.PostMovimentacoes(request);

			var badRequestResult = result.Result as ObjectResult;
			badRequestResult.Should().NotBeNull();
			badRequestResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

			var response = badRequestResult.Value;
			response.Should().NotBeNull();

			var errorType = (string)response.GetType().GetProperty("error").GetValue(response, null);
			var message = (string)response.GetType().GetProperty("message").GetValue(response, null);

			errorType.Should().Be("VALIDATION_ERROR");
			message.Should().Contain("O campo IdentificacaoContaCorrente é obrigatório.");
		}

		[Fact]
		public async Task GetSaldo_ValidId_ShouldReturnOk()
		{
			var idContaCorrente = "123";
			var saldoResponse = new SaldoResponse
			{
				IdContaCorrente = idContaCorrente,
				SaldoAtual = 100
			};

			_mediatorMock.Setup(m => m.Send(It.IsAny<SaldoRequest>(), default)).ReturnsAsync(saldoResponse);

			var result = await _controller.GetSaldo(idContaCorrente);

			var okResult = result.Result as OkObjectResult;
			okResult.Should().NotBeNull();
			okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

			var response = okResult.Value as SaldoResponse;
			response.Should().NotBeNull();
			response.Should().BeEquivalentTo(saldoResponse);
		}

		[Fact]
		public async Task GetSaldo_InvalidId_ShouldReturnNotFound()
		{
			var idContaCorrente = "invalid-id";
			var expectedMessage = "Conta corrente não encontrada.";

			_mediatorMock.Setup(m => m.Send(It.IsAny<SaldoRequest>(), default)).ReturnsAsync((SaldoResponse)null);

			var result = await _controller.GetSaldo(idContaCorrente);

			var notFoundResult = result.Result as NotFoundObjectResult;
			notFoundResult.Should().NotBeNull();
			notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
		}
	}
}