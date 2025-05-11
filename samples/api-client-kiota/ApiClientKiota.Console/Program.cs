using Clients.Petstore;
using Clients.Petstore.Models;

using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Bundle;

var requestAdapter = new DefaultRequestAdapter(new AnonymousAuthenticationProvider());
var client = new PetstoreClient(requestAdapter);

var pet = new Pet
{
    Id = 123,
    Name = "Sir Barksalot",
    Status = Pet_status.Available
};

await client.Pet.PostAsync(pet);

var createdPet = await client.Pet[pet.Id!.Value].GetAsync();

Console.ReadLine();
