# 🛒 EcommerceApp - ASP.NET Core Clean Architecture

Este proyecto es una aplicación de ecommerce construida utilizando **ASP.NET Core MVC** siguiendo los principios de **Clean Architecture** y **SOLID**, lo que permite un desarrollo limpio, escalable y mantenible.

---

## 📁 Estructura del Proyecto

```
project_ecommerce/
├── Ecommerce.Domain            # Núcleo del negocio: entidades y contratos
│   └── Entities/              # Clases como Product, Category
│
├── Ecommerce.Application       # Lógica de aplicación (casos de uso)
│   ├── Interfaces/            # IProductService, ICategoryService
│   └── Services/             # Implementaciones de servicios
│
├── Ecommerce.Infrastructure    # Implementaciones técnicas
│   ├── Data/                  # AppDbContext y EF Core
│   ├── Interfaces/            # Contratos de repositorios (ICategoryRepository)
│   └── Repositories/          # Repositorios concretos usando EF Core
│
├── EcommerceApp.Web            # Capa de presentación (ASP.NET MVC)
│   └── Controllers/           # Controladores MVC (ProductController)
```

---

## ⚙️ Tecnologías Utilizadas

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server (o SQLite)
- .NET 7.0 / .NET 8.0
- Bootstrap (opcional)
- AutoMapper (opcional)
- FluentValidation (opcional)

---

## 🚀 Instalación y Ejecución

### 1. Clona el proyecto

```bash
git clone https://github.com/tu_usuario/project_ecommerce.git
cd project_ecommerce
```

### 2. Configura la cadena de conexión

Edita el archivo `appsettings.json` en `EcommerceApp.Web`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=EcommerceDb;Trusted_Connection=True;"
}
```

### 3. Aplica las migraciones

```bash
cd EcommerceApp.Web
dotnet ef database update
```

### 4. Ejecuta la aplicación

```bash
dotnet run --project EcommerceApp.Web
```

---

## 📚 Principios Aplicados

### ✅ Clean Architecture

- Separación por capas: `Domain`, `Application`, `Infrastructure`, `Web`.
- Independencia de tecnologías y frameworks en el núcleo del negocio.

### ✅ SOLID

- **S**: Cada clase tiene una única responsabilidad.
- **O**: El código está abierto a extensión, cerrado a modificación.
- **L**: Sustitución de interfaces respetando contratos.
- **I**: Interfaces pequeñas y específicas.
- **D**: Inyección de dependencias para acoplamiento mínimo.

---

## ✅ Funcionalidades actuales

- CRUD de productos
- CRUD de categorías
- Vista de listado y detalles
- Uso de DropDowns para categorías
- Separación por responsabilidades
- Inyección de dependencias

---

## 🛠 Por hacer / mejoras sugeridas

- Validación con FluentValidation
- AutoMapper entre entidades y DTOs
- Autenticación / autorización
- Carga de imágenes de productos
- API REST (controladores Web API)

---

## 🧑‍💻 Autor

Desarrollado por 

---

## 📄 Licencia

Este proyecto está bajo la licencia MIT.
