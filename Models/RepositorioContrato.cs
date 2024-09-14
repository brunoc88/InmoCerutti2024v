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
        SELECT i.*, ti.tipoNombre, ti.id_tipoInmueble
        FROM inmueble i
        LEFT JOIN contrato c ON i.id_inmueble = c.id_inmueble
        LEFT JOIN tipoinmueble ti ON i.id_tipoInmueble = ti.id_tipoInmueble
        WHERE i.Precio BETWEEN @PrecioMin AND @PrecioMax
        AND i.Uso = @Uso
        AND i.id_tipoInmueble = @Tipo
        AND i.Estado = 1
        AND (c.id_inmueble IS NULL OR c.FechaFin < @FechaInicio OR c.FechaInicio > @FechaFin OR c.Estado = 0);
        ";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@PrecioMin", filtro.PrecioMin ?? 0);
            command.Parameters.AddWithValue("@PrecioMax", filtro.PrecioMax ?? decimal.MaxValue);
            command.Parameters.AddWithValue("@Uso", filtro.Uso ?? string.Empty);
            command.Parameters.AddWithValue("@Tipo", filtro.Tipo ?? 0); // Asegúrate de que `Tipo` es un entero
            command.Parameters.AddWithValue("@FechaInicio", filtro.FechaInicio ?? DateTime.MinValue);
            command.Parameters.AddWithValue("@FechaFin", filtro.FechaFin ?? DateTime.MaxValue);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                var idInmuebleIndex = reader.GetOrdinal("id_inmueble");
                var direccionIndex = reader.GetOrdinal("Direccion");
                var usoIndex = reader.GetOrdinal("Uso");
                var ambientesIndex = reader.GetOrdinal("Ambientes");
                var latitudIndex = reader.GetOrdinal("Latitud");
                var longitudIndex = reader.GetOrdinal("Longitud");
                var precioIndex = reader.GetOrdinal("Precio");
                var idPropietarioIndex = reader.GetOrdinal("id_propietario");
                var estadoIndex = reader.GetOrdinal("Estado");
                var tipoNombreIndex = reader.GetOrdinal("tipoNombre");
                var tipoIndex = reader.GetOrdinal("id_tipoInmueble");

                while (reader.Read())
                {
                    var inmueble = new Inmueble
                    {
                        id_inmueble = reader.GetInt32(idInmuebleIndex),
                        Precio = reader.GetDecimal(precioIndex),
                        Uso = reader.GetString(usoIndex),
                        Tipo = new TipoInmueble
                        {
                            id_tipoInmueble = reader.GetInt32(tipoIndex),
                            tipoNombre = reader.GetString(tipoNombreIndex)
                        },
                        estado = reader.GetByte(estadoIndex) == 1, // Conversión a bool
                        Direccion = reader.GetString(direccionIndex),
                        Ambientes = reader.GetInt32(ambientesIndex),
                        Latitud = reader.GetDecimal(latitudIndex),
                        Longitud = reader.GetDecimal(longitudIndex),
                        id_propietario = reader.GetInt32(idPropietarioIndex)
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
        var sql = @"INSERT INTO contrato (id_inmueble, id_inquilino, Precio, FechaInicio, FechaFin, id_usuario)
                    VALUES (@id_inmueble, @id_inquilino, @Precio, @FechaInicio, @FechaFin, @id_usuario)";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@id_inmueble", contrato.id_inmueble);
            command.Parameters.AddWithValue("@id_inquilino", contrato.id_inquilino);
            command.Parameters.AddWithValue("@Precio", contrato.Precio);
            command.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
            command.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
            command.Parameters.AddWithValue("@id_usuario", contrato.id_usuario); 

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}

