using FluentValidation.Results;

using MediatR;

namespace Book.Domain.Common;


/// <summary>
/// By creating a base class that inherits from <see cref="IRequest"/> I can abstract the dependency 
/// of my entire system to this class instead of a library interface. If at any point we want to 
/// change the lib, just change this reference here.
/// </summary>
public abstract record BaseCommand : IRequest
{
    public virtual ValidationResult Validate()
    {
        throw new NotImplementedException();
    }
}