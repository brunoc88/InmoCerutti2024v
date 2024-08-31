using System.Windows.Markup;
using MySql.Data.MySqlClient;


public class RepositorioInquilino
{
    readonly string ConnectionString = "Server=localhost;Port=3306;Database=inmoCerutti;User=root;";

    //guardar inquilino
    public void AltaInquilino(Inquilino inquilino)
    {


        using (var conexion = new MySqlConnection(ConnectionString))
        {
            var sql = $"INSERT INTO inquilino ({nameof(Inquilino.Nombre)}, {nameof(Inquilino.Apellido)}, {nameof(Inquilino.Email)},{nameof(Inquilino.Dni)},{nameof(Inquilino.Telefono)}) " +
                      $"VALUES (@{nameof(Inquilino.Nombre)}, @{nameof(Inquilino.Apellido)}, @{nameof(Inquilino.Email)}, @{nameof(Inquilino.Dni)},@{nameof(Inquilino.Telefono)})";

            using (var comand = new MySqlCommand(sql, conexion))
            {
                // Agregar los parámetros usando nameof para evitar errores tipográficos
                comand.Parameters.AddWithValue($"@{nameof(Inquilino.Nombre)}", inquilino.Nombre);
                comand.Parameters.AddWithValue($"@{nameof(Inquilino.Apellido)}", inquilino.Apellido);
                comand.Parameters.AddWithValue($"@{nameof(Inquilino.Email)}", inquilino.Email);
                comand.Parameters.AddWithValue($"@{nameof(Inquilino.Dni)}", inquilino.Dni);
                comand.Parameters.AddWithValue($"@{nameof(Inquilino.Telefono)}", inquilino.Telefono);

                conexion.Open();
                comand.ExecuteNonQuery();
            }
        }
    }




    public List<Inquilino> GetInquilinos()
    {
        List<Inquilino> res = new List<Inquilino>();
        using (var conexion = new MySqlConnection(ConnectionString))
        {
            string sql = @$"
					SELECT 
						{nameof(Inquilino.id_inquilino)}, 
						{nameof(Inquilino.Nombre)},
                        {nameof(Inquilino.Apellido)},
                        {nameof(Inquilino.Email)},
                        {nameof(Inquilino.Dni)},
                        {nameof(Inquilino.Telefono)}
                       
					FROM inquilino WHERE Estado = 1";
            //tambien si quiere mostrar todo hago SELECT * FROM inquilino
            using (var comand = new MySqlCommand(sql, conexion))
            {
                conexion.Open();
                var reader = comand.ExecuteReader();
                while (reader.Read())
                {
                    res.Add(new Inquilino
                    {
                        id_inquilino = reader.GetInt32(nameof(Inquilino.id_inquilino)),
                        Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                        Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                        Email = reader.GetString(nameof(Inquilino.Email)),
                        Dni = reader.GetString(nameof(Inquilino.Dni)),
                        Telefono = reader.GetString(nameof(Inquilino.Telefono))
                    });
                }
                reader.Close();

            }
        }
        return res;
    }

    public Inquilino GetInquilino(int id)
    {
        var inquilino = new Inquilino();
        using (var conexion = new MySqlConnection(ConnectionString))
        {
            var sql = "SELECT * FROM inquilino WHERE id_inquilino = @id_inquilino";
            using (var comand = new MySqlCommand(sql, conexion))
            {
                conexion.Open();
                comand.Parameters.AddWithValue("@id_inquilino", id);

                using (var reader = comand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Inquilino
                        {
                            id_inquilino = reader.GetInt32(reader.GetOrdinal(nameof(Inquilino.id_inquilino))),
                            Nombre = reader.GetString(reader.GetOrdinal(nameof(Inquilino.Nombre))),
                            Apellido = reader.GetString(reader.GetOrdinal(nameof(Inquilino.Apellido))),
                            Email = reader.GetString(reader.GetOrdinal(nameof(Inquilino.Email))),
                            Dni = reader.GetString(reader.GetOrdinal(nameof(Inquilino.Dni))),
                            Telefono = reader.GetString(reader.GetOrdinal(nameof(Inquilino.Telefono))),
                        };
                    }

                }

            }
        }

        return inquilino;
    }

    // modificar inquilino
    public void ModificarInquilino(Inquilino inquilino)
    {
        using (var conexion = new MySqlConnection(ConnectionString))
        {
            var sql = @$"
            UPDATE inquilino 
            SET 
                {nameof(Inquilino.Nombre)} = @{nameof(Inquilino.Nombre)}, 
                {nameof(Inquilino.Apellido)} = @{nameof(Inquilino.Apellido)}, 
                {nameof(Inquilino.Email)} = @{nameof(Inquilino.Email)},
                {nameof(Inquilino.Dni)} = @{nameof(Inquilino.Dni)},
                {nameof(Inquilino.Telefono)} = @{nameof(Inquilino.Telefono)}
                
            WHERE 
                {nameof(Inquilino.id_inquilino)} = @{nameof(Inquilino.id_inquilino)}";

            using (var comand = new MySqlCommand(sql, conexion))
            {
                conexion.Open();
                comand.Parameters.AddWithValue($"@{nameof(Inquilino.id_inquilino)}", inquilino.id_inquilino);
                comand.Parameters.AddWithValue($"@{nameof(Inquilino.Nombre)}", inquilino.Nombre);
                comand.Parameters.AddWithValue($"@{nameof(Inquilino.Apellido)}", inquilino.Apellido);
                comand.Parameters.AddWithValue($"@{nameof(Inquilino.Email)}", inquilino.Email);
                comand.Parameters.AddWithValue($"@{nameof(Inquilino.Dni)}", inquilino.Dni);
                comand.Parameters.AddWithValue($"@{nameof(Inquilino.Telefono)}", inquilino.Telefono);

                comand.ExecuteNonQuery();
            }
        }
    }
    /*
    //eliminar inquilino
    public void EliminarInquilino(int id_inquilino)
    {
        using (var conexion = new MySqlConnection(ConnectionString))
        {
            var sql = @$"
            DELETE FROM inquilino WHERE id_inquilino = @id_inquilino";
            using (var comand = new MySqlCommand(sql, conexion))
            {
                conexion.Open();
                comand.Parameters.AddWithValue("@id_inquilino", id_inquilino);
                comand.ExecuteNonQuery();
            }
        }
    }*/

    //borrado logico
    public void EliminarInquilino(int id_inquilino)
    {
        using (var conexion = new MySqlConnection(ConnectionString))
        {
            var sql = @$"UPDATE inquilino
            SET {nameof(Inquilino.estado)} = @{nameof(Inquilino.estado)}
            WHERE 
                {nameof(Inquilino.id_inquilino)} = @{nameof(Inquilino.id_inquilino)}";
            using (var comand = new MySqlCommand(sql, conexion))
            {
                conexion.Open();
                comand.Parameters.AddWithValue($"@{nameof(Inquilino.id_inquilino)}", id_inquilino);
                comand.Parameters.AddWithValue($"@{nameof(Inquilino.estado)}", 0);
                comand.ExecuteNonQuery();
            }
        }
    }



}

