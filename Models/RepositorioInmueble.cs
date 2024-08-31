using System.Windows.Markup;
using MySql.Data.MySqlClient;

public class RepositorioInmueble
{
    readonly string ConnectionString = "Server=localhost;Port=3306;Database=inmoCerutti;User=root;";


 public List<Inmueble> GetInmuebles()
{
    List<Inmueble> lista = new List<Inmueble>();

    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = $@"
            SELECT 
                i.{nameof(Inmueble.id_inmueble)},
                i.{nameof(Inmueble.Direccion)},
                i.{nameof(Inmueble.Ambientes)},
                i.{nameof(Inmueble.Precio)},
                i.{nameof(Inmueble.Longitud)},
                i.{nameof(Inmueble.Latitud)},
                i.{nameof(Inmueble.id_propietario)},
                p.{nameof(Propietario.Nombre)},
                p.{nameof(Propietario.Apellido)},
                p.{nameof(Propietario.Dni)},
                p.{nameof(Propietario.Email)},
                p.{nameof(Propietario.Telefono)}
            FROM inmueble i 
            INNER JOIN propietario p 
            ON i.{nameof(Inmueble.id_propietario)} = p.{nameof(Propietario.id_propietario)}
            WHERE i.Estado = 1";

        using (var command = new MySqlCommand(sql, connection))
        {
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(new Inmueble
                    {
                        id_inmueble = reader.GetInt32(nameof(Inmueble.id_inmueble)),
                        Direccion = reader[nameof(Inmueble.Direccion)] == DBNull.Value ? string.Empty : reader.GetString(nameof(Inmueble.Direccion)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
                        Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
                        id_propietario = reader.GetInt32(nameof(Inmueble.id_propietario)),
                        duenio = new Propietario
                        {
                            id_propietario = reader.GetInt32(nameof(Propietario.id_propietario)),
                            Nombre = reader[nameof(Propietario.Nombre)] == DBNull.Value ? string.Empty : reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader[nameof(Propietario.Apellido)] == DBNull.Value ? string.Empty : reader.GetString(nameof(Propietario.Apellido)),
                            Dni = reader[nameof(Propietario.Dni)] == DBNull.Value ? string.Empty : reader.GetString(nameof(Propietario.Dni)),
                            Email = reader[nameof(Propietario.Email)] == DBNull.Value ? string.Empty : reader.GetString(nameof(Propietario.Email)),
                            Telefono = reader[nameof(Propietario.Telefono)] == DBNull.Value ? string.Empty : reader.GetString(nameof(Propietario.Telefono))
                        }
                    });
                }
            }
        }
    }
    return lista;
}



    public void AltaInmueble(Inmueble inmueble)
    {   //recordatorio: esto es otra forma en vez del nameof
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @"INSERT INTO inmueble (Direccion, Ambientes, Latitud, Longitud, Precio, id_propietario) 
                    VALUES (@Direccion, @Ambientes, @Latitud, @Longitud, @Precio, @id_propietario)";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
                command.Parameters.AddWithValue("@Ambientes", inmueble.Ambientes);
                command.Parameters.AddWithValue("@Latitud", inmueble.Latitud);
                command.Parameters.AddWithValue("@Longitud", inmueble.Longitud);
                command.Parameters.AddWithValue("@Precio", inmueble.Precio);
                command.Parameters.AddWithValue("@id_propietario", inmueble.id_propietario);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
/*
    public void eliminarPropiedad(int id)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            //usa name of para la consulta
            var sql = @"DELETE FROM inmueble WHERE id_inmueble = @id_inmueble";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@id_inmueble", id);
                command.ExecuteNonQuery();
            }
        }
    }
*/
   public void EditarInmueble(Inmueble inmueble)
{
    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @$"UPDATE inmueble 
                    SET {nameof(Inmueble.Direccion)} = @{nameof(Inmueble.Direccion)},
                        {nameof(Inmueble.Ambientes)} = @{nameof(Inmueble.Ambientes)},
                        {nameof(Inmueble.Latitud)} = @{nameof(Inmueble.Latitud)},
                        {nameof(Inmueble.Longitud)} = @{nameof(Inmueble.Longitud)},
                        {nameof(Inmueble.Precio)} = @{nameof(Inmueble.Precio)},
                        {nameof(Inmueble.id_propietario)} = @{nameof(Inmueble.id_propietario)}
                    WHERE {nameof(Inmueble.id_inmueble)} = @{nameof(Inmueble.id_inmueble)}";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue($"@{nameof(Inmueble.Direccion)}", inmueble.Direccion);
            command.Parameters.AddWithValue($"@{nameof(Inmueble.Ambientes)}", inmueble.Ambientes);
            command.Parameters.AddWithValue($"@{nameof(Inmueble.Latitud)}", inmueble.Latitud);
            command.Parameters.AddWithValue($"@{nameof(Inmueble.Longitud)}", inmueble.Longitud);
            command.Parameters.AddWithValue($"@{nameof(Inmueble.Precio)}", inmueble.Precio);
            command.Parameters.AddWithValue($"@{nameof(Inmueble.id_propietario)}", inmueble.id_propietario);
            command.Parameters.AddWithValue($"@{nameof(Inmueble.id_inmueble)}", inmueble.id_inmueble);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}

public Inmueble GetInmueble(int id_inmueble)
{
    Inmueble inmueble = null;

    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @$"SELECT 
                        {nameof(Inmueble.id_inmueble)},
                        {nameof(Inmueble.Direccion)},
                        {nameof(Inmueble.Uso)},
                        {nameof(Inmueble.Tipo)},
                        {nameof(Inmueble.Ambientes)},
                        {nameof(Inmueble.Latitud)},
                        {nameof(Inmueble.Longitud)},
                        {nameof(Inmueble.Precio)},
                        {nameof(Inmueble.id_propietario)}
                     FROM inmueble 
                     WHERE {nameof(Inmueble.id_inmueble)} = @{nameof(Inmueble.id_inmueble)}";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue($"@{nameof(Inmueble.id_inmueble)}", id_inmueble);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    inmueble = new Inmueble
                    {
                        id_inmueble = Convert.ToInt32(reader[nameof(Inmueble.id_inmueble)]),
                        Direccion = reader[nameof(Inmueble.Direccion)].ToString(),
                        Uso = (UsoInmueble)Enum.Parse(typeof(UsoInmueble), reader[nameof(Inmueble.Uso)].ToString()),
                        Tipo = (TipoInmueble)Enum.Parse(typeof(TipoInmueble), reader[nameof(Inmueble.Tipo)].ToString()),
                        Ambientes = Convert.ToInt32(reader[nameof(Inmueble.Ambientes)]),
                        Latitud = reader.IsDBNull(reader.GetOrdinal(nameof(Inmueble.Latitud))) ? 0 : Convert.ToDecimal(reader[nameof(Inmueble.Latitud)]),
                        Longitud = reader.IsDBNull(reader.GetOrdinal(nameof(Inmueble.Longitud))) ? 0 : Convert.ToDecimal(reader[nameof(Inmueble.Longitud)]),
                        Precio = Convert.ToDecimal(reader[nameof(Inmueble.Precio)]),
                        id_propietario = Convert.ToInt32(reader[nameof(Inmueble.id_propietario)])
                    };
                }
            }
        }
    }

    return inmueble;
}


public void eliminarPropiedad(int id_inmueble)
    {
        using (var conexion = new MySqlConnection(ConnectionString))
        {
            var sql = @$"UPDATE inmueble
            SET {nameof(Inmueble.estado)} = @{nameof(Inmueble.estado)}
            WHERE 
                {nameof(Inmueble.id_inmueble)} = @{nameof(Inmueble.id_inmueble)}";
            using (var comand = new MySqlCommand(sql, conexion))
            {
                conexion.Open();
                comand.Parameters.AddWithValue($"@{nameof(Inmueble.id_inmueble)}", id_inmueble);
                comand.Parameters.AddWithValue($"@{nameof(Inmueble.estado)}", 0);
                comand.ExecuteNonQuery();
            }
        }
    }



}
