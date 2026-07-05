# 🏗️ WebApiContru - API para Sistema de Gestión de Obras

API RESTful desarrollada en **.NET 8** con **Entity Framework Core** y **MySQL**, diseñada para ser consumida por un frontend en **Razor Pages**. Proporciona toda la lógica de negocio para la gestión completa de obras de construcción, incluyendo autenticación, órdenes de compra, presupuestos, empleados, materiales, herramientas, gastos y seguimiento.

---

## 📋 Tabla de Contenidos
- [Características Principales](#-características-principales)
- [Tecnologías Utilizadas](#-tecnologías-utilizadas)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Configuración Inicial](#-configuración-inicial)
- [Endpoints Disponibles](#-endpoints-disponibles)
- [Autenticación JWT](#-autenticación-jwt)
- [Cómo Probar con Swagger](#-cómo-probar-con-swagger)
- [Modelo de Datos](#-modelo-de-datos)
- [Contribución](#-contribución)
- [Licencia](#-licencia)

---

## 🚀 Características Principales

### 🔐 Autenticación y Seguridad
- Registro de usuarios con hash de contraseñas (BCrypt)
- Inicio de sesión con generación de **JWT Tokens**
- Control de acceso por roles (**Administrador**, **Empleado**, **Cliente**)
- Aprobación manual de usuarios por parte del administrador

### 📦 Módulos de Negocio

#### 🏗️ Obras
- CRUD completo de obras
- Asignación de empleados a obras
- Seguimiento de avance con porcentajes

#### 📊 Presupuestos
- Creación de presupuestos con múltiples renglones
- Cálculo automático de subtotales y totales
- Aprobación/Rechazo de presupuestos

#### 🛒 Órdenes de Compra
- Creación de órdenes con múltiples materiales
- Confirmación de entrega con **actualización automática de stock**
- Gestión de proveedores

#### 👷 Empleados
- CRUD completo de empleados
- Asignación a obras con rol específico
- Registro de horas trabajadas

#### 📦 Materiales
- Gestión de stock (entrada/salida)
- Control de stock mínimo
- Consumo de materiales en obras

#### 🛠️ Herramientas
- Control de inventario de herramientas
- Asignación y devolución a obras
- Registro de estado (Disponible/En Obra)

#### 💰 Gastos
- Registro de gastos por obra
- Categorización de gastos
- Comprobantes asociados

#### 📋 Novedades
- Registro de novedades en obras
- Seguimiento de estado (Pendiente/Revisado/Resuelto)

#### 💳 Pagos a Proveedores
- Registro de pagos por proveedor
- Historial de pagos

### 📈 Seguimiento
- Registro de avance de obras
- Porcentaje de finalización
- Imágenes de progreso

---

## 🛠️ Tecnologías Utilizadas

| Tecnología | Versión | Uso |
|------------|---------|-----|
| **.NET** | 8.0 | Framework principal |
| **C#** | 12.0 | Lenguaje de programación |
| **Entity Framework Core** | 8.0 | ORM para acceso a datos |
| **MySQL** | 8.0 | Base de datos relacional |
| **JWT** | - | Autenticación y autorización |
| **BCrypt.Net-Next** | 4.0 | Hashing de contraseñas |
| **Swagger/OpenAPI** | 3.0 | Documentación interactiva de la API |
| **Razor Pages** | - | Frontend (cliente) |

---

## 📁 Estructura del Proyecto
📁 WebApiContru/
├── 📁 Controllers/ # Controladores de la API
│ ├── AuthController.cs
│ ├── OrdenCompraController.cs
│ ├── PresupuestosController.cs
│ ├── EmpleadoController.cs
│ ├── MaterialesController.cs
│ ├── ProveedoresController.cs
│ ├── GastoController.cs
│ ├── NovedadesController.cs
│ ├── SeguimientoController.cs
│ ├── PagoProveedorController.cs
│ ├── UsuarioController.cs
│ ├── ClienteController.cs
│ ├── ObrasController.cs
│ └── HerramientaController.cs
│
├── 📁 DTOs/ # Data Transfer Objects
│ ├── AuthDTOs.cs
│ ├── OrdenCompraDTOs.cs
│ ├── PresupuestoDTOs.cs
│ ├── EmpleadoDTOs.cs
│ ├── MaterialesDTOs.cs
│ ├── ProveedorDTOs.cs
│ ├── GastosDTOs.cs
│ ├── NovedadesDTOs.cs
│ ├── SeguimientoDTOs.cs
│ ├── PagoProveedorDTOs.cs
│ ├── UsuarioDTOs.cs
│ ├── ClientesDTOs.cs
│ ├── ObrasDTOs.cs
│ └── HerramientasDTOs.cs
│
├── 📁 Entity/ # Entidades de la base de datos
│ ├── Usuario.cs
│ ├── Empleado.cs
│ ├── Rol.cs
│ ├── OrdenCompra.cs
│ ├── DetalleOrdenCompra.cs
│ ├── Presupuesto.cs
│ ├── DetallePresupuesto.cs
│ ├── GastoObra.cs
│ ├── NovedadesObra.cs
│ ├── SeguimientoObra.cs
│ ├── PagoProveedor.cs
│ ├── Proveedor.cs
│ ├── Material.cs
│ ├── StockMaterial.cs
│ ├── Obra.cs
│ ├── Cliente.cs
│ ├── Herramienta.cs
│ ├── HerramientasAsignada.cs
│ ├── EmpleadoObra.cs
│ └── MaterialObra.cs
│
├── 📁 Repository/ # Capa de acceso a datos
│ ├── 📁 Auth/
│ ├── 📁 OrdenCompra/
│ ├── 📁 Presupuestos/
│ ├── 📁 Empleados/
│ ├── 📁 Materiales/
│ ├── 📁 Proveedores/
│ ├── 📁 Gastos/
│ ├── 📁 Novedades/
│ ├── 📁 Seguimientos/
│ ├── 📁 PagosProveedores/
│ ├── 📁 Usuarios/
│ ├── 📁 Clientes/
│ ├── 📁 Obras/
│ └── 📁 Herramientas/
│
├── 📁 Services/ # Lógica de negocio
│ ├── 📁 Auth/
│ ├── 📁 OrdenCompra/
│ ├── 📁 Presupuestos/
│ ├── 📁 Empleados/
│ ├── 📁 Materiales/
│ ├── 📁 Proveedores/
│ ├── 📁 Gastos/
│ ├── 📁 Novedades/
│ ├── 📁 Seguimientos/
│ ├── 📁 PagosProveedores/
│ ├── 📁 Usuarios/
│ ├── 📁 Clientes/
│ ├── 📁 Obras/
│ └── 📁 Herramientas/
│
├── 📁 Data/ # Contexto de base de datos
│ └── DbcontruContext.cs
│
├── 📁 Properties/ # Configuración del proyecto
├── 📄 Program.cs # Punto de entrada de la aplicación
├── 📄 appsettings.json # Configuración general
├── 📄 appsettings.Development.json
├── 📄 MiWebApi.csproj # Archivo de proyecto
└── 📄 README.md # Este archivo

text

---

## ⚙️ Configuración Inicial

### 1. Clonar el repositorio
```bash
git clone https://github.com/SantiPino95/WebApiContru.git
cd WebApiContru
2. Configurar la base de datos
Crear una base de datos en MySQL y ejecutar el script de migración:

sql
CREATE DATABASE dbcontru;
3. Configurar appsettings.json
json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=dbcontru;User=root;Password=tu_contraseña;"
  },
  "Jwt": {
    "Key": "MiClaveSuperSecretaParaJWT1234567890!",
    "Issuer": "MiWebApi",
    "Audience": "MiWebApiClient"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
4. Aplicar migraciones (si usas migraciones)
bash
dotnet ef database update
5. Inicializar roles en la base de datos
sql
INSERT INTO rol (Nombre_Rol, Descripcion) VALUES 
('Administrador', 'Acceso total al sistema'),
('Empleado', 'Acceso básico para empleados'),
('Cliente', 'Acceso para clientes');
6. Ejecutar la aplicación
bash
dotnet run
7. Acceder a Swagger
text
https://localhost:5001/swagger
🔗 Endpoints Disponibles
🔐 Autenticación
Método	Endpoint	Descripción
POST	/api/Auth/register	Registro de usuario
POST	/api/Auth/login	Inicio de sesión (devuelve JWT)
🏗️ Obras
Método	Endpoint	Descripción
GET	/api/Obras	Listar todas las obras
GET	/api/Obras/{id}	Obtener detalle de una obra
POST	/api/Obras	Crear una nueva obra
PUT	/api/Obras/{id}	Actualizar una obra
DELETE	/api/Obras/{id}	Eliminar una obra (baja lógica)
📊 Presupuestos
Método	Endpoint	Descripción
GET	/api/Presupuestos	Listar todos los presupuestos
POST	/api/Presupuestos	Crear un nuevo presupuesto
PUT	/api/Presupuestos/{id}/estado	Aprobar/Rechazar presupuesto
🛒 Órdenes de Compra
Método	Endpoint	Descripción
GET	/api/OrdenCompra	Listar todas las órdenes
GET	/api/OrdenCompra/{id}	Obtener detalle de una orden
POST	/api/OrdenCompra	Crear una nueva orden
PUT	/api/OrdenCompra/{id}	Actualizar una orden
DELETE	/api/OrdenCompra/{id}	Eliminar una orden
PATCH	/api/OrdenCompra/{id}/confirmar-entrega	Confirmar entrega y actualizar stock
👷 Empleados
Método	Endpoint	Descripción
GET	/api/Empleado	Listar todos los empleados
GET	/api/Empleado/{id}	Obtener detalle de un empleado
POST	/api/Empleado	Crear un nuevo empleado
PUT	/api/Empleado/{id}	Actualizar un empleado
DELETE	/api/Empleado/{id}	Eliminar un empleado
POST	/api/Empleado/asignar	Asignar empleado a una obra
📦 Materiales
Método	Endpoint	Descripción
GET	/api/Materiales	Listar todos los materiales
POST	/api/Materiales	Crear un nuevo material
PUT	/api/Materiales/{id}	Actualizar un material
DELETE	/api/Materiales/{id}	Eliminar un material
POST	/api/Materiales/consumir	Consumir material en obra
POST	/api/Materiales/entrada	Reponer stock
🛠️ Herramientas
Método	Endpoint	Descripción
GET	/api/Herramienta	Listar todas las herramientas
POST	/api/Herramienta/asignar-salida	Asignar herramienta a obra
POST	/api/Herramienta/registrar-devolucion	Registrar devolución
💰 Gastos
Método	Endpoint	Descripción
GET	/api/Gasto	Listar todos los gastos
GET	/api/Gasto/obra/{idObra}	Listar gastos por obra
POST	/api/Gasto	Crear un nuevo gasto
PUT	/api/Gasto/{id}	Actualizar un gasto
DELETE	/api/Gasto/{id}	Eliminar un gasto
📋 Novedades
Método	Endpoint	Descripción
GET	/api/Novedades	Listar todas las novedades
POST	/api/Novedades	Crear una nueva novedad
PUT	/api/Novedades/{id}/estado	Cambiar estado de novedad
📈 Seguimiento
Método	Endpoint	Descripción
GET	/api/Seguimiento/obra/{idObra}	Listar seguimientos por obra
POST	/api/Seguimiento	Crear un nuevo seguimiento
💳 Pagos a Proveedores
Método	Endpoint	Descripción
GET	/api/PagoProveedor	Listar todos los pagos
POST	/api/PagoProveedor	Registrar un nuevo pago
👤 Usuarios
Método	Endpoint	Descripción
GET	/api/Usuario/pendientes	Listar usuarios pendientes
PUT	/api/Usuario/{id}/aprobar	Aprobar acceso de usuario
PUT	/api/Usuario/{id}/rechazar	Rechazar acceso de usuario
🔐 Autenticación JWT
Cómo obtener un token
1. Registrar un usuario
http
POST /api/Auth/register
Content-Type: application/json

{
  "email": "usuario@empresa.com",
  "password": "123456",
  "nombre": "Juan",
  "apellido": "Perez",
  "cedula": "12345678-9",
  "telefono": "099123456",
  "valorHora": 500,
  "categoria": "Oficial",
  "idRol": 2
}
2. Iniciar sesión
http
POST /api/Auth/login
Content-Type: application/json

{
  "email": "usuario@empresa.com",
  "password": "123456"
}
3. Respuesta (token JWT)
json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "usuario@empresa.com",
  "nombreCompleto": "Juan Perez",
  "rol": "Empleado",
  "idUsuario": 1,
  "expira": "2026-07-05T12:00:00"
}
4. Usar el token en endpoints protegidos
http
GET /api/Obras
Authorization: Bearer [TOKEN_AQUI]
🧪 Cómo Probar con Swagger
Ejecutar la aplicación:

bash
dotnet run
Abrir Swagger en el navegador:

text
https://localhost:5001/swagger
Registrar un usuario:

Buscar el endpoint POST /api/Auth/register

Hacer clic en "Try it out"

Completar el JSON con los datos del usuario

Hacer clic en "Execute"

Activar el usuario (si está en estado "Pendiente"):

Ejecutar en MySQL:

sql
UPDATE usuario SET Estado = 'Activo' WHERE Email = 'usuario@empresa.com';
Iniciar sesión:

Buscar el endpoint POST /api/Auth/login

Completar con email y password

Copiar el token de la respuesta

Probar un endpoint protegido:

Buscar el endpoint GET /api/Obras

Hacer clic en el candado 🔒 en la parte superior de Swagger

Pegar el token (sin "Bearer", solo el token)

Hacer clic en "Execute"

📊 Modelo de Datos
Relaciones Principales
text
┌─────────────┐      ┌─────────────┐      ┌─────────────┐
│   Usuario   │1────1│   Empleado  │      │    Obra     │
└─────────────┘      └─────────────┘      └─────────────┘
       │                    │                     │
       │1                   │N                     │1
       │                    │                     │
       ▼                    ▼                     ▼
┌─────────────┐      ┌─────────────┐      ┌─────────────┐
│     Rol     │      │EmpleadoObra │      │   Cliente   │
└─────────────┘      └─────────────┘      └─────────────┘
                            │
                            │N
                            ▼
                    ┌─────────────┐
                    │  Seguimiento │
                    └─────────────┘
Diagrama de Entidades (Simplificado)
https://ruta/de/tu/imagen.png

🖼️ Capturas de Pantalla
Swagger UI
https://ruta/de/tu/imagen.png

Ejemplo de Registro
https://ruta/de/tu/imagen.png

Ejemplo de Login
https://ruta/de/tu/imagen.png

Ejemplo de Endpoint Protegido
https://ruta/de/tu/imagen.png

🤝 Contribución
Si deseas contribuir a este proyecto, por favor:

Fork el repositorio

Crea una rama para tu funcionalidad (git checkout -b feature/nueva-funcionalidad)

Commit tus cambios (git commit -m 'Agrega nueva funcionalidad')

Push a la rama (git push origin feature/nueva-funcionalidad)

Abre un Pull Request

📝 Notas de Desarrollo
Patrón de Arquitectura
Repository Pattern: Abstracción de la capa de datos

Service Pattern: Lógica de negocio

DTO Pattern: Transferencia de datos entre capas

Dependency Injection: Inyección de dependencias

Buenas Prácticas Implementadas
✅ Validación de datos con [Required] y [EmailAddress]

✅ Hash de contraseñas con BCrypt

✅ Autenticación con JWT

✅ Manejo de excepciones

✅ Documentación automática con Swagger

✅ Código limpio y estructurado

📄 Licencia
Este proyecto está bajo la licencia MIT. Consulta el archivo LICENSE para más detalles.

📞 Contacto
Desarrollador: Santiago Pino

GitHub: SantiPino95

Email: santiago.pino@email.com

🙏 Agradecimientos
Equipo de desarrollo por el apoyo en el diseño de la API

Comunidad de .NET y C# por la documentación y ejemplos

¡Gracias por usar WebApiContru! 🚀
