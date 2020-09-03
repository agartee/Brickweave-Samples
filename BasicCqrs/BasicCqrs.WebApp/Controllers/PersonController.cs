﻿using BasicCqrs.Domain.People.Commands;
using BasicCqrs.Domain.People.Models;
using BasicCqrs.Domain.People.Queries;
using BasicCqrs.WebApp.Extensions;
using BasicCqrs.WebApp.Models;
using Brickweave.Cqrs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BasicCqrs.WebApp.Controllers
{
    public class PersonController : Controller
    {
        private readonly IDispatcher dispatcher;

        public PersonController(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpGet, Route("/people/")]
        public async Task<IActionResult> ListAsync()
        {
            var results = await dispatcher.DispatchQueryAsync(new ListPeople());

            return View(results
                .Select(p => p.ToViewModel())
                .ToList());
        }

        [HttpGet, Route("/person/create")]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost, Route("/person/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(PersonViewModel viewModel)
        {
            await dispatcher.DispatchCommandAsync(new CreatePerson(
                viewModel.FirstName,
                viewModel.LastName));

            return Redirect("/people");
        }

        [HttpGet, Route("/person/{id}/edit")]
        public async Task<IActionResult> EditAsync([FromRoute] Guid id)
        {
            var result = await dispatcher.DispatchQueryAsync(new GetPerson(
                new PersonId(id)));

            return View(result.ToViewModel());
        }

        [HttpPost, Route("/person/{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(PersonViewModel viewModel)
        {
            var result = await dispatcher.DispatchCommandAsync(new UpdatePerson(
                new PersonId(viewModel.Id),
                viewModel.FirstName,
                viewModel.LastName));

            return View(result.ToViewModel());
        }

        [HttpGet, Route("/person/{id}/delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var result = await dispatcher.DispatchQueryAsync(new GetPerson(
                new PersonId(id)));

            return View(result.ToViewModel());
        }

        [HttpPost, Route("/person/{id}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, IFormCollection collection)
        {
            var result = await dispatcher.DispatchCommandAsync(new DeletePerson(
                new PersonId(id)));

            return Redirect("/people");
        }
    }
}
