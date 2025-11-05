using Common.Extensions;
using Common.Results;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Extensions
{
    public static class ResultExtensions
    {
        #region Falhas (Result)
        /// <summary>
        /// Marca o resultado como falha com status HTTP BadRequest.
        /// </summary>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result Failed(this Result result)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.WithStatus(false, HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Marca o resultado como falha com mensagem e status HTTP BadRequest.
        /// </summary>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="message">A mensagem de erro a ser definida.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result Failed(this Result result, string message)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.Failed().WithMessage(message);
        }

        /// <summary>
        /// Marca o resultado como falha com mensagem, exceção e status HTTP BadRequest.
        /// </summary>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="message">A mensagem de erro a ser definida.</param>
        /// <param name="exception">A exceção associada à falha.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result Failed(this Result result, string message, Exception exception)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.Failed(message).WithException(exception);
        }

        /// <summary>
        /// Marca o resultado como não encontrado (NotFound) com status HTTP 404.
        /// </summary>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="message">A mensagem opcional.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result NotFound(this Result result, string? message = null)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.WithStatus(false, HttpStatusCode.NotFound).WithMessage(message);
        }

        /// <summary>
        /// Marca o resultado como erro interno do servidor com status HTTP 500.
        /// </summary>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result InternalServerError(this Result result)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.WithStatus(false, HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Marca o resultado como não autorizado com status HTTP 401.
        /// </summary>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result Unauthorized(this Result result)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.WithStatus(false, HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// Trata um erro de serviço adicionando mensagem e exceção ao resultado.
        /// </summary>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="errorMessage">A mensagem de erro.</param>
        /// <param name="ex">A exceção opcional associada.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result HandleServiceError(this Result result, string errorMessage, Exception? ex = null)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.Failed(errorMessage).WithException(ex);
        }
        #endregion

        #region Sucesso/Aviso (Result)
        /// <summary>
        /// Marca o resultado como sucesso com status HTTP 200.
        /// </summary>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result Successful(this Result result)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.WithStatus(true, HttpStatusCode.OK).RegisterFound();
        }

        /// <summary>
        /// Marca o resultado como sucesso com mensagem e status HTTP 200.
        /// </summary>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="message">A mensagem de sucesso.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result Successful(this Result result, string message)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.Successful().WithMessage(message);
        }
        #endregion

        #region Mensagens/Erros (Result)
        /// <summary>
        /// Define a mensagem do resultado.
        /// </summary>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="message">A mensagem a ser definida.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result WithMessage(this Result result, string? message)
        {
            ArgumentNullException.ThrowIfNull(result);
            result.Message = message;
            return result;
        }

        public static Result WithError(this Result result, string? error)
        {
            if (!string.IsNullOrWhiteSpace(error))
                result.AddError(error);
            return result;
        }

        public static Result WithError(this Result result, string error, string code)
        {
            result.AddError(error, code);
            return result;
        }

        public static Result WithErrors(this Result result, IEnumerable<IResultError> errors)
        {
            result.AddErrors(errors);
            return result;
        }
        #endregion

        #region Estado/Metadados/Exceção (Result)
        public static Result WithException(this Result result, Exception? exception)
        {
            result.Exception = exception;
            return result;
        }

        public static Result RegisterFound(this Result result)
        {
            result.Found = true;
            return result;
        }

        public static Result RegisterNotFound(this Result result)
        {
            result.Found = false;
            return result;
        }
        #endregion

        #region Falhas (Result<T>)
        /// <summary>
        /// Marca o resultado genérico como falha com status HTTP BadRequest.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> Failed<T>(this Result<T> result)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.WithStatus(false, HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Marca o resultado genérico como falha com mensagem.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="message">A mensagem de erro.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> Failed<T>(this Result<T> result, string message)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.Failed().WithMessage(message);
        }

        /// <summary>
        /// Marca o resultado genérico como falha com mensagem e controle de continuação.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="message">A mensagem de erro.</param>
        /// <param name="canContinue">Indica se pode continuar após a falha.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> Failed<T>(this Result<T> result, string message, bool canContinue = true)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.Failed(message).CanContinue(canContinue);
        }

        /// <summary>
        /// Marca o resultado genérico como falha com mensagem e dados.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="message">A mensagem de erro.</param>
        /// <param name="data">Os dados a serem associados.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> Failed<T>(this Result<T> result, string message, T data)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.Failed(message).WithData(data);
        }

        /// <summary>
        /// Marca o resultado genérico como falha com mensagem e exceção.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="message">A mensagem de erro.</param>
        /// <param name="ex">A exceção associada.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> Failed<T>(this Result<T> result, string message, Exception ex)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.Failed(message).WithException(ex);
        }

        /// <summary>
        /// Marca o resultado genérico como não encontrado (NotFound).
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="message">A mensagem opcional.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> NotFound<T>(this Result<T> result, string? message = null)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.WithStatus(false, HttpStatusCode.NotFound).WithMessage(message);
        }

        /// <summary>
        /// Marca o resultado genérico como erro interno do servidor.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> InternalServerError<T>(this Result<T> result)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.WithStatus(false, HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Marca o resultado genérico como não autorizado.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> Unauthorized<T>(this Result<T> result)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.WithStatus(false, HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// Trata um erro de serviço no resultado genérico.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="errorMessage">A mensagem de erro.</param>
        /// <param name="ex">A exceção opcional.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> HandleServiceError<T>(this Result<T> result, string errorMessage, Exception? ex = null)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.Failed(errorMessage).WithException(ex).WithData(default!);
        }
        #endregion

        #region Sucesso/Aviso (Result<T>)
        /// <summary>
        /// Marca o resultado genérico como sucesso.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> Successful<T>(this Result<T> result)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.WithStatus(true, HttpStatusCode.OK).RegisterFound();
        }

        /// <summary>
        /// Marca o resultado genérico como sucesso com dados.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="data">Os dados a serem associados.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> Successful<T>(this Result<T> result, T data)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.Successful().WithData(data);
        }

        /// <summary>
        /// Marca o resultado genérico como aviso (warning).
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="message">A mensagem de aviso.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> Warning<T>(this Result<T> result, string message)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.WithStatus(true, HttpStatusCode.NoContent).WithMessage(message);
        }

        /// <summary>
        /// Marca o resultado genérico como aviso (warning) com dados.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="data">Os dados a serem associados.</param>
        /// <param name="message">A mensagem de aviso.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> Warning<T>(this Result<T> result, T data, string message)
        {
            ArgumentNullException.ThrowIfNull(result);
            return result.WithStatus(true, HttpStatusCode.NoContent).WithMessage(message).WithData(data);
        }
        #endregion

        #region Mensagens/Erros/Dados (Result<T>)
        /// <summary>
        /// Define a mensagem do resultado genérico.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="message">A mensagem a ser definida.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> WithMessage<T>(this Result<T> result, string? message)
        {
            ArgumentNullException.ThrowIfNull(result);
            result.Message = message;
            return result;
        }

        /// <summary>
        /// Define os dados do resultado genérico.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="data">Os dados a serem definidos.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> WithData<T>(this Result<T> result, T data)
        {
            ArgumentNullException.ThrowIfNull(result);
            result.Data = data;
            return result;
        }

        /// <summary>
        /// Adiciona um erro ao resultado genérico.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="error">A mensagem de erro.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> WithError<T>(this Result<T> result, string? error)
        {
            ArgumentNullException.ThrowIfNull(result);
            if (!string.IsNullOrWhiteSpace(error))
                result.AddError(error);
            return result;
        }

        /// <summary>
        /// Adiciona um erro com código ao resultado genérico.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="error">A mensagem de erro.</param>
        /// <param name="code">O código de erro.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> WithError<T>(this Result<T> result, string error, string code)
        {
            ArgumentNullException.ThrowIfNull(result);
            result.AddError(error, code);
            return result;
        }

        /// <summary>
        /// Adiciona múltiplos erros ao resultado genérico.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="errors">A coleção de erros.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> WithErrors<T>(this Result<T> result, IEnumerable<IResultError> errors)
        {
            ArgumentNullException.ThrowIfNull(result);
            result.AddErrors(errors);
            return result;
        }

        /// <summary>
        /// Define a exceção do resultado genérico.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="exception">A exceção a ser associada.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> WithException<T>(this Result<T> result, Exception? exception)
        {
            ArgumentNullException.ThrowIfNull(result);
            result.Exception = exception;
            return result;
        }
        #endregion

        #region Controle e Marcação (Result<T>)
        /// <summary>
        /// Define se o fluxo pode continuar após o resultado genérico.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="canContinue">Indica se pode continuar.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> CanContinue<T>(this Result<T> result, bool canContinue)
        {
            ArgumentNullException.ThrowIfNull(result);
            result.CanContinue = canContinue;
            return result;
        }

        /// <summary>
        /// Marca o resultado genérico como encontrado.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> RegisterFound<T>(this Result<T> result)
        {
            ArgumentNullException.ThrowIfNull(result);
            result.Found = true;
            return result;
        }

        /// <summary>
        /// Marca o resultado genérico como não encontrado.
        /// </summary>
        /// <typeparam name="T">O tipo dos dados do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <returns>O resultado modificado para permitir encadeamento de chamadas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static Result<T> RegisterNotFound<T>(this Result<T> result)
        {
            ArgumentNullException.ThrowIfNull(result);
            result.Found = false;
            return result;
        }
        #endregion

        #region Utilidades
        /// <summary>
        /// Obtém a mensagem ou erro do resultado como string formatada.
        /// </summary>
        /// <param name="result">O resultado a ser processado.</param>
        /// <returns>String formatada contendo mensagens e erros.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static string GetMessageOrError(this IResult result)
        {
            ArgumentNullException.ThrowIfNull(result);
            
            var builder = new StringBuilder();

            if (result.Succeeded)
                return string.Empty;

            if (!string.IsNullOrWhiteSpace(result.Message))
                builder.AppendLine(result.Message);

            if (result.Errors is { } errors && errors.Any())
                builder.AppendJoin(" | ", errors.Select(x => x?.Error ?? string.Empty)).AppendLine();

            if (result is Result r && r.Exception != null)
                builder.AppendLine(r.Exception.Message);

            return builder.ToString();
        }

        /// <summary>
        /// Obtém o resultado serializado como JSON.
        /// </summary>
        /// <param name="result">O resultado a ser serializado.</param>
        /// <param name="includeException">Indica se deve incluir exceções na serialização.</param>
        /// <returns>JSON string do resultado.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        public static string GetMessageOrErrorToJson(this IResult result, bool includeException = false)
        {
            ArgumentNullException.ThrowIfNull(result);
            
            if (!includeException && result is Result r)
                r.Exception = null;

            return JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false
            });
        }
        #endregion

        #region Método Interno de Status (T)
        /// <summary>
        /// Define o status do resultado (interno).
        /// </summary>
        /// <typeparam name="T">O tipo do resultado.</typeparam>
        /// <param name="result">O resultado a ser modificado.</param>
        /// <param name="succeeded">Indica se foi bem-sucedido.</param>
        /// <param name="statusCode">O código de status HTTP.</param>
        /// <returns>O resultado modificado.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando result é null.</exception>
        private static T WithStatus<T>(this T result, bool succeeded, HttpStatusCode statusCode) where T : Result
        {
            ArgumentNullException.ThrowIfNull(result);
            result.Succeeded = succeeded;
            result.StatusCode = statusCode;
            return result;
        }
        #endregion
    }
}
