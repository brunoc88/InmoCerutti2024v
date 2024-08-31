using Microsoft.AspNetCore.SignalR;
using MySql.Data.MySqlClient;

public class RepositorioContrato
{
    readonly string ConnectionString = "Server=localhost;Port=3306;Database=inmoCerutti;User=root;";

public List<Inmueble> GetInmueblesDisponibles(Filtro filtro)
{
    var inmuebles = new List<Inmueble>();

    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @"
            SELECT i.*
            FROM inmueble i
            LEFT JOIN contrato c ON i.id_inmueble = c.id_inmueble
            WHERE i.Precio BETWEEN @PrecioMin AND @PrecioMax
            AND i.Uso = @Uso
            AND i.Tipo = @Tipo
            AND i.Estado = 1
            AND (c.id_inmueble IS NULL OR c.FechaFin < @FechaInicio OR c.FechaInicio > @FechaFin OR c.Estado = 0)";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@PrecioMin", filtro.PrecioMin ?? 0);
            command.Parameters.AddWithValue("@PrecioMax", filtro.PrecioMax ?? decimal.MaxValue);
            command.Parameters.AddWithValue("@Uso", filtro.Uso ?? string.Empty);
            command.Parameters.AddWithValue("@Tipo", filtro.Tipo ?? string.Empty);
            command.Parameters.AddWithValue("@FechaInicio", filtro.FechaInicio ?? DateTime.MinValue);
            command.Parameters.AddWithValue("@FechaFin", filtro.FechaFin ?? DateTime.MaxValue);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var inmueble = new Inmueble
                    {
                        id_inmueble = Convert.ToInt32(reader["id_inmueble"]),
                        Direccion = reader["Direccion"].ToString(),
                        Uso = (UsoInmueble)Enum.Parse(typeof(UsoInmueble), reader["Uso"].ToString()),
                        Tipo = (TipoInmueble)Enum.Parse(typeof(TipoInmueble), reader["Tipo"].ToString()),
                        Ambientes = Convert.ToInt32(reader["Ambientes"]),
                        Latitud = reader.IsDBNull(reader.GetOrdinal("Latitud")) ? (decimal?)null : Convert.ToDecimal(reader["Latitud"]),
                        Longitud = reader.IsDBNull(reader.GetOrdinal("Longitud")) ? (decimal?)null : Convert.ToDecimal(reader["Longitud"]),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        id_propietario = Convert.ToInt32(reader["id_propietario"])
                    };
                    inmuebles.Add(inmueble);
                }
            }
        }
    }

    return inmuebles;
}


    public void CrearContrato(Contrato contrato)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"INSERT INTO contrato ({nameof(contrato.id_inmueble)}, {nameof(contrato.id_inquilino)}, 
                                           {nameof(contrato.Precio)}, {nameof(contrato.FechaInicio)}, {nameof(contrato.FechaFin)})
                     VALUES (@{nameof(contrato.id_inmueble)}, @{nameof(contrato.id_inquilino)}, 
                             @{nameof(contrato.Precio)}, @{nameof(contrato.FechaInicio)}, @{nameof(contrato.FechaFin)})";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(contrato.id_inmueble)}", contrato.id_inmueble);
                command.Parameters.AddWithValue($"@{nameof(contrato.id_inquilino)}", contrato.id_inquilino);
                command.Parameters.AddWithValue($"@{nameof(contrato.Precio)}", contrato.Precio);
                command.Parameters.AddWithValue($"@{nameof(contrato.FechaInicio)}", contrato.FechaInicio);
                command.Parameters.AddWithValue($"@{nameof(contrato.FechaFin)}", contrato.FechaFin);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    public List<Contrato> GetContratos()//funciona
    {
        List<Contrato> contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"
            SELECT 
                c.{nameof(Contrato.id_contrato)},
                c.{nameof(Contrato.id_inmueble)}, 
                c.{nameof(Contrato.id_inquilino)},
                c.{nameof(Contrato.Precio)}, 
                c.{nameof(Contrato.FechaInicio)}, 
                c.{nameof(Contrato.FechaFin)},
                i.{nameof(Inquilino.Nombre)} as Nombre, 
                i.{nameof(Inquilino.Apellido)} as Apellido, 
                i.{nameof(Inquilino.Dni)} as Dni, 
                i.{nameof(Inquilino.Email)} as Email, 
                i.{nameof(Inquilino.Telefono)} as Telefono, 
                inm.{nameof(Inmueble.Direccion)}, 
                inm.{nameof(Inmueble.Uso)}, 
                inm.{nameof(Inmueble.Tipo)}, 
                inm.{nameof(Inmueble.Ambientes)}, 
                inm.{nameof(Inmueble.Latitud)}, 
                inm.{nameof(Inmueble.Longitud)}, 
                inm.{nameof(Inmueble.Precio)} as PrecioInmueble
            FROM {nameof(Inquilino)} i
            JOIN {nameof(Contrato)} c ON i.{nameof(Inquilino.id_inquilino)} = c.{nameof(Contrato.id_inquilino)}
            JOIN {nameof(Inmueble)} inm ON inm.{nameof(Inmueble.id_inmueble)} = c.{nameof(Contrato.id_inmueble)}
            WHERE c.{nameof(Contrato.estado)}=1";

            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var contrato = new Contrato
                        {
                            id_contrato = reader.GetInt32(nameof(Contrato.id_contrato)),
                            id_inmueble = reader.GetInt32(nameof(Contrato.id_inmueble)),
                            id_inquilino = reader.GetInt32(nameof(Contrato.id_inquilino)),
                            Precio = reader.GetDecimal(nameof(Contrato.Precio)),
                            FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                            FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                            inquilino = new Inquilino
                            {
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                                Dni = reader.GetString("Dni"),
                                Email = reader.GetString("Email"),
                                Telefono = reader.GetString("Telefono")
                            },
                            inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                                Uso = (UsoInmueble)Enum.Parse(typeof(UsoInmueble), reader.GetString(nameof(Inmueble.Uso))),
                                Tipo = (TipoInmueble)Enum.Parse(typeof(TipoInmueble), reader.GetString(nameof(Inmueble.Tipo))),
                                Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                                Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
                                Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
                                Precio = reader.GetDecimal("PrecioInmueble")
                            }
                        };
                        contratos.Add(contrato);
                    }
                }
            }
        }
        return contratos;
    }

