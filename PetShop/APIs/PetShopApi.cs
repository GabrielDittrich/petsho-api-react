using Microsoft.EntityFrameworkCore;
public static class PetShopApi
{
    public static void MapPetShopApi(this WebApplication app)
    {
        var group = app.MapGroup("/animais");


        group.MapGet("/", async (BancoDeDados db) =>
            //select * from pessoas
            await db.Animais.ToListAsync()
        );

        group.MapPost("/", async (Animal animal, BancoDeDados db) =>
        {
            db.Animais.Add(animal);
            //insert into...
            await db.SaveChangesAsync();

            return Results.Created($"/animais/{animal.Id}", animal);
        }
        );

        group.MapPut("/{id}", async (int id, Animal animalAlterado, BancoDeDados db) =>
        {
            //select * from pessoas where id = ?
            var animal = await db.Animais.FindAsync(id);
            if (animal is null)
            {
                return Results.NotFound();
            }
            animal.NomeAnimal = animalAlterado.NomeAnimal;
            animal.Raca = animalAlterado.Raca;

            //update....
            await db.SaveChangesAsync();

            return Results.NoContent();
        }
        );

        group.MapDelete("/{id}", async (int id, BancoDeDados db) =>
        {
            if (await db.Animais.FindAsync(id) is Animal animal)
            {
                //Operações de exclusão
                db.Animais.Remove(animal);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }
            return Results.NotFound();
        }
        );
    }
}