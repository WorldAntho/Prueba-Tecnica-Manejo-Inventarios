# Prueba-Tecnica-Manejo-Inventarios

Sistema web completo para la gestión de inventarios que permite administrar productos y transacciones de manera eficiente. Desarrollado con .NET 8 para el backend, Angular para el frontend y MySQL como base de datos.

##  Características Principales

- **Gestión de Productos**: CRUD completo (Crear, Leer, Actualizar, Eliminar)
- **Gestión de Transacciones**: Control total de movimientos de inventario
- **Paginación Dinámica**: Listados optimizados con navegación por páginas
- **Filtros Avanzados**: Búsqueda y filtrado dinámico en tiempo real
- **API RESTful**: Endpoints completos para todas las operaciones
- **Interfaz Responsiva**: Diseño adaptable a diferentes dispositivos
- **Validaciones**: Mensajes de éxito y error en todas las operaciones

## Requisitos

### Requisitos del Sistema
- **Node.js**: Versión 18.x o superior
- **Angular CLI**: Versión 17.x o superior
- **.NET SDK**: Versión 8.0 o superior
- **MySQL Server**: Versión 8.0 o superior
- **Git**: Para clonar el repositorio

### Herramientas Recomendadas
- Visual Studio 2022 o Visual Studio Code
- MySQL Workbench (opcional)
- Postman (para pruebas de API)

## Configuración de la Base de Datos

1. **Instalar MySQL Server** si no lo tienes instalado
2. **Crear la base de datos** ejecutando el script SQL ubicado en la raíz del proyecto:
   ```sql
   -- Ejecutar el archivo: database_script.sql
   ```
3. **Configurar la cadena de conexión** en el archivo `appsettings.json` del backend:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Port=3306;Database=gestioninventarios;Uid=tu_usuario;Pwd=tu_contraseña;"
     }
   }
   ```

## ⚙️ Ejecución del Backend (.NET 8)

1. **Navegar al directorio del backend**:
   ```bash
   cd backend
   ```

2. **Restaurar las dependencias**:
   ```bash
   dotnet restore
   ```

3. **Aplicar migraciones** (si usas Entity Framework):
   ```bash
   dotnet ef database update
   ```

4. **Ejecutar el proyecto**:
   ```bash
   dotnet run
   ```

5. **Verificar que el backend esté funcionando**:
   - La API estará disponible en: `https://localhost:7144`
   - Swagger UI disponible en: `https://localhost:7144/swagger`

### Endpoints Principales
- **Productos**: `/api/Productos/ver-productos`
- **Transacciones**: `/api/Transacciones/ver-transacciones`
- Cada endpoint soporta operaciones GET, POST y DELETE

## Ejecución del Frontend (Angular)

1. **Navegar al directorio del frontend**:
   ```bash
   cd frontend
   ```

2. **Instalar las dependencias**:
   ```bash
   npm install
   ```

3. **Configurar la URL del backend** en `src/environments/environment.ts`:
   ```typescript
   export const environment = {
     production: false,
     apiUrl: 'https://localhost:7144/api'
   };
   ```

4. **Ejecutar el servidor de desarrollo**:
   ```bash
   ng serve
   ```

5. **Acceder a la aplicación**:
   - La aplicación estará disponible en: `http://localhost:4200`

### Comandos Adicionales
- **Build para producción**: `ng build --prod`
- **Ejecutar tests**: `ng test`
- **Linting**: `ng lint`

## Evidencias del Sistema

### 1. Listado Dinámico de Productos con Paginación
![Listado de productos](./evidences/productos-listado.png)
*Pantalla principal mostrando la tabla de productos con controles de paginación*

### 2. Listado Dinámico de Transacciones con Paginación  
![Listado de transacciones](./evidences/transacciones-listado.png)
*Pantalla de transacciones con navegación por páginas*

### 3. Creación de Productos
![Crear producto](./evidences/producto-crear.png)
*Formulario para agregar nuevos productos al inventario*

### 4. Edición de Productos
![Editar producto](./evidences/producto-editar.png)
*Formulario de edición con datos pre-cargados del producto seleccionado*

### 5. Creación de Transacciones
![Crear transacción](./evidences/transaccion-crear.png)
*Formulario para registrar nuevas transacciones de inventario*

### 6. Edición de Transacciones
![Editar transacción](./evidences/transaccion-editar.png)
*Pantalla de modificación de transacciones existentes*

### 7. Filtros Dinámicos
![Filtros dinámicos](./evidences/filtros-dinamicos.png)
*Sistema de búsqueda y filtrado en tiempo real*

### 8. Consulta de Información Detallada
![Detalle de producto](./evidences/producto-detalle.png)
*Vista detallada con información completa del elemento seleccionado*

## Tecnologías Utilizadas

### Backend
- **.NET 8**: Framework principal
- **Entity Framework Core**: ORM para base de datos
- **AutoMapper**: Mapeo de objetos
- **Swagger**: Documentación de API
- **FluentValidation**: Validaciones de modelo

### Frontend
- **Angular 17**: Framework de frontend
- **TypeScript**: Lenguaje de programación
- **Angular Material**: Componentes UI
- **RxJS**: Programación reactiva
- **Bootstrap**: Framework CSS

### Base de Datos
- **MySQL 8.0**: Sistema de gestión de base de datos
- **MySQL Connector/NET**: Driver de conexión

## Estructura del Proyecto

```
Prueba-Tecnica-Manejo-Inventarios/
├── backend/
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Data/
│   └── appsettings.json
├── frontend/
│   ├── src/
│   │   ├── app/
│   │   ├── environments/
│   │   └── assets/
│   ├── angular.json
│   └── package.json
├── evidences/
│   └── [capturas de pantalla]
├── database_script.sql
└── README.md
```

## Estado del Proyecto

✅ Listado dinámico de productos y transacciones con paginación  
✅ Pantallas de inserción y modificación  
✅ APIs completas (CRUD)  
✅ Mensajes de éxito y error  
✅ Tablas dinámicas con paginación  
✅ Filtros dinámicos  
✅ Documentación completa  

## Contribución

Si deseas contribuir al proyecto:

1. Fork el repositorio
2. Crea una rama para tu feature (`git checkout -b feature/nueva-funcionalidad`)
3. Commit tus cambios (`git commit -am 'Agregar nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Abre un Pull Request

## Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## Contacto

Para preguntas o sugerencias sobre el proyecto, puedes contactar al desarrollador a través del repositorio de GitHub.

---

**Nota**: Asegúrate de tener todos los requisitos instalados antes de ejecutar el proyecto. Las capturas de pantalla en la sección de evidencias muestran el funcionamiento completo del sistema.