public List<Contrato> GetContratos()
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
            inm.{nameof(Inmueble.Ambientes)}, 
            inm.{nameof(Inmueble.Latitud)}, 
            inm.{nameof(Inmueble.Longitud)}, 
            inm.{nameof(Inmueble.Precio)} as PrecioInmueble,
            ti.{nameof(TipoInmueble.id_tipoInmueble)},
            ti.{nameof(TipoInmueble.tipoNombre)},
            ti.{nameof(TipoInmueble.Estado)}
        FROM {nameof(Contrato)} c
        JOIN {nameof(Inquilino)} i ON i.{nameof(Inquilino.id_inquilino)} = c.{nameof(Contrato.id_inquilino)}
        JOIN {nameof(Inmueble)} inm ON inm.{nameof(Inmueble.id_inmueble)} = c.{nameof(Contrato.id_inmueble)}
        JOIN {nameof(TipoInmueble)} ti ON ti.{nameof(TipoInmueble.id_tipoInmueble)} = inm.{nameof(Inmueble.id_tipoInmueble)}
        WHERE c.{nameof(Contrato.estado)} = 1";

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
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Tipo = new TipoInmueble
                            {
                                id_tipoInmueble = reader.GetInt32(nameof(TipoInmueble.id_tipoInmueble)),
                                tipoNombre = reader.GetString(nameof(TipoInmueble.tipoNombre)),
                                Estado = reader.GetBoolean(nameof(TipoInmueble.Estado))
                            },
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
        //tuve que usar alias para evitar problemas de mapeo
        var sql = @"
        SELECT 
            c.id_contrato,
            c.id_inmueble, 
            c.id_inquilino,
            c.Precio, 
            c.FechaInicio, 
            c.FechaFin,
            i.Nombre AS InquilinoNombre, 
            i.Apellido AS InquilinoApellido, 
            i.Dni AS InquilinoDni, 
            i.Email AS InquilinoEmail, 
            i.Telefono AS InquilinoTelefono, 
            inm.Direccion, 
            inm.Uso, 
            ti.tipoNombre AS TipoInmueble, 
            inm.Ambientes, 
            inm.Latitud, 
            inm.Longitud, 
            inm.Precio AS PrecioInmueble,
            u.Nombre AS UsuarioNombre,
            u.Apellido AS UsuarioApellido
        FROM Contrato c
        JOIN Inquilino i ON i.id_inquilino = c.id_inquilino
        JOIN Inmueble inm ON inm.id_inmueble = c.id_inmueble
        JOIN tipoInmueble ti ON ti.id_tipoInmueble = inm.id_tipoInmueble
        JOIN usuarios u ON u.Id = c.id_usuario
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
                            Nombre = reader.GetString("InquilinoNombre"),
                            Apellido = reader.GetString("InquilinoApellido"),
                            Dni = reader.GetString("InquilinoDni"),
                            Email = reader.GetString("InquilinoEmail"),
                            Telefono = reader.GetString("InquilinoTelefono")
                        },
                        inmueble = new Inmueble
                        {
                            Direccion = reader.GetString("Direccion"),
                            Uso = reader.GetString("Uso"),
                            Tipo = new TipoInmueble
                            {
                                tipoNombre = reader.GetString("TipoInmueble")
                            },
                            Ambientes = reader.GetInt32("Ambientes"),
                            Latitud = reader.GetDecimal("Latitud"),
                            Longitud = reader.GetDecimal("Longitud"),
                            Precio = reader.GetDecimal("PrecioInmueble")
                        },
                        usuario = new Usuario
                        {
                            Nombre = reader.GetString("UsuarioNombre"),
                            Apellido = reader.GetString("UsuarioApellido")
                        }
                    };
                }
            }
        }
    }

    return contrato;
   
}



//para pagos
public List<Contrato> GetContratoPorDni(string dni)
{
    List<Contrato> contratos = new List<Contrato>();

    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @"
        SELECT 
            c.id_contrato,
            c.Precio, 
            i.Nombre, 
            i.Apellido, 
            i.Dni,
            inm.Direccion,
            inm.Precio AS PrecioInmueble
        FROM Contrato c
        JOIN Inquilino i ON i.id_inquilino = c.id_inquilino
        JOIN Inmueble inm ON inm.id_inmueble = c.id_inmueble
        WHERE i.Dni = @dni";
        
        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@dni", dni);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                   var contrato = new Contrato
                    {
                        id_contrato = reader.GetInt32("id_contrato"),
                        Precio = reader.GetDecimal("Precio"),
                        inquilino = new Inquilino
                        {
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Dni = reader.GetString("Dni")
                        },
                        inmueble = new Inmueble
                        {
                            Direccion = reader.GetString("Direccion"),
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
//para pagos
public List<Contrato> GetContratoPorEmail(string email)
{
    List<Contrato> contratos = new List<Contrato>();

    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @"
        SELECT 
            c.id_contrato,
            c.Precio, 
            i.Nombre, 
            i.Apellido, 
            i.Dni,
            inm.Direccion,
            inm.Precio AS PrecioInmueble
        FROM Contrato c
        JOIN Inquilino i ON i.id_inquilino = c.id_inquilino
        JOIN Inmueble inm ON inm.id_inmueble = c.id_inmueble
        WHERE i.Email = @Email";
        
        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@Email", email);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                   var contrato = new Contrato
                    {
                        id_contrato = reader.GetInt32("id_contrato"),
                        Precio = reader.GetDecimal("Precio"),
                        inquilino = new Inquilino
                        {
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Dni = reader.GetString("Dni")
                        },
                        inmueble = new Inmueble
                        {
                            Direccion = reader.GetString("Direccion"),
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







