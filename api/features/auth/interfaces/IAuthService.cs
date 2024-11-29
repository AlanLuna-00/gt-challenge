namespace api.features.auth.interfaces;

using api.features.users.entities;

public interface IAuthService
{
    /// <summary>
    /// Genera un token JWT para un usuario autenticado.
    /// </summary>
    /// <param name="user">La entidad del usuario para la que se genera el token.</param>
    /// <returns>Un string con el token JWT.</returns>
    string GenerateJwtToken(User user);
}