public Contrato GetContrato(int id)
{
    Contrato contrato = null;

    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @"
        SELECT 
            c.id_contrato,
            c.id_inmueble, 
            c.id_inquilino,
            c.Precio, 
            c.FechaInicio, 
            c.FechaFin,
            i.Nombre, 
            i.Apellido, 
            i.Dni, 
            i.Email, 
            i.Telefono, 
            inm.Direccion, 
            inm.Uso, 
            inm.Tipo, 
            inm.Ambientes, 
            inm.Latitud, 
            inm.Longitud, 
            inm.Precio AS PrecioInmueble
        FROM Contrato c
        JOIN Inquilino i ON i.id_inquilino = c.id_inquilino
        JOIN Inmueble inm ON inm.id_inmueble = c.id_inmueble
        WHERE c.id_contrato = @id_contrato";
        
        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@id_contrato", id);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    contrato = new Contrato
                    {
                        id_contrato = reader.GetInt32("id_contrato"),
                        id_inmueble = reader.GetInt32("id_inmueble"),
                        id_inquilino = reader.GetInt32("id_inquilino"),
                        Precio = reader.GetDecimal("Precio"),
                        FechaInicio = reader.GetDateTime("FechaInicio"),
                        FechaFin = reader.GetDateTime("FechaFin"),
                        inquilino = new Inquilino
                        {
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Dni = reader.GetString("Dni"),
                            Email = reader.GetString("Email"),
                            Telefono = reader.GetString("Telefono")
                        },
                        inmueble = new Inmueble
                        {
                            Direccion = reader.GetString("Direccion"),
                            Uso = (UsoInmueble)Enum.Parse(typeof(UsoInmueble), reader.GetString("Uso")),
                            Tipo = (TipoInmueble)Enum.Parse(typeof(TipoInmueble), reader.GetString("Tipo")),
                            Ambientes = reader.GetInt32("Ambientes"),
                            Latitud = reader.GetDecimal("Latitud"),
                            Longitud = reader.GetDecimal("Longitud"),
                            Precio = reader.GetDecimal("PrecioInmueble")
                        }
                    };
                }
                else
                {
                    // No se encontró el contrato con el ID proporcionado.
                    contrato = null;
                }
            }
        }
    }

    return contrato;
}

/*
    public void EliminarContrato(int id)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = "DELETE FROM contrato WHERE id_contrato = @id_contrato";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id_contrato", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
*/
public void EliminarContrato(int id)
    {
        using (var conexion = new MySqlConnection(ConnectionString))
        {
            var sql = @$"UPDATE contrato
            SET {nameof(Contrato.estado)} = @{nameof(Contrato.estado)}
            WHERE 
                {nameof(Contrato.id_contrato)} = @{nameof(Contrato.id_contrato)}";
            using (var comand = new MySqlCommand(sql, conexion))
            {
                conexion.Open();
                comand.Parameters.AddWithValue($"@{nameof(Contrato.id_contrato)}", id);
                comand.Parameters.AddWithValue($"@{nameof(Contrato.estado)}", 0);
                comand.ExecuteNonQuery();
            }
        }
    }
    public void EditarContrato(Contrato contrato)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            
            var sql = $@"
            UPDATE contrato
            SET 
                {nameof(Contrato.Precio)} = @{nameof(Contrato.Precio)},
                {nameof(Contrato.FechaInicio)} = @{nameof(Contrato.FechaInicio)},
                {nameof(Contrato.FechaFin)} = @{nameof(Contrato.FechaFin)}
            WHERE 
                {nameof(Contrato.id_contrato)} = @{nameof(Contrato.id_contrato)}";

            using (var command = new MySqlCommand(sql, connection))
            {
                
                command.Parameters.AddWithValue($"@{nameof(Contrato.Precio)}", contrato.Precio);
                command.Parameters.AddWithValue($"@{nameof(Contrato.FechaInicio)}", contrato.FechaInicio);
                command.Parameters.AddWithValue($"@{nameof(Contrato.FechaFin)}", contrato.FechaFin);
                command.Parameters.AddWithValue($"@{nameof(Contrato.id_contrato)}", contrato.id_contrato);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }


}







