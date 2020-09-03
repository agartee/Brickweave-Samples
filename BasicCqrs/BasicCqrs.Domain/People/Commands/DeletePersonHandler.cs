﻿using BasicCqrs.Domain.People.Services;
using Brickweave.Cqrs;
using System.Threading.Tasks;

namespace BasicCqrs.Domain.People.Commands
{
    public class DeletePersonHandler : ICommandHandler<DeletePerson>
    {
        private readonly IPersonRepository personRepository;

        public DeletePersonHandler(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public async Task HandleAsync(DeletePerson command)
        {
            await personRepository.DeletePerson(command.Id);
        }
    }
}
