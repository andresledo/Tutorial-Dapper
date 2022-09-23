
using Dapper;
using Microsoft.Data.SqlClient;
using TutorialDapper.Entities;

namespace TutorialDapper.Services
{
    public class MarkService
    {
        //Connection string de nuestra base de datos, substituir por tus parámetros de conexión
        //NOTA: No es recomendable guardar la connection string en el código fuente, para no hacer
        //más largo el tutorial la guardamos aquí, pero debería escribirse en un json externo al código
        private static string _connectionString = "Server=TORRE;Database=Notas;User Id=sa;Password=Informatica_1;";

        public static List<Mark> GetMarks()
        {
            //SQL que ejecutara Dapper, aquí puedes jugar con los orders que quieras.
            string sql = @"SELECT [Id]
                              ,[Name]
                              ,[Surname]
                              ,[Nota]
                          FROM [Marks] 
                          ORDER BY Surname";

            //Iniciar la conexión con la base de datos
            var db = new SqlConnection(_connectionString);

            //Ejecutar la consulta SQL y almacenar las líneas en nuestro modelo. 
            var marks = db.Query<Mark>(sql);

            //Dapper devuelve un IEnumerable para trabajar más cómodos lo convertimos a listas. 
            return marks.ToList();
        }

        public static Mark GetMark(int id)
        {
            //En este caso tenemos que introducir un parametro para el Id,
            //NUNCA concatenes directamente la variable en el SQL porque puedes padecer inyecciones SQL
            string sql = @"SELECT [Id]
                              ,[Name]
                              ,[Surname]
                              ,[Nota]
                          FROM [Marks] 
                          WHERE Id = @id";

            //Iniciar la conexión con la base de datos
            var db = new SqlConnection(_connectionString);

            //Ejecutar la consulta SQL y pasar los parametros en un objeto, como id se llama igual
            //en la variable que en el parametro no hace falta escribir id = id. 
            //Agregamos first para que solo nos devuelva una línea (obviamente solo va a haber una al buscar por id)
            var mark = db.QueryFirst<Mark>(sql, new { id });

            //Devolvemos el objeto.
            return mark;
        }

        public static int CreateMark(Mark mark)
        {
            //Generamos la consulta con sus correspondientes parametros, agregamos
            //OUTPUT para que nos devuelva el id del registro insertado.
            string sql = @"INSERT INTO [Marks] ([Name], [Surname], [Nota])
                            OUTPUT INSERTED.Id
                           VALUES (@name, @surname, @mark);";

            //Iniciar la conexión con la base de datos
            var db = new SqlConnection(_connectionString);

            //Mapeamos los parametros y ejecutamos la consulta.
            var id = db.QuerySingle<int>(sql, new
            {
                name = mark.Name,
                surname = mark.Surname,
                mark = mark.Nota,
            });

            //Devolvemos el id del registro insertado
            return id;

        }

        public static void UpdateMark(Mark mark, int id)
        {
            //Generamos la consulta con sus correspondientes parametros
            string sql = @"UPDATE [Marks] 
                           SET
                                [Name] = @name, 
                                [Surname] = @surname, 
                                [Nota] = @mark 
                           WHERE 
                                [Id] = @id";

            //Iniciar la conexión con la base de datos
            var db = new SqlConnection(_connectionString);

            //Mapeamos los parametros y ejecutamos la consulta.
            db.Query(sql, new
            {
                id,
                name = mark.Name,
                surname = mark.Surname,
                mark = mark.Nota,
            });
        }

        public static void DeleteMark(int id)
        {
            //Generamos la consulta con sus correspondientes parametros
            string sql = @"DELETE FROM [Marks]       
                           WHERE [Id] = @id";

            //Iniciar la conexión con la base de datos
            var db = new SqlConnection(_connectionString);

            //Mapeamos los parametros y ejecutamos la consulta.
            db.Query(sql, new { id});
        }


    }
}
