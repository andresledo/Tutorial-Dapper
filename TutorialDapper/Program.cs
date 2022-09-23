
using TutorialDapper.Entities;
using TutorialDapper.Services;

//Listar todos los registros 
var marks = MarkService.GetMarks();

foreach (var mark in marks)
{
    Console.WriteLine($"{mark.Surname} {mark.Name} = {mark.Nota}");
}

//Insertar un registro en la base de datos
var newMark = new Mark()
{
    Name = "Juan",
    Surname = "Pablo",
    Nota = 8.00m,
};

var id = MarkService.CreateMark(newMark);
Console.WriteLine($"Se creo la nota con id = {id}");

//Buscar un registro por id
var findMark = MarkService.GetMark(id);
Console.WriteLine($"El registro con {id} se llama {findMark.Name}");

//Actualizar un registro en la base de datos
findMark.Name = "Pedro";
MarkService.UpdateMark(findMark, id);
Console.WriteLine($"El registro con {id} ahora se llama Pedro {findMark.Name}");

//Eliminamos el registro
MarkService.DeleteMark(id);
Console.WriteLine($"Pedro ahora está en un lugar mejor...");


