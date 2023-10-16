using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;
using TemplateVSAMinimalAPI.Common.Exceptions;
using TemplateVSAMinimalAPI.Persistence;

namespace TemplateVSAMinimalAPI.Common.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where  TRequest : IRequest<TRequest>
    {

        private readonly AppDbContext _context;
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger _logger;

        public TransactionBehavior(AppDbContext context, IEnumerable<IValidator<TRequest>> validators, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            _context = context;
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                if (_validators.Any())
                {
                    var validationContext = new ValidationContext<TRequest>(request);
                    var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(validationContext, cancellationToken)));
                    var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
                    if (failures.Count != 0)
                    {
                        throw new ResponseException(StatusCodes.Status500InternalServerError, "Validation request failed", failures);
                    }
                }

                await _context.BeginTransactionAsync();
                var response = await next();
                await _context.CommitTransactionAsync();

                return response;
            }
            catch
            {
                await _context.RollbackTransaction();
                throw;
            }
          
        }
    }
}
