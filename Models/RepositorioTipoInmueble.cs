using MySql.Data.MySqlClient;

public class RepositorioTipoInmueble{

    readonly string ConnectionString = "Server=localhost;Port=3306;Database=inmoCerutti;User=root;";

public void Alta(TipoInmueble tipoInmueble){
    using (var conexion = new MySqlConnection(ConnectionString)){
        var sql = "INSERT INTO TipoInmueble (tipoNombre) VALUES (@tipoNombre)";
        using (var comand = new MySqlCommand(sql, conexion)){
            comand.Parameters.AddWithValue("@tipoNombre", tipoInmueble.tipoNombre);
            conexion.Open();
            comand.ExecuteNonQuery();
        }
    }
}


public List<TipoInmueble> GetTipoInmuebles(){
    List<TipoInmueble> tipoInmuebles = new List<TipoInmueble>();

    using (var conexion = new MySqlConnection(ConnectionString)){
        var sql = "SELECT * FROM TipoInmueble WHERE Estado = 1";
        using (var comand = new MySqlCommand(sql, conexion)){
            conexion.Open();
            using (var reader = comand.ExecuteReader()){
                while (reader.Read()){
                    tipoInmuebles.Add(
                        new TipoInmueble{
                            id_tipoInmueble = reader.GetInt32("id_tipoInmueble"),
                            tipoNombre = reader.GetString("tipoNombre"),
                            Estado = reader.GetBoolean("Estado")
                        }
                    );
                }
            }
        }
    }
    
    return tipoInmuebles;
}


public void Eliminar(int id){
    using (var conexion = new MySqlConnection(ConnectionString)){
        var sql = "UPDATE TipoInmueble SET Estado = @Estado WHERE id_tipoInmueble = @id_tipoInmueble";
        using (var comand = new MySqlCommand(sql, conexion)){
            comand.Parameters.AddWithValue("@id_tipoInmueble", id);
            comand.Parameters.AddWithValue("@Estado", false); 
            conexion.Open();
            comand.ExecuteNonQuery();
        }
    }
}



public void ModificarTipoInmueble(TipoInmueble tipoInmueble){
    using (var conexion = new MySqlConnection(ConnectionString)){
        var sql = "UPDATE TipoInmueble SET tipoNombre = @tipoNombre WHERE id_tipoInmueble = @id_tipoInmueble";
        using (var comand = new MySqlCommand(sql, conexion)){
            comand.Parameters.AddWithValue("@id_tipoInmueble", tipoInmueble.id_tipoInmueble);
            comand.Parameters.AddWithValue("@tipoNombre", tipoInmueble.tipoNombre);
            conexion.Open();
            comand.ExecuteNonQuery();
        }
    }
}


public TipoInmueble GetTipoInmueble(int id)
{
    try
    {
        using (var conexion = new MySqlConnection(ConnectionString))
        {
            var sql = "SELECT * FROM TipoInmueble WHERE id_tipoInmueble = @id_tipoInmueble";
            using (var comand = new MySqlCommand(sql, conexion))
            {
                comand.Parameters.AddWithValue("@id_tipoInmueble", id);
                conexion.Open();

                using (var reader = comand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TipoInmueble
                        {
                            id_tipoInmueble = reader.GetInt32("id_tipoInmueble"),
                            tipoNombre = reader.GetString("tipoNombre"),
                            Estado = reader.GetBoolean("Estado")
                        };
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        
        throw new Exception("Error al obtener el tipo de inmueble", ex);
    }

    return null; 
}


}