using Bookify.Results;
using MediatR;
using Unit = Bookify.Results.Unit;

namespace Bookify.Application.Abstractions.Messaging
{
    /// <summary>
    /// Base marker interface for all commands in the CQRS pattern.
    /// This interface serves as the foundation for command objects that represent intentions to change the system state.
    /// </summary>
    public interface IBaseCommand { }

    /// <summary>
    /// Generic base interface for commands that return a specific response type.
    /// Integrates with MediatR's request/response pattern and wraps responses in a Result type for success/failure handling.
    /// </summary>
    /// <typeparam name="TResponse">The type of response returned when the command is processed</typeparam>
    public interface IBaseCommand<TResponse> : IRequest<Result<TResponse>> { }

    /// <summary>
    /// Interface for commands that don't return a specific value but indicate success or failure.
    /// These commands typically represent operations that modify state without needing to return data.
    /// Uses the Unit type to represent a void result within the Result pattern.
    /// </summary>
    public interface ICommand : IRequest<Result<Unit>>, IBaseCommand { }

    /// <summary>
    /// Interface for commands that return a specific response type.
    /// Use this for operations that both modify state and need to return data to the caller.
    /// </summary>
    /// <typeparam name="TResponse">The type of response returned when the command is processed</typeparam>
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand { }

    /// <summary>
    /// Handler interface for processing commands that don't return a specific value.
    /// In the CQRS pattern, command handlers contain the logic to process commands and produce state changes.
    /// Extends MediatR's IRequestHandler to handle the command processing pipeline.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to handle</typeparam>
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result<Unit>>
        where TCommand : ICommand { }

    /// <summary>
    /// Handler interface for processing commands that return a specific response type.
    /// Combines the command pattern (state modification) with the ability to return a result.
    /// Uses the Result type to encapsulate success/failure outcomes along with the response data.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to handle</typeparam>
    /// <typeparam name="TResponse">The type of response returned when the command is processed</typeparam>
    public interface ICommandHandler<in TCommand, TResponse>
        : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : ICommand<TResponse> { }

    /// <summary>
    /// Handler interface for processing queries that return data without modifying state.
    /// In the CQRS pattern, queries represent operations that read data but don't change system state.
    /// Queries are implemented as commands that only retrieve information.
    /// </summary>
    /// <typeparam name="TQuery">The type of query to handle</typeparam>
    /// <typeparam name="TResponse">The type of response returned by the query</typeparam>
    public interface IQueryHandler<in TQuery, TResponse>
        : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IBaseCommand<TResponse> { }
}
