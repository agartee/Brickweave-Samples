using BasicCqrs.Domain.People.Models;
using Brickweave.Cqrs;
using LiteGuard;

namespace BasicCqrs.Domain.People.Commands
{
    public class UpdatePerson : ICommand<Person>
    {
        /// <summary>
        /// Updates an existing Person.
        /// </summary>
        /// <param name="id">Existing person's unique identifier.</param>
        /// <param name="firstName">Existing person's new first name.</param>
        /// <param name="lastName">Existing person's new last name.</param>
        public UpdatePerson(PersonId id, string firstName, string lastName)
        {
            Guard.AgainstNullArgument(nameof(id), id);

            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public PersonId Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}
