using MySql.Data.MySqlClient;

public class RepositorioPago(){
    readonly string ConnectionString = "Server=localhost;Port=3306;Database=inmoCerutti;User=root;";
public void Pagar(Pago pago)
{
    using (var conexion = new MySqlConnection(ConnectionString))
    {
        var sql = "INSERT INTO pago (FechaDePago, Motivo, Importe, id_contrato) VALUES (@FechaDePago, @Motivo, @Importe, @id_contrato)";
        using (var command = new MySqlCommand(sql, conexion))
        {
            command.Parameters.AddWithValue("@FechaDePago", pago.FechaDePago);
            command.Parameters.AddWithValue("@Motivo", pago.Motivo);
            command.Parameters.AddWithValue("@Importe", pago.Importe);
            command.Parameters.AddWithValue("@id_contrato", pago.id_contrato);

            try
            {
                conexion.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al realizar el pago", ex);
            }
        }
    }
}

public void EliminarPago(int id_pago){
    {
        using (var conexion = new MySqlConnection(ConnectionString))
        {
            var sql = @$"UPDATE pago
            SET {nameof(Pago.Estado)} = @{nameof(Pago.Estado)}
            WHERE 
                {nameof(Pago.id_pago)} = @{nameof(Pago.id_pago)}";
            using (var comand = new MySqlCommand(sql, conexion))
            {
                conexion.Open();
                comand.Parameters.AddWithValue($"@{nameof(Pago.id_pago)}", id_pago);
                comand.Parameters.AddWithValue($"@{nameof(Pago.Estado)}", 0);
                comand.ExecuteNonQuery();
            }
        }
    }
}


public List<Pago> GetPagos(){
    using (var conexion = new MySqlConnection(ConnectionString)){
        var sql = @"SELECT * FROM pago p 
                    JOIN contrato c ON c.id_contrato = p.id_contrato 
                    JOIN inquilino i ON i.id_inquilino = c.id_inquilino 
                    JOIN inmueble inm ON inm.id_inmueble = c.id_inmueble
                    WHERE p.Estado = 1";
        using (var command = new MySqlCommand(sql, conexion))
        {
            conexion.Open();
            using (var reader = command.ExecuteReader())
            {
                var pagos = new List<Pago>();
                while (reader.Read())
                {   
                    var pago = new Pago() 
                    {
                        id_pago = reader.GetInt32("id_pago"),
                        FechaDePago = reader.GetDateTime("FechaDePago"),
                        Importe = reader.GetDecimal("Importe"),
                        Motivo = !reader.IsDBNull(reader.GetOrdinal("Motivo")) 
                                    ? reader.GetString("Motivo") 
                                    : null, // Verifica si Motivo es nulo
                        contrato = new Contrato(){
                            id_contrato = reader.GetInt32("id_contrato"),
                            FechaInicio = reader.GetDateTime("FechaInicio"),
                            FechaFin = reader.GetDateTime("FechaFin"),
                            inquilino = new Inquilino(){
                                id_inquilino = reader.GetInt32("id_inquilino"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                                Dni = reader.GetString("Dni")
                            },
                            inmueble = new Inmueble(){
                                Direccion = reader.GetString("Direccion")
                            }
                        }
                    };
                    pagos.Add(pago); 
                }
                return pagos;
            }
        }
    }
}


}