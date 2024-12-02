# Gestión de Usuarios, Productos y Órdenes con ASP.NET y Blazor

Este proyecto es una solución integral desarrollada en **ASP.NET Core** para el back-end y **Blazor WebAssembly** para el front-end. Su arquitectura modular permite una gestión eficiente de usuarios, productos y órdenes, mientras que la separación de responsabilidades facilita el mantenimiento y la escalabilidad.

## Estructura del proyecto

### Back-End: API

- **Framework:** ASP.NET Core 8.0.
- **Arquitectura modular:** Cada funcionalidad está separada en módulos dentro de la carpeta `features`.
  - Ejemplo: `features/users` contiene `entities`, `DTOs`, `controllers`, `services`, `repository`, e `interfaces`.
- **Base de datos:** PostgreSQL.
- **Gestión de autenticación y autorización:** Uso de JWT con roles (`Admin`, `Gerente`, `Empleado`).
- **Excepciones personalizadas:**
  - Implementación de clases específicas para manejar errores comunes (`NotFoundException`, `ValidationException`, entre otros).
  - Uso de un **middleware** centralizado para capturar y transformar estas excepciones en respuestas HTTP apropiadas.

Directorio del API:
```
api/
├── Data/                   # Inicialización de datos
├── domain/                 # Conexión con la base de datos
├── features/               # Módulos organizados por funcionalidad
│   ├── users/              # Gestión de usuarios
│   ├── products/           # Gestión de productos
│   ├── orders/             # Gestión de órdenes
├── Migrations/             # Archivos de migración de Entity Framework Core
├── Shared/                 # Excepciones, DTOs y Middlewares
├── appsettings.json        # Configuración de la API
├── compose.yaml            # Archivo de configuración para la API
```

### Front-End: Cliente

- **Framework:** Blazor WebAssembly.
- **Gestión de la UI:** Páginas distribuidas por funcionalidad en la carpeta `Pages`.
- **Servicios de comunicación:** Consumición de la API mediante `HttpClient`.
- **Autenticación:**
  - Implementación de un **CustomAuthStateProvider** para manejar el estado del usuario autenticado.
  - Tokens JWT almacenados en el cliente y validados en cada solicitud.
- **Interfaz de usuario:** Tablas con búsqueda, paginación y soporte para filtros personalizados.

Directorio del cliente:
```
client/
├── Pages/                  # Páginas Razor organizadas por funcionalidad
│   ├── Users/              # Gestión de usuarios
│   ├── Products/           # Gestión de productos
│   ├── Orders/             # Gestión de órdenes
├── Services/               # Servicios para consumo de la API
├── Shared/                 # DTOs compartidos y utilidades, como el state
├── App.razor               # Entrada principal de Blazor
├── MainLayout.razor        # Layout base para las páginas
```

## Funcionalidades clave

### Back-End
- **Gestión de Usuarios:**
  - Registro, actualización, eliminación lógica y cambio de contraseñas.
  - Control de roles: `Admin`, `Gerente`, `Empleado`.
- **Gestión de Productos:**
  - ABM de productos, con soporte para búsqueda y eliminación lógica.
  - Filtros para incluir o excluir productos eliminados.
- **Gestión de Órdenes:**
  - Creación de órdenes, cambio de estados, y manejo de stock según el estado de la orden.
- **Middleware para excepciones:**
  - Captura de excepciones personalizadas (`NotFoundException`, `ConflictException`, etc.) y transformación en respuestas HTTP consistentes.

### Front-End
- **Gestión de usuarios, productos y órdenes:**
  - Páginas de creación, edición, listado y detalle.
- **Autenticación:**
  - Uso de `CustomAuthStateProvider` para manejar el estado del usuario.
  - Control de acceso a vistas y acciones basado en roles.
- **Búsqueda y filtros avanzados:**
  - Búsqueda de productos con filtros para incluir o excluir elementos eliminados.

## Cómo ejecutar

### Requisitos previos
- [.NET SDK 8.0](https://dotnet.microsoft.com/) para pruebas locales.
- Editor de código como Visual Studio o en mi caso, JetBrains Riders.

### Pasos
1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/AlanLuna-00/gt-challenge.git
   cd gt-challenges
   ```

2. **Levantar base de datos dockerizada:**
   ```bash
   cd api
   docker compose up -d
   ```

3. **Restaurar dependencias del API:**
   ```bash
   cd api
   dotnet restore
   ```

4. **Aplicar migraciones:**
   ```bash
   cd api
   dotnet tool install --global dotnet-ef # Instalacion de dotnet en tu terminal
   dotnet ef database update # Aplicar migraciones
   ```
  
3. **Restaurar dependencias del API:**
   ```bash
   cd api
   dotnet restore
   ```

5. **Restaurar dependencias del cliente:**
   ```bash
   cd ../client
   dotnet restore
   ```

6. **Ejecutar ambos proyectos:**
   - **API:** `dotnet run` dentro de la carpeta `api`.
   - **Cliente:** `dotnet run` dentro de la carpeta `client`.

7. Acceso:
   - **API:** `http://localhost:5274/swagger/index.html`
   - **Cliente:** `http://localhost:5256`