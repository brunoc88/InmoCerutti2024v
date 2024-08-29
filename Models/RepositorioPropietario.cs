using MySql.Data.MySqlClient;


public class RepositorioPropietario
{
    readonly string ConnectionString = "Server=localhost;Port=3306;Database=inmoCerutti;User=root;";

    //guardar inquilino
    public void AltaPropietario(Propietario propietario)
    {
        
            using (var conexion = new MySqlConnection(ConnectionString))
            {
                var sql = $"INSERT INTO propietario ({nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Email)},{nameof(Propietario.Dni)},{nameof(Propietario.Telefono)}) " +
                          $"VALUES (@{nameof(Propietario.Nombre)}, @{nameof(Propietario.Apellido)}, @{nameof(Propietario.Email)}, @{nameof(Propietario.Dni)},@{nameof(Propietario.Telefono)})";

                using (var comand = new MySqlCommand(sql, conexion))
                {
                    // Agregar los parámetros usando nameof para evitar errores tipográficos
                    comand.Parameters.AddWithValue($"@{nameof(Inquilino.Nombre)}", propietario.Nombre);
                    comand.Parameters.AddWithValue($"@{nameof(Inquilino.Apellido)}", propietario.Apellido);
                    comand.Parameters.AddWithValue($"@{nameof(Inquilino.Email)}", propietario.Email);
                    comand.Parameters.AddWithValue($"@{nameof(Inquilino.Dni)}", propietario.Dni);
                    comand.Parameters.AddWithValue($"@{nameof(Inquilino.Telefono)}", propietario.Telefono);

                    conexion.Open();
                    comand.ExecuteNonQuery();
                }
            }
        
       
    }


    public List<Propietario> GetPropietarios()
    {
        List<Propietario> res = new List<Propietario>();
        using (var conexion = new MySqlConnection(ConnectionString))
        {
            string sql = @$"
					SELECT 
						{nameof(Propietario.id_propietario)}, 
						{nameof(Propietario.Nombre)},
                        {nameof(Propietario.Apellido)},
                        {nameof(Propietario.Email)},
                        {nameof(Propietario.Dni)},
                        {nameof(Propietario.Telefono)}
                       
					FROM propietario";
            //tambien si quiere mostrar todo hago SELECT * FROM inquilino
            using (var comand = new MySqlCommand(sql, conexion))
            {
                conexion.Open();
                var reader = comand.ExecuteReader();
                while (reader.Read())
                {
                    res.Add(new Propietario
                    {
                        id_propietario = reader.GetInt32(nameof(Propietario.id_propietario)),
                        Nombre = reader.GetString(nameof(Propietario.Nombre)),
                        Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        Email = reader.GetString(nameof(Propietario.Email)),
                        Dni = reader.GetString(nameof(Propietario.Dni)),
                        Telefono = reader.GetString(nameof(Propietario.Telefono))
                    });
                }
                reader.Close();

            }
        }
        return res;
    }

    public Propietario GetPropietario(int id)
    {
        var propietario = new Propietario();
        using (var conexion = new MySqlConnection(ConnectionString))
        {
            var sql = "SELECT * FROM propietario WHERE id_propietario = @id_propietario";
            using (var comand = new MySqlCommand(sql, conexion))
            {
                conexion.Open();
                comand.Parameters.AddWithValue("@id_propietario", id);

                using (var reader = comand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        propietario.id_propietario = reader.GetInt32(nameof(Propietario.id_propietario));
                        propietario.Nombre = reader.GetString(nameof(Propietario.Nombre));
                        propietario.Apellido = reader.GetString(nameof(Propietario.Apellido));
                        propietario.Email = reader.GetString(nameof(Propietario.Email));
                        propietario.Dni = reader.GetString(nameof(Propietario.Dni));
                        propietario.Telefono = reader.GetString(nameof(Propietario.Telefono));
                    }

                }

            }
        }

        return propietario; 
    }

    // modificar inquilino
    public void ModificarPropietario(Propietario propietario)
    {
        using (var conexion = new MySqlConnection(ConnectionString))
        {
            var sql = @$"
            UPDATE propietario 
            SET 
                {nameof(Propietario.Nombre)} = @{nameof(Propietario.Nombre)}, 
                {nameof(Propietario.Apellido)} = @{nameof(Propietario.Apellido)}, 
                {nameof(Propietario.Email)} = @{nameof(Propietario.Email)},
                {nameof(Propietario.Dni)} = @{nameof(Propietario.Dni)},
                {nameof(Propietario.Telefono)} = @{nameof(Propietario.Telefono)}
                
            WHERE 
                {nameof(Propietario.id_propietario)} = @{nameof(Propietario.id_propietario)}";

            using (var comand = new MySqlCommand(sql, conexion))
            {
                conexion.Open();
                comand.Parameters.AddWithValue($"@{nameof(Propietario.id_propietario)}", propietario.id_propietario);
                comand.Parameters.AddWithValue($"@{nameof(Propietario.Nombre)}", propietario.Nombre);
                comand.Parameters.AddWithValue($"@{nameof(Propietario.Apellido)}", propietario.Apellido);
                comand.Parameters.AddWithValue($"@{nameof(Propietario.Email)}", propietario.Email);
                comand.Parameters.AddWithValue($"@{nameof(Propietario.Dni)}", propietario.Dni);
                comand.Parameters.AddWithValue($"@{nameof(Propietario.Telefono)}", propietario.Telefono);

                comand.ExecuteNonQuery();
            }
        }
    }

    //eliminar inquilino
    public void EliminarPropietario(int id_propietario)
    {
        using (var conexion = new MySqlConnection(ConnectionString))
        {
            var sql = @$"
            DELETE FROM propietario WHERE id_propietario = @id_propietario";
            using (var comand = new MySqlCommand(sql, conexion))
            {
                conexion.Open();
                comand.Parameters.AddWithValue("@id_propietario", id_propietario);
                comand.ExecuteNonQuery();
            }
        }
    }


}
