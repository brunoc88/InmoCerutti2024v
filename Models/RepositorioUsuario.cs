using MySql.Data.MySqlClient;

public class RepositorioUsuario()
{

    readonly string ConnectionString = "Server=localhost;Port=3306;Database=inmoCerutti;User=root;";


    public void CrearUsuario(Usuario usuario)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {  
            string query = "INSERT INTO usuarios (Nombre, Apellido, Email, Clave, Salt, Rol, AvatarUrl) VALUES (@Nombre, @Apellido, @Email, @Clave, @Salt, @Rol, @AvatarUrl)";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Clave", usuario.Clave);
                command.Parameters.AddWithValue("@Salt", usuario.Salt);
                command.Parameters.AddWithValue("@Rol", usuario.Rol);
                command.Parameters.AddWithValue("@AvatarUrl", usuario.AvatarUrl);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }


    public Usuario GetUsuarioByEmail(string email)
    {
        Usuario usuario = null;

        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = "SELECT * FROM usuarios WHERE Email = @Email AND Estado = 1";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Email", email);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = reader.GetInt32("Id"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Email = reader.GetString("Email"),
                        Clave = reader.GetString("Clave"),
                        Salt = reader.GetString("Salt"),
                        Rol = reader.GetString("Rol"),
                        Estado = reader.GetBoolean("Estado"),
                        AvatarUrl = reader.GetString("AvatarUrl")
                    };
                }
            }
        }

        return usuario;
    }

    public List<Usuario> GetUsuarios()
    {
        List<Usuario> usuarios = new List<Usuario>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = "SELECT * FROM usuarios WHERE Estado = 1";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    usuarios.Add(new Usuario
                    {
                        Id = reader.GetInt32("Id"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Email = reader.GetString("Email"),
                        Clave = reader.GetString("Clave"),
                        Salt = reader.GetString("Salt"),
                        Rol = reader.GetString("Rol"),
                        Estado = reader.GetBoolean("Estado"),
                        AvatarUrl = reader.GetString("AvatarUrl")
                    });
                }
            }
        }
        return usuarios;
    }

    public void EliminarUsuario(int id)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            //borrado logico
            var sql = "UPDATE usuarios SET Estado = 0 WHERE Id = @Id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }

public void EditarUsuario(Usuario usuario)
{
    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = "UPDATE usuarios SET Nombre = @Nombre, Apellido = @Apellido, Email = @Email, Clave = @Clave, AvatarUrl = @AvatarUrl, Salt = @Salt, Rol = @Rol WHERE Id = @Id";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@Id", usuario.Id);
            command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            command.Parameters.AddWithValue("@Email", usuario.Email);
            command.Parameters.AddWithValue("@Clave", usuario.Clave); // Asegúrate de que la clave esté hasheada
            command.Parameters.AddWithValue("@AvatarUrl", (object)usuario.AvatarUrl ?? DBNull.Value); // Manejo de NULL
            command.Parameters.AddWithValue("@Salt", usuario.Salt);
            command.Parameters.AddWithValue("@Rol", usuario.Rol);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}



public Usuario GetUsuarioById(int id)
{
    Usuario usuario = null;

    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = "SELECT * FROM usuarios WHERE Id = @Id";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = reader.GetInt32("Id"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Email = reader.GetString("Email"),
                        Clave = reader.GetString("Clave"),
                        AvatarUrl = reader.IsDBNull(reader.GetOrdinal("AvatarUrl")) ? null : reader.GetString("AvatarUrl"),
                        Salt = reader.GetString("Salt"),
                        Rol = reader.GetString("Rol"),
                        Estado = reader.GetBoolean("Estado") 
                    };
                }
            }
        }
    }

    return usuario;
}

}