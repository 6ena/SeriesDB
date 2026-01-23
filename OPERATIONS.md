# SeriesDB

## Operaciones del Sistema
Estas operaciones describen las funcionalidades principales que debe tener la aplicación, así como los procesos necesarios para cumplir con los requerimientos del sistema

### 1. Búsqueda de Series
	* 1.1 Buscar series: Permite a los usuarios buscar series mediante título o género, utilizando la API externa (OMDB).
### 2. Gestión de Series
	* 2.1 Obtener información de series: Obtiene de la base de datos interna la información de las series obtenidas de la API, incluyendo título, género, fecha de lanzamiento, duración, equipo, foto de portada, país de origen y calificación en IMDB.
	* 2.2 Persistir información de series: Guarda en la base de datos interna la información de las series obtenidas de la API, incluyendo título, género, fecha de lanzamiento, duración, equipo, foto de portada, país de origen y calificación en IMDB.
### 3. Lista de Seguimiento
	* 3.1 Obtiener las series de lista de seguimiento: Permite a los usuarios obtener la informacion de las series que estan en lista de seguimiento
	* 3.2 Agregar series a la lista de seguimiento: Permite a los usuarios agregar series a su lista de seguimiento para recibir notificaciones sobre cambios relevantes.
	* 3.4 Eliminar series de la lista de seguimiento: Permite a los usuarios eliminar series de su lista de seguimiento.
### 4. Notificaciones
	* 4.1 Mostrar notificaciones en la pantalla principal: Muestra notificaciones sobre cambios relevantes en las series de la lista de seguimiento en la pantalla principal, diferenciando entre leídas y no leídas.
	* 4.2 Enviar notificaciones por email: Envío de notificaciones por correo electrónico al usuario.
	* 4.3 Configuración de notificaciones: Permite al usuario seleccionar el tipo de notificaciones que desea recibir.
	* 4.4 Generar notificaciones: Se generan los datos de las notificaciones a enviar en base a los cambios en las series de la lista de seguimiento. Este proceso debe ejecutarse periodicamente por parte del sistema sin intervención del usuario pesistiendo en la base de datos las notificaciones a enviar por los diferentes métodos existentes.
### 5. Calificación de Series
	* 5.1 Calificar series: Permite a los usuarios calificar las series con una puntuación de 1 a 5 y agregar comentarios opcionales.
	* 5.2 Modificar calificación: Permite a los usuarios editar la calificación y los comentarios de las series.
### 6. Autenticación
	* 6.1 Inicio de sesión: Solicita nombre de usuario y contraseña para acceder a la aplicación.
### 7. Funcionalidades de Administrador
	* 7.1 Administrar usuarios: Acceso a todas las funcionalidades de gestión de usuarios.
	* 7.2 Panel de monitoreo de API: Visualización de estadísticas de acceso a la API, tiempos de respuesta, cantidad de errores, etc.
	* 7.3 Generación de bitácora de monitoreo: Registro de eventos en un archivo de log para diagnósticos de errores.
### 8. Administración de Usuarios
	* 8.1 Registrar nuevo usuario: Permite al administrador crear un nuevo usuario con nombre de usuario, nombre completo, contraseña y foto de perfil.
	* 8.2 Eliminar usuario: Permite al administrador eliminar usuarios existentes.
	* 8.3 Visualizar usuarios: Permite al administrador visualizar los datos completos de todos los usuarios. Los usuarios no administradores solo pueden ver el nombre de otros usuarios.
	* 8.4 Modificar perfil de usuario: Permite a cada usuario modificar su propio perfil (nombre completo, contraseña, foto de perfil).
