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
    i.{nameof(Inmueble.id_tipoInmueble)},
    i.{nameof(Inmueble.Uso)},
    i.{nameof(Inmueble.id_propietario)},
    p.{nameof(Propietario.Nombre)},
    p.{nameof(Propietario.Apellido)},
    p.{nameof(Propietario.Dni)},
    p.{nameof(Propietario.Email)},
    p.{nameof(Propietario.Telefono)},
    ti.{nameof(TipoInmueble.tipoNombre)},  
    ti.{nameof(TipoInmueble.Estado)}   
FROM inmueble i 
INNER JOIN propietario p 
ON i.{nameof(Inmueble.id_propietario)} = p.{nameof(Propietario.id_propietario)}
INNER JOIN tipoinmueble ti
ON i.{nameof(Inmueble.id_tipoInmueble)} = ti.{nameof(TipoInmueble.id_tipoInmueble)}  -- Unión con TipoInmueble
WHERE i.Estado = 1 ORDER BY {nameof(Inmueble.id_inmueble)} ASC";


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
                            Tipo = new TipoInmueble
                            {
                                id_tipoInmueble = reader.GetInt32(nameof(TipoInmueble.id_tipoInmueble)),
                                tipoNombre = reader.GetString(nameof(TipoInmueble.tipoNombre)),
                                Estado = reader.GetBoolean(nameof(TipoInmueble.Estado))
                            },
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
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
            var sql = @"INSERT INTO inmueble (Direccion, Ambientes, Latitud, Longitud, Precio, id_tipoInmueble, Uso, id_propietario) 
                    VALUES (@Direccion, @Ambientes, @Latitud, @Longitud, @Precio, @id_tipoInmueble, @Uso, @id_propietario)";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
                command.Parameters.AddWithValue("@Ambientes", inmueble.Ambientes);
                command.Parameters.AddWithValue("@Latitud", inmueble.Latitud);
                command.Parameters.AddWithValue("@Longitud", inmueble.Longitud);
                command.Parameters.AddWithValue("@Precio", inmueble.Precio);
                command.Parameters.AddWithValue("@id_tipoInmueble", inmueble.id_tipoInmueble);
                command.Parameters.AddWithValue("@Uso", inmueble.Uso);
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
                        {nameof(Inmueble.Uso)} = @{nameof(Inmueble.Uso)},
                        {nameof(Inmueble.id_tipoInmueble)} = @{nameof(Inmueble.id_tipoInmueble)},
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
                command.Parameters.AddWithValue($"@{nameof(Inmueble.id_tipoInmueble)}", inmueble.id_tipoInmueble);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Uso)}", inmueble.Uso);
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
            i.{nameof(Inmueble.id_inmueble)},
            i.{nameof(Inmueble.Direccion)},
            i.{nameof(Inmueble.Uso)},
            i.{nameof(Inmueble.id_tipoInmueble)},
            ti.{nameof(TipoInmueble.tipoNombre)},
            ti.{nameof(TipoInmueble.Estado)},
            i.{nameof(Inmueble.Ambientes)},
            i.{nameof(Inmueble.Latitud)},
            i.{nameof(Inmueble.Longitud)},
            i.{nameof(Inmueble.Precio)},
            i.{nameof(Inmueble.id_propietario)}
        FROM inmueble i 
        INNER JOIN tipoinmueble ti 
        ON i.{nameof(Inmueble.id_tipoInmueble)} = ti.{nameof(TipoInmueble.id_tipoInmueble)}
        WHERE i.{nameof(Inmueble.id_inmueble)} = @{nameof(Inmueble.id_inmueble)}";

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
                        id_inmueble = reader.GetInt32(nameof(Inmueble.id_inmueble)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Uso = reader.GetString(nameof(Inmueble.Uso)),
                        Tipo = new TipoInmueble
                        {
                            id_tipoInmueble = reader.GetInt32(nameof(Inmueble.id_tipoInmueble)),
                            tipoNombre = reader.GetString("tipoNombre"), // Alias correcto
                            Estado = reader.GetBoolean(nameof(TipoInmueble.Estado))
                        },
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Latitud = reader.IsDBNull(reader.GetOrdinal(nameof(Inmueble.Latitud))) ? 0 : reader.GetDecimal(nameof(Inmueble.Latitud)),
                        Longitud = reader.IsDBNull(reader.GetOrdinal(nameof(Inmueble.Longitud))) ? 0 : reader.GetDecimal(nameof(Inmueble.Longitud)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        id_propietario = reader.GetInt32(nameof(Inmueble.id_propietario))
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
